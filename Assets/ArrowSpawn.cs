using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ArrowSpawn : MonoBehaviour
{

    public Transform spawnLocation;
    public Vector3 targetLocation;
    public GameObject projectile;
    public float shootAngle = 15f;

    public Rigidbody rb;

    void Start()
    {

    }

    void Update()
    {

        if (Input.GetMouseButton(0))
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {

                 GameObject newArrow = GameObject.Instantiate(projectile, spawnLocation.position, Quaternion.identity) as GameObject;
                 rb = newArrow.GetComponent<Rigidbody>();
                 rb.velocity = BallisticVel(targetLocation, shootAngle);
                 //Destroy(ball, 10);
                /*
                //float attackVelocity = 30f;
                
                //float angle = Mathf.Atan((Mathf.Pow(attackVelocity, 2) + Mathf.Sqrt(Mathf.Pow(attackVelocity, 4) - (gravity * ((gravity * Mathf.Pow(distance, 2)) + (2 * y * Mathf.Pow(attackVelocity, 2)))))) / (gravity * distance)) * Mathf.Rad2Deg;

                targetLocation = hit.point;
                //Vector3 direction = VectorExtras.Direction(transfor,targetLocation);

                //Create projectile
                GameObject newArrow = GameObject.Instantiate(projectile,spawnLocation.position,Quaternion.identity) as GameObject;
                newArrow.GetComponent<ArrowArch>().targetLocation = targetLocation;
                newArrow.GetComponent<ArrowArch>().speed = 30f;

                //foreach (ProjectileBase proj_init in projectiles)
                //{
                //    proj_init.target =  //targetLocation;
                //}

                //val[0] = projScript;
                //return val;
                 * */
            }
        }
            
    }
    public Vector3 BallisticVel(Vector3 targetLocation, float angle)
    {
        Vector3 dir = targetLocation - transform.position;  // get target direction
        float h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        float dist = dir.magnitude;  // get horizontal distance
        float a = angle * Mathf.Deg2Rad;  // convert angle to radians
        dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
        dist += h / Mathf.Tan(a);  // correct for small height differences
        // calculate the velocity magnitude
        float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return vel * dir.normalized;
    }
}
