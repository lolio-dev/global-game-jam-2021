using System;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
	public Rigidbody2D rb;

	public string horizontaleInputRef;
	public string verticallInputRef;

	public float moveSpeed;
	public float jumpForce;
	
	private Vector3 velocity = Vector3.zero;

	private float horizontalMovement;

	private bool isGrounded;
	private bool isJumping;
	
	void Start()
	{
		
	}

	void Update()
	{	
		horizontalMovement = Input.GetAxis(horizontaleInputRef) * moveSpeed * Time.fixedDeltaTime;
		
		if (Input.GetButtonDown(verticallInputRef) && isGrounded)
		{
			isJumping = true;
		}

		if (horizontalMovement > 0 && transform.localScale.x < 0)
		{
			transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
		}
		else if (horizontalMovement < 0 && transform.localScale.x > 0)
		{
			transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
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
		if (col.contacts[0].point.y > col.collider.bounds.center.y)
		{
			isGrounded = true;
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("world"))
		{
			isGrounded = false;
		}
	}
}