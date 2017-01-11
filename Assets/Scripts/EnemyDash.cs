using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EnemyDash : Enemy 
{
	public Vector2 dashCooldown = new Vector2(1, 3);

	public float dashLength;

	public float dashSpeed = 5;

	public float dashDuration;

	public Ease dashEase;

	public LayerMask wallLayer;

	private Vector2 dashDirection;

	private float dashSpeedTemp;

	protected override void Start ()
	{
		base.Start ();
		StartCoroutine (DashCooldown ());
	}

	void FixedUpdate ()
	{
		rigidBody.MovePosition (rigidBody.position + dashDirection * dashSpeedTemp * Time.fixedDeltaTime);
	}

	void Dash ()
	{
		RaycastHit2D raycastHit = new RaycastHit2D ();


			dashDirection = new Vector2 (Random.Range (-1f, 1f), Random.Range (-1f, 1f));
			dashDirection = dashDirection.normalized * dashLength;

			raycastHit = Physics2D.Linecast (transform.position, dashDirection, wallLayer);
			Debug.Log (raycastHit.collider);


		dashSpeedTemp = dashSpeed;
		DOTween.To (()=> dashSpeedTemp, x=> dashSpeedTemp = x, 0, dashDuration).SetEase (dashEase);
	}

	IEnumerator DashCooldown ()
	{
		float cooldown = Random.Range (dashCooldown.x, dashCooldown.y);

		yield return new WaitForSeconds (cooldown);

		Dash ();

		StartCoroutine (DashCooldown ());
	}
}
