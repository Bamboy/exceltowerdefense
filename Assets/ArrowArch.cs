using UnityEngine;
using System.Collections;

public class ArrowArch : MonoBehaviour {

    public float speed;
    private float turnSpeed;
    public Vector3 targetLocation;

    //public Rigidbody rb;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        /*
        // update direction each frame:
       Vector3 dir = targetLocation - transform.position;
       // calculate desired rotation:
       Quaternion rot = Quaternion.LookRotation(dir);
       // Slerp to it over time:
       transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed * Time.deltaTime);
       transform.LookAt(Vector3.Slerp(transform.rotation,rot,turnSpeed * Time.deltaTime));
       // move in the current forward direction at specified speed:
       transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));

        //rb.rotation = Quaternion.LookRotation(rb.velocity);*/
	}
}
