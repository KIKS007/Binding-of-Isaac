using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Teardrop : MonoBehaviour 
{
	public float damage = 5;

	public float fireSpeed;

	public float fireDuration;
	public float fireEndDuration;
	public Ease fireRangeEase;

	protected Rigidbody2D tearDropRigidbody;

	private Tween tween;

	protected virtual void FixedUpdate ()
	{
		tearDropRigidbody.MovePosition (tearDropRigidbody.position + new Vector2 (transform.right.x, transform.right.y) * fireSpeed * Time.fixedDeltaTime);
	}

	protected virtual void LookAt (Vector2 target)
	{
		tearDropRigidbody = GetComponent<Rigidbody2D> ();

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
		yield return new WaitForSeconds (fireDuration);

		tween = DOTween.To (()=> fireSpeed, x=> fireSpeed = x, 0, fireEndDuration).SetEase (fireRangeEase).OnComplete (()=> 
			{ if(gameObject) 
				Destroy (gameObject);
			});
	}

	public virtual void Kill ()
	{
		DOTween.Kill (tween);
		Destroy (gameObject);
	}

	protected virtual void OnCollisionEnter2D (Collision2D other)
	{
		if(other.gameObject.tag == "Enemy")
			other.gameObject.GetComponent<Enemy> ().Damage (damage);

		if(other.gameObject.tag == "Teardrop" || other.gameObject.tag == "Wall" || other.gameObject.tag == "Enemy")
			Kill ();
	}

}
