using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsaacHealth : MonoBehaviour 
{
	[Header ("Health")]
	public int health = 100;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Damage (int damage)
	{
		health -= damage;

		if (health <= 0)
			Death ();
	}

	public void Death ()
	{
		Destroy (gameObject);
	}
}
