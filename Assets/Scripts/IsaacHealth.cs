using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IsaacHealth : MonoBehaviour 
{
	[Header ("Health")]
	public int health = 100;

	[Header ("Safe")]
	public float safeDuration = 2;

	private SpriteRenderer head;
	private Vector3 initialScale;

	private bool safe = false;

	// Use this for initialization
	void Start () {
		head = transform.GetChild (0).GetComponent<SpriteRenderer> ();
		initialScale = transform.localScale;

		health = (int)Interface.Instance.maxHealth.value;
	}

	public void Damage (int damage, Transform enemy)
	{
		if(!safe)
		{
			head.DOColor (Color.red, 0.2f).OnComplete (()=> head.DOColor (Color.white, 0.1f));
			transform.DOScale (1, 0.2f).SetRelative ().SetEase (Ease.OutElastic).OnComplete (()=> transform.DOScale (initialScale, 0.1f)).SetId ("FX" + GetInstanceID ());
			
			health -= damage;
			
			if (health <= 0)
				Death ();

			StartCoroutine (Safe ());
		}

	}

	IEnumerator Safe ()
	{
		safe = true;

		yield return new WaitForSeconds (safeDuration);

		safe = false;
	}

	public void Death ()
	{
		DOTween.Kill ("FX" + GetInstanceID ());
		Destroy (gameObject);
	}
}
