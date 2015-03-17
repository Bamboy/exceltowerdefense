using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ArrowSpawn : MonoBehaviour
{

    public Transform spawnLocation;
    public Vector3 targetLocation;
    public GameObject projectile;
    public GameObject testingObject;
    public float shootAngle = 45f;

    private Rigidbody rb;

    void Start()
    {

    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (projectile != null)
                {
                    GameObject newArrow = GameObject.Instantiate(projectile, spawnLocation.position, Quaternion.identity) as GameObject;
                    GameObject.Instantiate(testingObject, hit.point, Quaternion.identity);
                    rb = newArrow.GetComponent<Rigidbody>();
                    targetLocation = hit.point;
                    rb.velocity = BallisticVel(targetLocation, shootAngle);
                }
                else
                {
                    Debug.Log("Please assign a projectile prefab");
                }
                 
            }
        }
            
    }
    public Vector3 BallisticVel(Vector3 targetLocation, float angle)
    {
        Vector3 dir = new Vector3(targetLocation.x,targetLocation.y+2f,targetLocation.z) - spawnLocation.position;  // get target direction

        float h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        float dist = dir.magnitude;  // get horizontal distance
        float a = angle * Mathf.Deg2Rad;  // convert angle to radians
        dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
        dist += h / Mathf.Tan(a);  // correct for small height differences
        // calculate the velocity magnitude
        float vel = Mathf.Sqrt((dist) * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        Vector3 calculatedVal = (vel * dir.normalized);

        if (calculatedVal.y > 0)
        {
            return calculatedVal;
        }
        else
        {
            calculatedVal = new Vector3(0, 1, 0);
            return calculatedVal;
        }
        
        //return new Vector3(vel * dir.normalized.x, vel * dir.normalized.y, vel * dir.normalized.z);
        
    }
}
