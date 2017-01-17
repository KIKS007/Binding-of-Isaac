using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TeardropZigZag : Teardrop 
{
	public enum Direction {RightLeft, DownUp};

	[Header ("ZigZag")]
	public Direction direction;
	public Vector2 zigzagLength;
	public float zigzagDuration;
	public Ease zigzagEase;

	private float zigzagMovement;
	private float resetPosition;

	private int movementSign = 1;

	void Start ()
	{
		if (transform.right == Vector3.right)
			direction = Direction.RightLeft;
		
		else if(transform.right == Vector3.down)
			direction = Direction.DownUp;
		
		else if(transform.right == Vector3.left)
			direction = Direction.RightLeft;
		
		else if(transform.right == Vector3.up)
			direction = Direction.DownUp;

		if (direction == Direction.DownUp)
			resetPosition = tearDropRigidbody.position.x;
		else
			resetPosition = tearDropRigidbody.position.y;

		Zigzag ();
	}

	/*protected override void FixedUpdate ()
	{
		if(direction == Direction.DownUp)
			tearDropRigidbody.MovePosition (tearDropRigidbody.position + new Vector2 (zigzagMovement, 0) * fireSpeed * Time.fixedDeltaTime);

		else
			tearDropRigidbody.MovePosition (tearDropRigidbody.position + new Vector2 (0, zigzagMovement) * fireSpeed * Time.fixedDeltaTime);
	}*/

	void Zigzag ()
	{
		//DOTween.To (() => zigzagMovement, x => zigzagMovement = x, movementSign * Random.Range (zigzagLength.x, zigzagLength.y), zigzagDuration).SetEase (zigzagEase).OnComplete (Reset).SetRelative ();

		if (transform == null || travelEnded)
			return;

		movementSign *= -1;

		if(direction == Direction.DownUp)
			transform.DOMoveX (movementSign *  Random.Range (zigzagLength.x, zigzagLength.y), zigzagDuration).SetEase (zigzagEase).OnComplete (Reset).SetRelative ().SetId ("ZigZag" + GetInstanceID ());

		else
			transform.DOMoveY (movementSign * Random.Range (zigzagLength.x, zigzagLength.y), zigzagDuration).SetEase (zigzagEase).OnComplete (Reset).SetRelative ().SetId ("ZigZag" + GetInstanceID ());
		
	}

	void Reset ()
	{
		if (transform == null || travelEnded)
			return;

		//DOTween.To (() => zigzagMovement, x => zigzagMovement = x, resetPosition, zigzagDuration).SetEase (zigzagEase).OnComplete (Zigzag);

		if(direction == Direction.DownUp)
			transform.DOMoveX (resetPosition, zigzagDuration).SetEase (zigzagEase).OnComplete (Zigzag).SetId ("ZigZag" + GetInstanceID ());

		else
			transform.DOMoveY (resetPosition, zigzagDuration).SetEase (zigzagEase).OnComplete (Zigzag).SetId ("ZigZag" + GetInstanceID ());
	}

	public override void Kill ()
	{
		base.Kill ();
		DOTween.Kill ("ZigZag" + GetInstanceID ());
	}
}
