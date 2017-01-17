using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EnemyDash : Enemy 
{
	[Header ("Attack")]
	public int attackingPlayerChance = 30;

	[Header ("Dash")]
	public Vector2 dashCooldown = new Vector2(1, 3);
	public float dashLength;
	public float dashSpeed = 5;
	public float dashDuration;
	public Ease dashEase;

	[Header ("Obstacles")]
	public LayerMask obstaclesLayer;

	private Vector2 dashDirection;
	private float dashSpeedTemp;
	private bool dashing = false;

	protected override void Start ()
	{
		base.Start ();
		StartCoroutine (DashCooldown ());
	}

	void FixedUpdate ()
	{
		if(dashing)
		rigidBody.MovePosition (rigidBody.position + dashDirection * dashSpeedTemp * Time.fixedDeltaTime);
	}

	void LookAtDirection ()
	{
		Vector3 dir = new Vector3 (dashDirection.x, dashDirection.y, 0) - transform.position; 
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; 
		Quaternion newRot = Quaternion.AngleAxis(angle, Vector3.forward);

		transform.DORotate (newRot.eulerAngles, 0.5f);
	}

	void Dash ()
	{
		dashing = true;
		RaycastHit2D raycastHit = new RaycastHit2D ();

		if(Random.Range (0, 100) < attackingPlayerChance && player != null)
		{
			//Debug.Log("Attacking Player");

			dashDirection = new Vector2 (player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
			dashDirection = dashDirection.normalized * dashLength;

			Debug.DrawRay (transform.position, dashDirection * 2, Color.red, 0.5f);
		}

		else
		{
			do
			{
				dashDirection = new Vector2 (Random.Range (-1f, 1f), Random.Range (-1f, 1f));
				dashDirection = dashDirection.normalized * dashLength;
				
				Debug.DrawRay (transform.position, dashDirection * 2, Color.red, 0.5f);
				
				raycastHit = Physics2D.Raycast (transform.position, dashDirection, dashLength * 2, obstaclesLayer);
				
			}
			while(raycastHit.collider);		
		}

		LookAtDirection ();

		dashSpeedTemp = dashSpeed;
		DOTween.To (()=> dashSpeedTemp, x=> dashSpeedTemp = x, 0, dashDuration).SetEase (dashEase).OnComplete (()=> dashing = false).SetId ("Dash" + GetInstanceID ());
	}

	IEnumerator DashCooldown ()
	{
		yield return new WaitWhile (()=> dashing);

		float cooldown = Random.Range (dashCooldown.x, dashCooldown.y);

		yield return new WaitForSeconds (cooldown);

		Dash ();

		StartCoroutine (DashCooldown ());
	}

	protected override void OnCollisionEnter2D (Collision2D other)
	{
		base.OnCollisionEnter2D (other);

		if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
		{
			DOTween.Kill ("Dash" + GetInstanceID ());
			dashing = false;
			rigidBody.velocity = Vector2.zero;
		}
	}
}
