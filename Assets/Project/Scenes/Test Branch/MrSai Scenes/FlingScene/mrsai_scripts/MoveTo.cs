using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {

	// Use this for initialization
	public Transform target;
	public float speed;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target.position, speed);
		}
	}
}
