using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	public float health = 100;

	protected Rigidbody2D rigidBody;

	protected Transform player;

	// Use this for initialization
	protected virtual void Start () 
	{
		rigidBody = GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	public virtual void Damage (float damage)
	{
		health -= damage;

		if (health <= 0)
			Death ();
	}

	protected virtual void Death ()
	{
		Destroy (gameObject);
	}
}
