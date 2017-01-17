using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Enemy : MonoBehaviour 
{
	[Header ("Health")]
	public int health = 100;

	[Header ("Damage")]
	public int damage = 2;

	protected Rigidbody2D rigidBody;
	protected Transform player;
	protected SpriteRenderer spriteRenderer;
	protected Vector3 initialScale;

	// Use this for initialization
	protected virtual void Start () 
	{
		rigidBody = GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		spriteRenderer = GetComponent<SpriteRenderer> ();
		initialScale = transform.localScale;
	}

	public virtual void Damage (int damage)
	{
		spriteRenderer.DOColor (Color.red, 0.2f).OnComplete (()=> spriteRenderer.DOColor (Color.white, 0.1f));
		transform.DOScale (1, 0.2f).SetRelative ().SetEase (Ease.OutElastic).OnComplete (()=> transform.DOScale (initialScale, 0.1f)).SetId ("FX" + GetInstanceID ());

		health -= damage;

		if (health <= 0)
			Death ();
	}

	protected virtual void Death ()
	{
		DOTween.Kill ("FX" + GetInstanceID ());
		Destroy (gameObject);
	}

	protected virtual void OnCollisionEnter2D (Collision2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			player.gameObject.GetComponent<IsaacHealth> ().Damage (damage, transform);
		}
	}
}
