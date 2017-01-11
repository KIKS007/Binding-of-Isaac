using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Xml.Linq;

public class IsaacFire : MonoBehaviour 
{
	public GameObject tearDrop;

	public float fireRate;

	public bool canFire = true;

	public Transform eyes;
	public Transform leftEye;
	public Transform rightEye;
	public Transform oneEye;

	public Vector2 fireDirection;

	private bool fireOnLeftEye = true;
	
	// Update is called once per frame
	void Update () 
	{
		GetFireDirection ();

		//LookAtFireDirection ();

		if(Input.GetAxisRaw ("FireHorizontal") != 0 || Input.GetAxisRaw ("FireVertical") != 0)
		{
			if (canFire)
				Fire ();			
		}
	}

	void LookAtFireDirection ()
	{
		float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg; 
		angle -= 90;
		eyes.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	void TeardropsDisplay (GameObject tearDrop)
	{
		SpriteRenderer spriteRend = tearDrop.GetComponent<SpriteRenderer> ();

		//One Eye - Right
		if (fireDirection.x > 0 && fireDirection.y == 0)
		{
			if (fireOnLeftEye)
				spriteRend.sortingOrder = -1;
			else
				spriteRend.sortingOrder = 1;
		}

		//One Eye - Left
		else if (fireDirection.x < 0 && fireDirection.y == 0)
		{
			if (fireOnLeftEye)
				spriteRend.sortingOrder = 1;
			else
				spriteRend.sortingOrder = -1;
		}

		else
		{
			if(fireDirection.y > 0)
				spriteRend.sortingOrder = -1;

			if(fireDirection.y < 0)
				spriteRend.sortingOrder = 1;

		}
	}

	void GetFireDirection ()
	{
		fireDirection = new Vector2 (Input.GetAxisRaw ("FireHorizontal"), Input.GetAxisRaw ("FireVertical"));

		if(Input.GetAxisRaw ("FireVertical") != 0)
			fireDirection = new Vector2 (0, Input.GetAxisRaw ("FireVertical"));
		
		if(Input.GetAxisRaw ("FireHorizontal") != 0 && Input.GetAxisRaw ("FireVertical") == 0)
			fireDirection = new Vector2 (Input.GetAxisRaw ("FireHorizontal"), 0);

	}

	void Fire ()
	{
		StartCoroutine (FireRate ());

		Vector3 pos = fireOnLeftEye ? leftEye.position : rightEye.position;

		if (fireDirection.x != 0 && fireDirection.y == 0)
			pos = oneEye.position;
		
		GameObject tearDropClone = Instantiate (tearDrop, pos, tearDrop.transform.rotation) as GameObject;
		tearDropClone.GetComponent<Teardrop> ().Fire (fireDirection);

		TeardropsDisplay (tearDropClone);

		fireOnLeftEye = !fireOnLeftEye;
	}
		
	IEnumerator FireRate ()
	{
		canFire = false;

		yield return new WaitForSeconds (fireRate);

		canFire = true;
	}
}
