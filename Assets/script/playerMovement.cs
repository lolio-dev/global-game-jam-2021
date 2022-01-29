using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class playerMovement : MonoBehaviour
{
	public Rigidbody2D rb;

	public ParticleSystem particles;
	public GameObject gbParticles;
	private ParticleSystem particles;
	public GameObject playerTagObject;

	[FormerlySerializedAs("horizontaleInputRef")]
	public string horizontalInputRef;

	[FormerlySerializedAs("verticallInputRef")]
	public string verticalInputRef;
	private AudioSource audio;
	public AudioClip frottementSound;
	public AudioClip sautSound;

	public string horizontaleInputRef;
	public string verticallInputRef;

	[Range(4f, 8f)]
	public float moveSpeed = 6f;

	[Range(5f, 10f)]
	public float initialJumpSpeed = 8f;


	/* State */

	private float velocityXDampingCurrentVelocity = 0f;

	private float wantedHorizontalSpeed;

	private List<Collider2D> currentGrounds = new List<Collider2D>();
	private bool IsGrounded => currentGrounds.Count > 0;

	private bool isJumping;

	
	void Start()
	{
		particles = gbParticles.GetComponent<ParticleSystem>();
		audio = gameObject.GetComponent<AudioSource>();
	}

	void Update()
	{
		wantedHorizontalSpeed = Input.GetAxis(horizontalInputRef) * moveSpeed;

		if (Input.GetButtonDown(verticalInputRef) && IsGrounded)
		{
			isJumping = true;
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
		if (wantedHorizontalSpeed != 0 && IsGrounded == true)
		{
			particles.gameObject.SetActive(true);
		}
		else
		{
			particles.gameObject.SetActive(false);
		}

		//Play Sound
		if (Input.GetButtonDown(horizontaleInputRef) && isGrounded == true)
		{
			audio.Play();
		}
		else if (Input.GetButtonUp(horizontaleInputRef) || isGrounded == false)
		{
			audio.Stop();
		}
	}

	void FixedUpdate()
	{
		MovePlayer(wantedHorizontalSpeed);
	}

	void MovePlayer(float _wantedHorizontalSpeed)
	{
		var oldVelocity = rb.velocity;

		// Quickly reach target velocity X
		float newVelocityX = Mathf.SmoothDamp(oldVelocity.x, _wantedHorizontalSpeed, ref velocityXDampingCurrentVelocity, .05f);

		float newVelocityY;

		if (isJumping)
		{
			// Instantly apply jump velocity on Y
			newVelocityY = initialJumpSpeed;
			isJumping = false;
		}
		else
		{
			// Keep last velocity Y
			newVelocityY = oldVelocity.y;
		}

		rb.velocity = new Vector2(newVelocityX, newVelocityY);
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		audio.PlayOneShot(sautSound);
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
}