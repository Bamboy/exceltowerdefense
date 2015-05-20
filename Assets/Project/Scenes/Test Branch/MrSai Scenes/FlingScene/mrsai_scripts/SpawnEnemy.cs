using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {
	public float spawnRate;
	public Transform tower;
	public GameObject prefab;

	float initRate;
	// Use this for initialization
	void Start () {
		initRate = spawnRate;
	}
	
	// Update is called once per frame
	void Update () {
		spawnRate -= Time.deltaTime;
		if (spawnRate <= 0) {
			GameObject newEnemy = (GameObject)Instantiate(prefab, gameObject.transform.position, prefab.transform.rotation);
			newEnemy.GetComponent<MoveTo>().target = tower;
			spawnRate = initRate;
		}
	}
}
