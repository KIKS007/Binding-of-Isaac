﻿using UnityEngine;
using System.Collections;

public class EnemyFollow : Enemy {

	public float speed = 5;

	public float rotationLerp = 0.1f;

	public bool followPlayer = true;

	void FixedUpdate ()
	{
		if (followPlayer)
			FollowPlayer ();
	}

	void FollowPlayer ()
	{
		LookAtPlayer ();

		rigidBody.MovePosition (rigidBody.position + new Vector2 (transform.right.x, transform.right.y) * speed * Time.fixedDeltaTime);
	}

	void LookAtPlayer ()
	{
		Vector3 dir = player.transform.position - transform.position; 
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; 
		Quaternion newRot = Quaternion.AngleAxis(angle, Vector3.forward);

		transform.rotation = Quaternion.Slerp(transform.rotation, newRot, rotationLerp);
	}
}
