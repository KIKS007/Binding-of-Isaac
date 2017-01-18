using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour 
{
	public static EnemySpawner Instance;

	[Header ("Spawns")]
	public Transform[] spawns = new Transform[0];
	public Vector2 randomSpawnDuration;
	public LayerMask spawnLayer;

	[Header ("Enemies Prefabs")]
	public List<GameObject> enemiesPrefabs = new List<GameObject> (); 

	[Header ("Enemies")]
	public int currentMaxEnemies = 1;
	public int maxMaxEnemies = 30;
	public float waveDuration;

	[Header ("Alive Enemies")]
	public List<GameObject> aliveEnemies = new List<GameObject> ();

	[HideInInspector]
	public Transform player;

	void Awake () 
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy (this);
	}

	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		
		SpawnEnemy ();
		
		StartCoroutine (NextWave ());
		
		StartCoroutine (RandomSpawn ());
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void SpawnEnemy ()
	{
		int randomSpawn = 0;
		Collider2D collider = new Collider2D ();
		do
		{
			randomSpawn = Random.Range (0, spawns.Length);
			collider = Physics2D.OverlapCircle (spawns [randomSpawn].transform.position, 1, spawnLayer);
			//Debug.Log (collider);
		}

		while (collider != null);

		GameObject enemyClone = Instantiate (enemiesPrefabs [Random.Range (0, enemiesPrefabs.Count)], spawns [randomSpawn].transform.position, Quaternion.identity, transform) as GameObject;
		aliveEnemies.Add (enemyClone);
	}

	IEnumerator RandomSpawn ()
	{
		yield return new WaitWhile (() => aliveEnemies.Count == currentMaxEnemies);

		yield return new WaitWhile (()=> player.gameObject.activeSelf == false);

		yield return new WaitForSeconds (Random.Range (randomSpawnDuration.x, randomSpawnDuration.y));

		if(player != null)
		{
			SpawnEnemy ();
			
			StartCoroutine (RandomSpawn ());			
		}
	}

	public void RemoveEnemy (GameObject enemy)
	{
		aliveEnemies.Remove (enemy);
	}

	IEnumerator NextWave ()
	{
		yield return new WaitWhile (()=> player.gameObject.activeSelf == false);

		yield return new WaitForSeconds (waveDuration);

		currentMaxEnemies++;
		Interface.Instance.maxEnemies.value = (float)currentMaxEnemies;

		if(currentMaxEnemies < maxMaxEnemies)
			StartCoroutine (NextWave ());
	}
}
