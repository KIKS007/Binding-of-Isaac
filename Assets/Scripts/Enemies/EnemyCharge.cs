using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class EnemyCharge : Enemy 
{
	[Header ("Charge")]
	public Vector2 chargeCooldown = new Vector2(1, 3);
	public float chargeSpeed = 5;
	public float chargeDuration;
	public Ease chargeEase;

	[Header ("Layer")]
	public LayerMask playerLayer;

	private Vector2 chargeDirection;
	private float chargeSpeedTemp;
	private bool charging = false;
	private bool detecting = true;

	void Update ()
	{
		if (detecting)
			DetectPlayer ();
	}

	void FixedUpdate ()
	{
		if(charging)
			rigidBody.MovePosition (rigidBody.position + chargeDirection * chargeSpeedTemp * Time.fixedDeltaTime);
	}

	void LookAtDirection ()
	{
		Vector3 dir = player.transform.position - transform.position; 
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; 
		Quaternion newRot = Quaternion.AngleAxis(angle, Vector3.forward);

		transform.DORotate (newRot.eulerAngles, 0.5f);
	}

	void DetectPlayer ()
	{
		if (Physics2D.Raycast (transform.position, Vector2.right, 100, playerLayer))
			Charge(Vector2.right);

		else if (Physics2D.Raycast (transform.position, Vector2.down, 100, playerLayer))
			Charge(Vector2.down);

		else if (Physics2D.Raycast (transform.position, Vector2.left, 100, playerLayer))
			Charge(Vector2.left);

		else if (Physics2D.Raycast (transform.position, Vector2.up, 100, playerLayer))
			Charge(Vector2.up);
	}

	void Charge (Vector2 direction)
	{
		detecting = false;
		charging = true;

		chargeDirection = direction.normalized;

		chargeSpeedTemp = chargeSpeed;
		DOTween.To (()=> chargeSpeedTemp, x=> chargeSpeedTemp = x, 0, chargeDuration).SetEase (chargeEase).OnComplete (()=> charging = false).SetId ("Charge" + GetInstanceID ());

		StartCoroutine (ChargeCooldown ());
	}

	IEnumerator ChargeCooldown ()
	{
		yield return new WaitWhile (()=> charging);

		float cooldown = Random.Range (chargeCooldown.x, chargeCooldown.y);

		yield return new WaitForSeconds (cooldown);

		detecting = true;
	}

	protected override void OnCollisionEnter2D (Collision2D other)
	{
		base.OnCollisionEnter2D (other);

		if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
		{
			DOTween.Kill ("Charge" + GetInstanceID ());
			charging = false;
			rigidBody.velocity = Vector2.zero;
		}
	}
}
