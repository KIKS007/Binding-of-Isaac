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
	public GameObject[] enemiesPrefabs = new GameObject[3]; 
	public Slider maxEnemies;

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

		isaacFire.currentTearDrop = TearDropType.Basic;

		maxEnemies.value = (float)EnemySpawner.Instance.currentMaxEnemies;
		maxEnemies.maxValue = (float)EnemySpawner.Instance.maxMaxEnemies;

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

	public void UpdateEnemy (int enemy)
	{
		if(EnemySpawner.Instance.enemiesPrefabs.Contains (enemiesPrefabs [enemy]))
			EnemySpawner.Instance.enemiesPrefabs.Remove (enemiesPrefabs [enemy]);

		else
			EnemySpawner.Instance.enemiesPrefabs.Add (enemiesPrefabs [enemy]);
	}

	public void UpdateMaxEnemies ()
	{
		EnemySpawner.Instance.currentMaxEnemies = (int)maxEnemies.value;
	}

	public void TeardropType (int type)
	{
		tearDropType = (TearDropType)type;

		isaacFire.currentTearDrop = (TearDropType)type;
	}

	public void Show ()
	{
		mainContent.DOAnchorPosX (panelPositions.x, movementDuration).SetEase (movementEase).SetUpdate (UpdateType.Normal, true);
	}

	public void Hide ()
	{
		mainContent.DOAnchorPosX (panelPositions.y, movementDuration).SetEase (movementEase).SetUpdate (UpdateType.Normal, true);
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
		Time.timeScale = 1;
		Vector3 initialScale = isaac.transform.localScale;

		Tween tween = isaac.transform.DOScale (0, 0.2f);

		yield return tween.WaitForCompletion ();
		isaac.SetActive (false);
		isaacHealth.health = (int)maxHealth.value;


		int randomSpawn = 0;
		Collider2D collider = new Collider2D ();

		if(Physics2D.OverlapCircle (Vector2.zero, 1, EnemySpawner.Instance.spawnLayer) == null)
			isaac.transform.position = Vector2.zero;

		else
		{
			do
			{
				randomSpawn = Random.Range (0, EnemySpawner.Instance.spawns.Length);
				collider = Physics2D.OverlapCircle (EnemySpawner.Instance.spawns [randomSpawn].transform.position, 1, EnemySpawner.Instance.spawnLayer);
				//Debug.Log (collider);
			}
			
			while (collider != null);
			
			isaac.transform.position = EnemySpawner.Instance.spawns [randomSpawn].transform.position;			
		}

		isaac.SetActive (true);
		isaac.transform.DOScale (initialScale, 0.2f);
		Setup ();

	}
}
