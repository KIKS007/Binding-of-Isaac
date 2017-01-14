using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeardropZigZag : Teardrop 
{
	public enum Direction {RightLeft, DownUp};

	[Header ("ZigZag")]
	public Vector2 zigzagLength;
	public Direction direction;

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
	}

	void Zigzag ()
	{
		
	}
}
