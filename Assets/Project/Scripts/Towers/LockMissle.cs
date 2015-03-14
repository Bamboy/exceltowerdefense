using UnityEngine;
using System.Collections;

public class LockMissle : MonoBehaviour {

	public Transform target;
	public float missleSpeed;
	public bool isFlying = true;

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
		if (isFlying) {
			GameObject go = GameObject.FindGameObjectWithTag ("Enemy");
			target = go.transform;
			missleSpeed = 5f;
			myTransform.LookAt (target);
			float moveMissle = missleSpeed * Time.deltaTime;
			myTransform.Translate (Vector3.forward * moveMissle);
		}
		stopArrow ();
	}
	void OnTriggerEnter (Collider col){
		if (col.gameObject.tag == "Enemy") {
			isFlying = false;
			transform.parent = col.transform;

		}
	}
	void stopArrow(){
		if (isFlying == false) {
			missleSpeed = 0f;
		}
	}
}
