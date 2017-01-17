using UnityEngine;
using System.Collections;

public class EnemyFollow : Enemy {

	[Header ("Follow")]
	public float speed = 5;
	public float rotationLerp = 0.1f;
	public bool followPlayer = true;

	void FixedUpdate ()
	{
		if (followPlayer && player != null)
			FollowPlayer ();
	}

	void FollowPlayer ()
	{
		Vector3 dir = player.transform.position - transform.position; 
		rigidBody.MovePosition (rigidBody.position + new Vector2 (dir.x, dir.y).normalized * speed * Time.fixedDeltaTime);
	}
}
