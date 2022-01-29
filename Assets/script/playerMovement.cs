using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerMovement : MonoBehaviour
{
	public Rigidbody2D rb;

	public GameObject gbParticles;
	public ParticleSystem particles;
	public GameObject playerTagObject;

	public string horizontaleInputRef;
	public string verticallInputRef;

	public float moveSpeed;
	public float jumpForce;


	/* State */

	private Vector3 velocity = Vector3.zero;

	private float horizontalMovement;

	private List<Collider2D> currentGrounds = new List<Collider2D>();
	private bool IsGrounded => currentGrounds.Count > 0;

	private bool isJumping;


	void Start()
	{

		particles = gbParticles.GetComponent<ParticleSystem>();
	}

	void Update()
	{
		horizontalMovement = Input.GetAxis(horizontaleInputRef) * moveSpeed * Time.fixedDeltaTime;

		if (Input.GetButtonDown(verticallInputRef) && IsGrounded)
		{
			isJumping = true;
		}

		//PLayer & particles rotation + PlayerTag flip
		if (horizontalMovement > 0 && transform.localScale.x < 0)
		{
			transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
			gbParticles.transform.position = new Vector2(gbParticles.transform.position.x,
				gbParticles.transform.position.y -  0.415f);
			playerTagObject.GetComponent<SpriteRenderer>().flipX = false;
		}
		else if (horizontalMovement < 0 && transform.localScale.x > 0)
		{
			transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
			gbParticles.transform.position = new Vector2(gbParticles.transform.position.x,
				gbParticles.transform.position.y + 0.415f);
			playerTagObject.GetComponent<SpriteRenderer>().flipX = true;
		}

		//Particles activation
		if (horizontalMovement != 0 && IsGrounded == true)
		{
			gbParticles.SetActive(true);
		}
		else
		{
			gbParticles.SetActive(false);
		}
	}

	void FixedUpdate()
	{
		MovePlayer(horizontalMovement);
	}

	void MovePlayer(float _horizontalMovement)
	{
		Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
		rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);

		if (isJumping)
		{
			rb.AddForce(new Vector2(0f, jumpForce));
			isJumping = false;
		}
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
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