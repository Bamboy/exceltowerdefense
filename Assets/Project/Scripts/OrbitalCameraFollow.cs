﻿using UnityEngine;
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
    public float terrainHeighest = 0;
    private float betweenTerrainHeight = 0;
    private float lastBetweenTerrainHeight = 0;
    private float terrainCalculationAccuracy = 30;


	void Start () {
	}
	
	void FixedUpdate () {

        //Temp variables for terrain location
        RaycastHit terrainLocation; 
        Vector3 fwd = transform.TransformDirection(new Vector3(0,0.25f,1));
        Vector3 dwn = transform.TransformDirection(new Vector3(0,-1,1));
        terrainHeighest = 0;

        //Calculate the highest terrain in a arch from fwd to down
        for(int i = 0; i < terrainCalculationAccuracy; i++)
        {
            Vector3 dir = Vector3.Lerp(fwd,dwn,Time.deltaTime*i*2);
            if (Physics.Raycast(transform.position, dir, out terrainLocation, Mathf.Infinity))
            {
                terrainHeight = terrainLocation.point.y;
                
                Debug.DrawLine(transform.position, terrainLocation.point, Color.red, 0.1f);
            }

            if (terrainHeight > terrainHeighest)
            {
                terrainHeighest = terrainHeight;
                betweenTerrainHeight = terrainHeighest;
            }
        }

        //Sets the camera rotation point
        if (Physics.Raycast(transform.position, fwd, out terrainLocation, Mathf.Infinity))
        {
            cameraPosition = new Vector3(terrainLocation.point.x, zoomCurrent + betweenTerrainHeight, terrainLocation.point.z);
        }

        //Sets the zoomGoTo
        if (((Input.GetAxisRaw("Mouse ScrollWheel") > 0) || (Input.GetKey("z"))) && (zoomGoTo > zoomMin))
        {
            zoomGoTo -= zoomIncriment;
        }
        else if (((Input.GetAxisRaw("Mouse ScrollWheel") < 0) || (Input.GetKey("x"))) && (zoomGoTo < zoomMax))
        {
            zoomGoTo += zoomIncriment;
        }

        //Smooths the zoomCurrent
        zoomCurrent = Mathf.Lerp(zoomCurrent, zoomGoTo, Time.deltaTime * zoomSpeed);

        //Allows for logical transitioning of the forward
        if (zoomCurrent + betweenTerrainHeight != (transform.position.y))
        {
            while ((zoomCurrent + betweenTerrainHeight) < ((transform.position.y)))
            {
                transform.position += transform.forward;
            }

            while ((zoomCurrent + betweenTerrainHeight) > (transform.position.y))
            {
                transform.position -= transform.forward;
            }
            
        }

        //Allows for camera strave
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
	}
}