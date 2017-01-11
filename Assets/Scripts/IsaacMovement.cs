using UnityEngine;
using System.Collections;

public class IsaacMovement : MonoBehaviour 
{
	public float health = 100;

	public float movementSpeed = 2;

	private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () 
	{
		rigidBody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void FixedUpdate ()
	{
		Vector2 movement = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		movement.Normalize ();

		rigidBody.MovePosition (rigidBody.position + movement * movementSpeed * Time.fixedDeltaTime);
	}
}
