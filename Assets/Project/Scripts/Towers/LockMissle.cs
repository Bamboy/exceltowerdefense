using UnityEngine;
using System.Collections;

public class LockMissle : MonoBehaviour {

	public Transform target;
	public float missleSpeed = 5f;

	private Transform myTransform;

	void Awake(){
		myTransform = transform;
	}
	// Use this for initialization
	void Start () 
	{


	}
	
	// Update is called once per frame
	void Update () 
	{
		GameObject go = GameObject.FindGameObjectWithTag("Enemy");
		target = go.transform;
		// rotate the projectile to aim the target:
		myTransform.LookAt(target);
		float moveMissle = missleSpeed * Time.deltaTime;
		myTransform.Translate(Vector3.forward * moveMissle);
	}

	void OnTriggerEnter (Collider col){
		if (col.gameObject.tag == "Enemy") {
			Destroy(col.gameObject);
			Destroy(this.gameObject);
		}
	}
}
