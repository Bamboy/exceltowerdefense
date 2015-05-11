using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Excelsion.Enemies;

public class SpawnPoint : MonoBehaviour 
{
    public GameObject enemyObjective; 
    public List<Enemy> enemies;
    public GameObject enemyPrefab;
    public float spawnTime;

	void Start () 
	{
        enemies = new List<Enemy>();
        enemyObjective = GameObject.FindGameObjectWithTag("Player") as GameObject;
        if (enemyObjective == null)
        {
            Debug.LogError("You need to tag an object as Player so the enemies have something to attack!");
        }
        StartCoroutine("TimedSpawner");
	}

	void Update () 
	{
	}

    IEnumerator TimedSpawner()
    {
        yield return new WaitForSeconds(spawnTime);
        while (enemies.Count >= 20)
            yield return null;

        GameObject obj = GameObject.Instantiate(enemyPrefab, transform.position, Quaternion.identity) as GameObject;
        Enemy newEnemy = obj.GetComponent<Enemy>();
        enemies.Add(newEnemy);

        StartCoroutine("TimedSpawner"); //Repeat forever...
    }
}
