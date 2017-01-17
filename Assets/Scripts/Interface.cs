using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Interface : MonoBehaviour 
{
	public static Interface Instance;

	public RectTransform mainContent;

	[Header ("Show / Hide")]
	public Vector2 panelPositions;
	public float movementDuration;
	public Ease movementEase;

	[Header ("Health")]
	public Slider currentHealth;

	[Header ("Player")]
	public Slider movementSpeed;
	public Slider maxHealth;

	[Header ("Shoot")]
	public Slider fireRate;
	public Slider fireSpeed;
	public Slider fireRange;
	public Slider fireDamage;

	[Header ("Tears Type")]
	public TearDropType tearDropType;

	[Header ("Enemies")]
	public Toggle follow;
	public Toggle dash;
	public Toggle charge;

	private GameObject isaac;
	private IsaacFire isaacFire;
	private IsaacHealth isaacHealth;
	private IsaacMovement isaacMovement;

	// Use this for initialization
	void Start () 
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy (this);

		Setup ();
	}

	void Setup ()
	{
		isaac = GameObject.FindGameObjectWithTag ("Player");

		isaacFire = isaac.GetComponent<IsaacFire> ();
		isaacHealth = isaac.GetComponent<IsaacHealth> ();
		isaacMovement = isaac.GetComponent<IsaacMovement> ();

		currentHealth.maxValue = maxHealth.value;
		currentHealth.value = maxHealth.value;

		isaacHealth.health = (int)maxHealth.value;
		movementSpeed.value = isaacMovement.movementSpeed;

		UpdateIsaac ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void UpdateIsaac ()
	{
		isaacFire.currentFireRate = fireRate.value;

		for(int i = 0; i < isaacFire.tearDrops.Length; i++)
		{
			Teardrop tearScript = isaacFire.tearDrops [i].GetComponent<Teardrop> ();

			tearScript.fireSpeed = fireSpeed.value;
			tearScript.fireRate =  1 / fireRate.value;
			tearScript.fireRange = fireRange.value;
			tearScript.damage = (int)fireDamage.value;
		}

		isaacFire.currentFireRate = 1 / fireRate.value;
		isaacMovement.movementSpeed = movementSpeed.value;
	}

	public void TeardropType (int type)
	{
		tearDropType = (TearDropType)type;
	}

	public void Show ()
	{
		mainContent.DOAnchorPosX (panelPositions.x, movementDuration).SetEase (movementEase);
	}

	public void Hide ()
	{
		mainContent.DOAnchorPosX (panelPositions.y, movementDuration).SetEase (movementEase);
	}

	public void TogglePause ()
	{
		if (Time.timeScale == 0)
			Time.timeScale = 1;
		else
			Time.timeScale = 0;
	}

	public void Spawn ()
	{
		StartCoroutine (SpawnCoroutine ());
	}

	IEnumerator SpawnCoroutine ()
	{
		Vector3 initialScale = isaac.transform.localScale;

		Tween tween = isaac.transform.DOScale (0, 0.2f);

		yield return tween.WaitForCompletion ();
		isaac.SetActive (false);
		isaacHealth.health = (int)maxHealth.value;

		isaac.transform.position = Vector3.zero;

		isaac.SetActive (true);
		isaac.transform.DOScale (initialScale, 0.2f);
		Setup ();
	}
}
