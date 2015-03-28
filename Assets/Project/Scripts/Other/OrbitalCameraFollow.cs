using UnityEngine;
using System.Collections;

public class OrbitalCameraFollow : MonoBehaviour {

    //Allows for rotation of camera
    private Vector3 cameraPosition = new Vector3(1000,100,900);

    //How fast can it rotate
    public float rotationSpeed = 100;

    //How fast can it move around
    public float movementSpeed = 1;

    //Used for calculating smoothing in zoom
    public float zoomCurrent;
    public float zoomGoTo;
    public float zoomSpeed = 10;

    //Sets the min and maximum zoom along with the incriment
    public float zoomMin = 10;
    public float zoomMax = 120;
    private float zoomPercentage;
    public float zoomIncriment = 10;

    //Used for calculating camera displacment
    public float terrainHeight = 0;
    public float terrainHeighest = 0;
    private float midTerrainHeight = 0;
    private float terrainCalculationAccuracy = 30;

    public LayerMask cameraOrbitBasedOff;

	void Start () {
        zoomCurrent = zoomMax;
        zoomGoTo = zoomMax;
        zoomPercentage = zoomMax / 100; //(zoomMin / zoomMax) * 100;
        zoomMin = zoomMin * zoomPercentage;
	}
	
	void Update () {

        
        //Sets the zoomGoTo
        if (((Input.GetAxis("Mouse ScrollWheel") > 0) || (Input.GetKey("z"))) && (zoomGoTo > zoomMin))
        {
            //zoomGoTo -= zoomIncriment;
            zoomGoTo -= (zoomPercentage * zoomIncriment);
        }
        else if (((Input.GetAxis("Mouse ScrollWheel") < 0) || (Input.GetKey("x"))) && (zoomGoTo < zoomMax))
        {
            //zoomGoTo += zoomIncriment;
            zoomGoTo += (zoomPercentage * zoomIncriment);
        }

        //Allows for camera strave
        if (Input.GetKey("q"))
        {
			RaycastHit data;
			if( Physics.Raycast( Camera.main.transform.position, Camera.main.transform.forward, out data ) )
			{
				transform.RotateAround( data.point, Vector3.up, -rotationSpeed * Time.deltaTime);
			}


        }
        else if (Input.GetKey("e"))
        {
			RaycastHit data;
			if( Physics.Raycast( Camera.main.transform.position, Camera.main.transform.forward, out data ) )
			{
				transform.RotateAround( data.point, Vector3.up, rotationSpeed * Time.deltaTime);
			}

        }

        if (Input.GetKey("w"))
        {
            transform.Translate(new Vector3(0,movementSpeed,movementSpeed));
        }
        else if (Input.GetKey("s"))
        {
            transform.Translate(new Vector3(0, -movementSpeed, -movementSpeed));
        }

        if (Input.GetKey("a"))
        {
            transform.Translate(new Vector3(-movementSpeed, 0, 0));
        }
        else if (Input.GetKey("d"))
        {
            transform.Translate(new Vector3(movementSpeed, 0, 0));
        }
	}

    void FixedUpdate()
    {

        //Temp variables for terrain location
        RaycastHit terrainLocation;
        Vector3 fwd = transform.TransformDirection(new Vector3(0, 0.1f, 1));
        Vector3 dwn = transform.TransformDirection(new Vector3(0, -1, 1));
        terrainHeight = 0;
        terrainHeighest = 0;

        //Calculate the highest terrain in a arch from fwd to down
        for (int i = 0; i < terrainCalculationAccuracy; i++)
        {
            Vector3 dir = Vector3.Lerp(fwd, dwn, Time.deltaTime * i * 2);
            if (Physics.Raycast(transform.position, dir, out terrainLocation, Mathf.Infinity, cameraOrbitBasedOff))
            {
                if (terrainLocation.collider.tag == "Terrain")
                {
                    terrainHeight = terrainLocation.point.y;
                    Debug.DrawLine(transform.position, terrainLocation.point, Color.red, 0.1f);
                }

            }

            if (terrainHeight > terrainHeighest)
            {
                terrainHeighest = terrainHeight;
            }
        }

        //Sets the camera rotation point
        Vector3 dirMid = transform.TransformDirection(new Vector3(0, 0, 1));
        if (Physics.Raycast(transform.position, dirMid, out terrainLocation, Mathf.Infinity,cameraOrbitBasedOff))
        {
            midTerrainHeight = terrainLocation.point.y;
            cameraPosition = new Vector3(terrainLocation.point.x, zoomCurrent + midTerrainHeight, terrainLocation.point.z);
            Debug.DrawLine(transform.position, terrainLocation.point, Color.green, 0.1f);
        }

        //Smooths the zoomCurrent
        zoomCurrent = Mathf.Lerp(zoomCurrent, zoomGoTo, Time.deltaTime * zoomSpeed);

        //Allows for logical transitioning of the forward
        if (zoomCurrent + terrainHeighest != (transform.position.y))
        {
            while ((zoomCurrent + terrainHeighest) < ((transform.position.y)))
            {
                transform.position += transform.forward;
            }

            while ((zoomCurrent + terrainHeighest) > (transform.position.y))
            {
                transform.position -= transform.forward;
            }

        }
    }
}
