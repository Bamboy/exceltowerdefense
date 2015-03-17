using UnityEngine;
using System.Collections;

public class ArrowArch : MonoBehaviour {

    public float speed;
    private float turnSpeed;
    public Vector3 targetLocation;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rb.rotation = Quaternion.LookRotation(rb.velocity, new Vector3(0,1,0));
	}

    void OnTriggerEnter (Collider col){
        Debug.Log("test");
			transform.parent = col.transform;
		}
}
