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

		StartCoroutine (OnMovementChange (movementState));
		StartCoroutine (OnFireChange (fireState));
	}
	
	// Update is called once per frame
	void Update () 
	{
		GetFireDirection ();

		GetMovementDirection ();
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


	void SendXMovement ()
	{
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

	}

	void SendYMovement ()
	{
		if(movementState == MovementState.Up || movementState == MovementState.Down)
			bodyAnimator.SetBool ("yMovement", true);

		else
			bodyAnimator.SetBool ("yMovement", false);
	}


	IEnumerator OnMovementChange (MovementState previousState)
	{
		SendXMovement ();
		SendYMovement ();

		yield return new WaitUntil (() => previousState != movementState);

		StartCoroutine (OnMovementChange (movementState));
	}

	IEnumerator OnFireChange (FireState previousState)
	{
		SetHead ();

		yield return new WaitUntil (() => previousState != fireState);

		StartCoroutine (OnFireChange (fireState));
	}


	void SetHead ()
	{
		if(fireState != FireState.Idle)
		{
			switch(fireState)
			{
			case FireState.Down:
				headAnimator.SetTrigger ("Head_Down");
				break;
			case FireState.Up:
				headAnimator.SetTrigger ("Head_Up");
				break;
			case FireState.Right:
				headAnimator.SetTrigger ("Head_Right");
				break;
			case FireState.Left:
				headAnimator.SetTrigger ("Head_Left");
				break;
			case FireState.Idle:
				headAnimator.SetTrigger ("Head_Idle");
				break;
			}			
		}
		else
		{
			switch(movementState)
			{
			case MovementState.Down:
				headAnimator.SetTrigger ("Head_Down");
				break;
			case MovementState.Up:
				headAnimator.SetTrigger ("Head_Up");
				break;
			case MovementState.Right:
				headAnimator.SetTrigger ("Head_Right");
				break;
			case MovementState.Left:
				headAnimator.SetTrigger ("Head_Left");
				break;
			case MovementState.Idle:
				headAnimator.SetTrigger ("Head_Idle");
				break;
			}	
		}
	}

	void SetBody ()
	{
		switch(movementState)
		{
		case MovementState.Down:
			//bodyAnimator.SetTrigger ("Body_UpDown");
			break;
		case MovementState.Up:
			//bodyAnimator.SetTrigger ("Body_UpDown");
			break;
		case MovementState.Right:
			//bodyAnimator.SetTrigger ("Body_LeftRight");
			bodyRend.flipX = false;
			break;
		case MovementState.Left:
			//bodyAnimator.SetTrigger ("Body_LeftRight");
			bodyRend.flipX = true;
			break;
		case MovementState.Idle:
			//bodyAnimator.SetTrigger ("Body_Idle");
			bodyRend.flipX = false;
			bodyRend.flipY = false;
			break;
		}
	}
}
