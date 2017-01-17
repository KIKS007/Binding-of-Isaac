using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Teardrop : MonoBehaviour 
{
	public TearDropType tearDropType;

	[Header ("Damage")]
	public int damage = 5;

	[Header ("Fire Rate")]
	public float fireRate;

	[Header ("Fire Details")]
	public float fireSpeed;
	public float fireRange;
	public float fireEndDuration;

	protected Ease fireRangeEase = Ease.Linear;

	protected Rigidbody2D tearDropRigidbody;

	private Tween tween;
	protected bool dead = false;
	protected bool travelEnded = false;

	protected virtual void Awake ()
	{
		tearDropRigidbody = GetComponent<Rigidbody2D> ();
	}

	protected virtual void FixedUpdate ()
	{
		tearDropRigidbody.MovePosition (tearDropRigidbody.position + new Vector2 (transform.right.x, transform.right.y) * fireSpeed * Time.fixedDeltaTime);
	}

	protected virtual void LookAt (Vector2 target)
	{
		float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	public virtual void Fire (Vector2 fireDirection)
	{
		LookAt (fireDirection);

		StartCoroutine (FireRange (tearDropRigidbody));
	}

	protected virtual IEnumerator FireRange (Rigidbody2D tearDropRigidbody)
	{
		Vector3 lastPosition = transform.position;
		float distanceTravelled = 0;

		do
		{
			distanceTravelled += Vector3.Distance(transform.position, lastPosition);
			lastPosition = transform.position;

			yield return new WaitForEndOfFrame();
		}
		while(distanceTravelled < fireRange);

		travelEnded = true;

		tween = DOTween.To (()=> fireSpeed, x=> fireSpeed = x, 0, fireEndDuration).SetEase (fireRangeEase).OnComplete (()=> 
			{ 
				if(!dead && gameObject != null) 
					Kill ();
			});
	}

	public virtual void Kill ()
	{
		StartCoroutine (KillCoroutine ());
	}

	IEnumerator KillCoroutine ()
	{
		dead = true;
		DOTween.Kill (tween);

		transform.DOScale (0, 0.1f);
		GetComponent<Collider2D> ().enabled = false;

		yield return new WaitForSeconds (0.1f);

		Destroy (gameObject);
	}

	protected virtual void OnCollisionEnter2D (Collision2D other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			other.gameObject.GetComponent<Enemy> ().Damage (damage);
			Kill ();
		}

		if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Enemy")
			Kill ();
	}

}
