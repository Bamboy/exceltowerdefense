using UnityEngine;
using System.Collections;

public class OrbitalCameraFollow : MonoBehaviour {

    //Allows for rotation of camera
    private Vector3 cameraPosition = new Vector3(1000,100,900);

    //How fast can it rotate
    public float rotationSpeed = 100;

    //How fast can it move around
    public float movementSpeed = 10;

    //Used for calculating smoothing in zoom
    public float zoomCurrent = 100;
    public float zoomGoTo = 100;
    public float zoomSpeed = 10;

    //Sets the min and maximum zoom along with the incriment
    public float zoomMin = 5;
    public float zoomMax = 100;
    public float zoomIncriment = 5;

    //Used for calculating camera displacment
    public float terrainHeight = 0;
    private float predictUpperTerrainHeight = 0;
    private float predictLowerTerrainHeight = 0;
    private float betweenTerrainHeight = 0;

    

	void Start () {
	}
	
	void FixedUpdate () {

        //Temp variables for terrain location
        RaycastHit terrainLocation; 
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        //Used for calculating the height of the terrain
        if (Physics.Raycast(transform.position,fwd,out terrainLocation, Mathf.Infinity))
        {
            //Provides the height of the terrain
            terrainHeight = terrainLocation.point.y;

            //Predicts upper terrain height
            RaycastHit predictUpperTerrainLocation;
            if (Physics.Raycast(new Vector3(transform.position.x,transform.position.y+terrainHeight,transform.position.z),fwd,out predictUpperTerrainLocation,Mathf.Infinity))
            {
                predictUpperTerrainHeight = predictUpperTerrainLocation.point.y;
                Debug.DrawLine(transform.position, predictUpperTerrainLocation.point, Color.red, 0.1f);
            }

            //Predicts lower terrain height
            RaycastHit predictLowerTerrainLocation;
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - terrainHeight, transform.position.z), fwd, out predictLowerTerrainLocation, Mathf.Infinity))
            {
                predictLowerTerrainHeight = predictLowerTerrainLocation.point.y;
                Debug.DrawLine(transform.position, predictLowerTerrainLocation.point, Color.red, 0.1f);
            }

            //Checks if it needs to change the between terrain height
            if (terrainHeight != predictUpperTerrainHeight || terrainHeight != predictLowerTerrainHeight )
            {
                betweenTerrainHeight = (terrainHeight + predictUpperTerrainHeight + predictLowerTerrainHeight) / 3f;
            }
            else
            {
                betweenTerrainHeight = terrainHeight;
            }

            //Sets camera position based of current zoom
            cameraPosition = new Vector3(terrainLocation.point.x, zoomCurrent, terrainLocation.point.z);

            //transform.position = new Vector3(transform.position.x, zoomCurrent + betweenTerrainHeight, transform.position.z);
            //transform.position += transform.forward * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            

            Debug.DrawLine(transform.position, terrainLocation.point, Color.red,0.1f);
            //Debug.Log(cameraPosition);
        }

        

        if (((Input.GetAxisRaw("Mouse ScrollWheel") > 0) || (Input.GetKey("z"))) && (zoomGoTo > zoomMin))
        {
            zoomGoTo -= zoomIncriment;
        }
        else if (((Input.GetAxisRaw("Mouse ScrollWheel") < 0) || (Input.GetKey("x"))) && (zoomGoTo < zoomMax))
        {
            zoomGoTo += zoomIncriment;
        }

        //zoomCurrent = Mathf.Lerp(zoomCurrent, zoomGoTo, Time.deltaTime * zoomSpeed);

        zoomCurrent = transform.position.y;

        if ((zoomCurrent + betweenTerrainHeight) > zoomGoTo)
        {
            transform.position += transform.forward * Time.deltaTime * zoomSpeed;
        }
        else if ((zoomCurrent + betweenTerrainHeight) < zoomGoTo)
        {
            transform.position -= transform.forward * Time.deltaTime * zoomSpeed;
        }

        transform.position += transform.forward * Time.deltaTime;

        //transform.position = new Vector3(transform.position.x, zoomCurrent + betweenTerrainHeight, transform.position.z);
        //transform.position += transform.forward * Input.GetAxis("Mouse ScrollWheel") * zoomIncriment * zoomSpeed;

        //zoom = goTo;


        if (Input.GetKey("q"))
        {
            transform.RotateAround(cameraPosition, Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey("e"))
        {
            transform.RotateAround(cameraPosition, Vector3.up, rotationSpeed * Time.deltaTime);
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

        //zoom -= 10 * (Input.GetAxis("Mouse ScrollWheel"));
        //zoom -= Input.GetAxis("Mouse ScrollWheel");
         //transform.position.y-terrainHeight;

        
        //Camera.main.fieldOfView = zoom;
        /*
        if (Input.GetKey("w"))
        {
            Vector3 folPos = followObject.transform.position;
            followObject.transform.position = new Vector3(folPos.x,folPos.y,folPos.z);
        }*/


	}
}
