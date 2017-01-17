using UnityEngine;
using System.Collections;

public class IsaacAnim : MonoBehaviour 
{
	public enum MovementState {Up, Down, Left, Right, Idle};
	public enum FireState {Up, Down, Left, Right, Idle};

	[Header ("States")]
	public MovementState movementState;
	public FireState fireState;

	[Header ("Animators")]
	public Animator headAnimator;
	public Animator bodyAnimator;

	[Header ("Sprites")]
	public SpriteRenderer bodyRend;

	private IsaacFire isaacFire;
	private Vector2 fireDirection;

	// Use this for initialization
	void Start () 
	{
		isaacFire = GetComponent<IsaacFire> ();
		isaacFire.OnFire += () => headAnimator.SetTrigger ("Blink");
	}
	
	// Update is called once per frame
	void Update () 
	{
		GetFireDirection ();
		GetMovementDirection ();

		SetMovementDirection ();
		SetFireDirection ();
	}

	void GetFireDirection ()
	{
		fireDirection = isaacFire.fireDirection;

		if(fireDirection.x > 0)
			fireState = FireState.Right;

		else if(fireDirection.x < 0)
			fireState = FireState.Left;

		else if(fireDirection.y > 0)
			fireState = FireState.Up;

		else if(fireDirection.y < 0)
			fireState = FireState.Down;

		else if(fireDirection.x == 0 && fireDirection.y == 0)
			fireState = FireState.Idle;
	}

	void GetMovementDirection ()
	{
		if (Input.GetAxisRaw ("Horizontal") > 0)
			movementState = MovementState.Right;

		else if (Input.GetAxisRaw ("Horizontal") < 0)
			movementState = MovementState.Left;

		else if(Input.GetAxisRaw ("Horizontal") == 0 && Input.GetAxisRaw ("Vertical") != 0)
		{
			if (Input.GetAxisRaw ("Vertical") > 0)
				movementState = MovementState.Up;

			if (Input.GetAxisRaw ("Vertical") < 0)
				movementState = MovementState.Down;
		}

		else if(Input.GetAxisRaw ("Horizontal") == 0 && Input.GetAxisRaw ("Vertical") == 0)
			movementState = MovementState.Idle;
	}


	void SetMovementDirection ()
	{
		// X Movement
		if (movementState == MovementState.Right)
		{
			bodyRend.flipX = false;
			bodyAnimator.SetBool ("xMovement", true);
		}

		else if(movementState == MovementState.Left)
		{
			bodyRend.flipX = true;
			bodyAnimator.SetBool ("xMovement", true);
		}

		else
		{
			bodyRend.flipX = false;
			bodyAnimator.SetBool ("xMovement", false);
		}

		// Y Movement
		if(movementState == MovementState.Up || movementState == MovementState.Down)
			bodyAnimator.SetBool ("yMovement", true);

		else
			bodyAnimator.SetBool ("yMovement", false);
	}

	void SetFireDirection ()
	{
		if(fireState == FireState.Idle)
		{
			headAnimator.SetInteger ("xDirection", (int)Input.GetAxisRaw ("Horizontal"));
			headAnimator.SetInteger ("yDirection", (int)Input.GetAxisRaw ("Vertical"));
			headAnimator.SetBool ("Shooting", false);
		}

		else
		{
			headAnimator.SetInteger ("xDirection", (int)fireDirection.x);
			headAnimator.SetInteger ("yDirection", (int)fireDirection.y);
			headAnimator.SetBool ("Shooting", true);
		}
	}
}
