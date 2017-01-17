using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeardropDouble : Teardrop 
{
	[Header ("Double")]
	public float tearsAngles = 10;

	public bool spawnOtherTear = true;

	void Start () 
	{
		if(spawnOtherTear)
		{
			GameObject clone = Instantiate (gameObject, transform.position, transform.rotation) as GameObject;
			clone.GetComponent<TeardropDouble> ().spawnOtherTear = false;
			transform.Rotate (0, 0, tearsAngles);
		}
		else
			transform.Rotate (0, 0, -tearsAngles);
	}
}
