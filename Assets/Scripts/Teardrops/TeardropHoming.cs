using UnityEngine;
using System.Collections;

public class TeardropHoming : Teardrop 
{
	[Header ("Homing")]
	public bool homing = false;
	public float rotationLerp = 0.1f;

	private Transform enemy;
	
	// Update is called once per frame
	void Update () 
	{
		if(homing && enemy)
		{
			Vector3 dir = enemy.transform.position - transform.position; 
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; 
			Quaternion newRot = Quaternion.AngleAxis(angle, Vector3.forward);

			transform.rotation = Quaternion.Slerp(transform.rotation, newRot, rotationLerp);			
		}
	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		if(!homing && collider.tag == "Enemy")
		{
			enemy = collider.transform;
			homing = true;
		}
	}

	void OnTriggerExit2D (Collider2D collider)
	{
		if(homing && collider.gameObject == enemy)
			homing = false;
	}
}
