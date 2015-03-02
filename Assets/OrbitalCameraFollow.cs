using UnityEngine;
using System.Collections;

public class OrbitalCameraFollow : MonoBehaviour {

    public Vector3 cameraPosition = new Vector3(1000,100,1000);

    public float rotationSpeed = 100;

    public float zoom = 60;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
        if (Input.GetKey("q"))
        {
            transform.RotateAround(cameraPosition, Vector3.up, rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey("e"))
        {
            transform.RotateAround(cameraPosition, Vector3.up, -rotationSpeed * Time.deltaTime);
        }

        zoom -= 10 * (Input.GetAxis("Mouse ScrollWheel"));

        Camera.main.fieldOfView = zoom;
        /*
        if (Input.GetKey("w"))
        {
            Vector3 folPos = followObject.transform.position;
            followObject.transform.position = new Vector3(folPos.x,folPos.y,folPos.z);
        }*/


	}
}
