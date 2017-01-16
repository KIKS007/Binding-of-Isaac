using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	[Header ("Health")]
	public int health = 100;

	[Header ("Damage")]
	public int damage = 2;

	protected Rigidbody2D rigidBody;

	protected Transform player;

	// Use this for initialization
	protected virtual void Start () 
	{
		rigidBody = GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	public virtual void Damage (int damage)
	{
		health -= damage;

		if (health <= 0)
			Death ();
	}

	protected virtual void Death ()
	{
		Destroy (gameObject);
	}

	protected virtual void OnCollisionEnter2D (Collision2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			player.gameObject.GetComponent<IsaacHealth> ().Damage (damage);
		}
	}
}
