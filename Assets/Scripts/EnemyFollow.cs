using UnityEngine;
using System.Collections;

public class EnemyFollow : Enemy {

	[Header ("Follow")]
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
		//LookAtPlayer ();

		Vector3 dir = player.transform.position - transform.position; 
		rigidBody.MovePosition (rigidBody.position + new Vector2 (dir.x, dir.y) * speed * Time.fixedDeltaTime);
	}

	void LookAtPlayer ()
	{
		Vector3 dir = player.transform.position - transform.position; 
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; 
		Quaternion newRot = Quaternion.AngleAxis(angle, Vector3.forward);

		transform.rotation = Quaternion.Slerp(transform.rotation, newRot, rotationLerp);
	}
}
