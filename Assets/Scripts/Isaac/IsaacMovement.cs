using UnityEngine;
using System.Collections;

public class IsaacMovement : MonoBehaviour 
{
	[Header ("Movement")]
	public float movementSpeed = 2;

	private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () 
	{
		rigidBody = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate ()
	{
		Vector2 movement = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		movement.Normalize ();

		rigidBody.MovePosition (rigidBody.position + movement * movementSpeed * Time.fixedDeltaTime);
	}
}
