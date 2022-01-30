using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class playerMovement : MonoBehaviour
{
	public Rigidbody2D rb;
	public SpriteRenderer spriteRenderer;
	public Collider2D myCollider2D;

	public ParticleSystem particles;
	public GameObject playerTagObject;

	[Tooltip("Player number: should be 1 or 2")]
	public int playerNumber;

	[FormerlySerializedAs("horizontaleInputRef")]
	public string horizontalInputRef;

	[FormerlySerializedAs("verticallInputRef")]
	public string moveUpInputRef;

	public string moveDownInputRef;

	private AudioSource audioSource;
	public AudioClip frottementSound;
	public AudioClip sautSound;


	[Range(4f, 8f)]
	public float moveSpeed = 6f;

	[Range(5f, 10f)]
	public float initialJumpSpeed = 8f;


	/* State */

	private float velocityXDampingCurrentVelocity = 0f;

	private float wantedHorizontalSpeed;

	private readonly List<Collider2D> currentGrounds = new List<Collider2D>();
	private bool IsGrounded => currentGrounds.Count > 0;

	private platformMovement cachedPlatformMovement = null;

	private bool wantsToGoUp;
	private bool wantsToGoDown;

	private doorScript nearbyDoor;
	private doorScript enteredDoor;

	void Start()
	{
		audioSource = gameObject.GetComponent<AudioSource>();
	}

	void Update()
	{
		wantedHorizontalSpeed = Input.GetAxis(horizontalInputRef) * moveSpeed;

		if (Input.GetButtonDown(moveUpInputRef))
		{
			wantsToGoUp = true;
		}
		else if (Input.GetButtonDown(moveDownInputRef))
		{
			wantsToGoDown = true;
		}

		//PLayer & particles rotation + PlayerTag flip
		if (wantedHorizontalSpeed > 0 && transform.localScale.x < 0)
		{
			transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
			particles.transform.position = new Vector2(particles.transform.position.x,
				particles.transform.position.y -  0.415f);
			playerTagObject.GetComponent<SpriteRenderer>().flipX = false;
		}
		else if (wantedHorizontalSpeed < 0 && transform.localScale.x > 0)
		{
			transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
			particles.transform.position = new Vector2(particles.transform.position.x,
				particles.transform.position.y + 0.415f);
			playerTagObject.GetComponent<SpriteRenderer>().flipX = true;
		}

		//Particles activation
		if (wantedHorizontalSpeed != 0 && IsGrounded)
		{
			particles.gameObject.SetActive(true);
		}
		else
		{
			particles.gameObject.SetActive(false);
		}
	}

	void FixedUpdate()
	{
		if (enteredDoor)
		{
			HandleActionsInsideDoor();
		}
		else
		{
			MovePlayer();

			//Play Sound
			if (!audioSource.isPlaying)
			{
				if (wantedHorizontalSpeed != 0 && IsGrounded)
				{
					audioSource.Play();
				}
			}
			else
			{
				if (wantedHorizontalSpeed == 0 || !IsGrounded)
				{
					audioSource.Stop();
				}
			}
		}
	}

	void MovePlayer()
	{
		var oldVelocity = rb.velocity;

		// Quickly reach target velocity X
		float newVelocityX = Mathf.SmoothDamp(oldVelocity.x, wantedHorizontalSpeed, ref velocityXDampingCurrentVelocity, .05f);

		float newVelocityY = oldVelocity.y;

		if (wantsToGoUp)
		{
			wantsToGoUp = false;

			// Can only jump or enter door if grounded
			if (IsGrounded)
			{
				if (nearbyDoor)
				{
					// Door nearby, enter it instead of jumping
					nearbyDoor.MakeCharacterEnterDoor(this);
				}
				else
				{
					// Instantly apply jump velocity on Y
					newVelocityY = initialJumpSpeed;
				}
			}
		}

		rb.velocity = new Vector2(newVelocityX, newVelocityY);
	}

	void HandleActionsInsideDoor()
	{
		Debug.Assert(enteredDoor != null, "Missing entered door, yet we should be inside door", this);

		if (wantsToGoDown)
		{
			wantsToGoDown = false;

			// Exit door we are in
			enteredDoor.MakeCharacterExitDoor(this);
		}
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		audioSource.PlayOneShot(sautSound);
		// This check should be enough for simple rectangular colliders than cannot be touched
		// from multiple directions at once (otherwise player character may "slide" from one side to another
		// and pretend it's not on top because it collided on the side first, or reversely)
		if (col.contacts[0].point.y > col.collider.bounds.center.y)
		{
			if (!currentGrounds.Contains(col.collider))
			{
				currentGrounds.Add(col.collider);
			}
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if (currentGrounds.Contains(other.collider))
		{
			currentGrounds.Remove(other.collider);
		}
	}

	public void RegisterNearbyDoor(doorScript door)
	{
		if (nearbyDoor == door)
		{
			Debug.LogWarningFormat("Nearby door {0} already registered on player {1}, nothing to do!",
				nearbyDoor, playerNumber);
			return;
		}

		if (nearbyDoor)
		{
			Debug.LogWarningFormat("Different nearby door {0} already registered on player {1}, new door {2} will replace it!",
				nearbyDoor, playerNumber, door);
		}

		nearbyDoor = door;
	}

	public void UnregisterNearbyDoor(doorScript door)
	{
		if (!nearbyDoor)
		{
			Debug.LogWarningFormat("No nearby door registered on player {1}, no nearby door to unregister!",
				playerNumber);
			return;
		}

		if (nearbyDoor != door)
		{
			Debug.LogWarningFormat("Different nearby door registered on player {0} is {1}, instead of expected {2}. We will NOT unregister {1}!",
				playerNumber, nearbyDoor, door);
			return;
		}

		nearbyDoor = null;
	}

	public void OnEnterDoor(doorScript door)
	{
		if (enteredDoor == door)
		{
			Debug.LogWarningFormat("Entered door {0} already registered on player {1}, nothing to do!",
				enteredDoor, playerNumber);
			return;
		}

		if (enteredDoor)
		{
			Debug.LogWarningFormat("Different entered door {0} already registered on player {1}, new door {2} will replace it!",
				enteredDoor, playerNumber, door);
		}

		enteredDoor = door;

		// If you add new components, make sure to deactivate them here, except the Tag and this very playerMovement so we can still process Down to exit door
		spriteRenderer.enabled = false;
		rb.simulated = false;
		myCollider2D.enabled = false;

		currentGrounds.Clear();

		audioSource.Stop();

		particles.gameObject.SetActive(false);
	}

	public void OnExitDoor(doorScript door)
	{
		if (!enteredDoor)
		{
			Debug.LogWarningFormat("No entered door registered on player {1}, no entered door to unregister!",
				playerNumber);
			return;
		}

		if (enteredDoor != door)
		{
			Debug.LogWarningFormat("Different entered door registered on player {0} is {1}, instead of expected {2}. We will NOT unregister {1}!",
				playerNumber, enteredDoor, door);
			return;
		}

		enteredDoor = null;

		// If you add new components, make sure to reactivate them here, except the Tag and this very playerMovement
		spriteRenderer.enabled = true;
		rb.simulated = true;
		myCollider2D.enabled = true;

		particles.gameObject.SetActive(true);
	}
}