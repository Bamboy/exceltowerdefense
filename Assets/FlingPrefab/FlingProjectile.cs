using System.Collections;
using UnityEngine;

// Coding by Esai - 5/18/2015. Commenting & very minor tweaks by Matt.
public class FlingProjectile : MonoBehaviour 
{
	public GameObject projectilePrefab;			// Projectile prefab we wish to "fling."
	public float upwardForce;					// The upward force our flings will have.
	public float sideForce;						// The forward force our flings will have.
	public Vector2 lastMousePosition;			// Store the mouse position when we click the mouse down. This will help us with direction calculation.
	public Vector2 direction;					// TODO: Private eventually. This is calculated and stores the direction the projectile will fling in.
	public float maxForce;						// The maximum force we can exert onto the projectile to launch it. In Angry Bird's, this would be how far back the slingshot can be pulled back.
	GameObject currentLoadedProjectile;			// Current Instantiated projectile to fling
	public float dist;
	public int pullBackRadius;
	public float speed;
	
	// Use this for initialization
	void Start () 
	{

	}
	
	// During update, we check for mouse down and up event. Using those, we determine when and in what direction / force to fling the projectile.
	void Update () 
	{
		// When we press the left mouse button down, we set the last mouse position to the current position of the mouse.
		if (Input.GetKey(KeyCode.Mouse0)) 
		{
			if(currentLoadedProjectile == null)
			currentLoadedProjectile = (GameObject)Instantiate(projectilePrefab, gameObject.transform.position, projectilePrefab.transform.rotation);

			if(Input.GetKeyDown(KeyCode.Mouse0)) lastMousePosition = Input.mousePosition;
			direction = new Vector2 (Mathf.Clamp(Input.mousePosition.x - lastMousePosition.x, -maxForce, maxForce)/maxForce,
			                         Mathf.Clamp(Input.mousePosition.y - lastMousePosition.y, -maxForce, maxForce)/maxForce);


			dist = Vector3.Distance(currentLoadedProjectile.transform.position, gameObject.transform.position);


			//if(dist <= maxDistance)
			currentLoadedProjectile.transform.position += new Vector3(direction.x * speed, 0 , direction.y * speed);
			Vector3 clampedPosition = currentLoadedProjectile.transform.position;
			clampedPosition.x = Mathf.Clamp(clampedPosition.x, gameObject.transform.position.x - pullBackRadius, gameObject.transform.position.x + pullBackRadius);
			clampedPosition.y = Mathf.Clamp(clampedPosition.y, clampedPosition.y - pullBackRadius, clampedPosition.y + pullBackRadius);
			clampedPosition.z = Mathf.Clamp(clampedPosition.z, gameObject.transform.position.z - pullBackRadius, gameObject.transform.position.z + pullBackRadius);
			currentLoadedProjectile.transform.position = clampedPosition;
				//dist = Mathf.Clamp(dist, -maxDistance, maxDistance);
			//if(dist <= maxDistance)
//			currentLoadedProjectile.transform.RotateAround(gameObject.transform.position, Vector3.left, currentLoadedProjectile.transform.rotation);
			//if(currentLoadedProjectile != null) currentLoadedProjectile.transform.position += direction * dist;
		}

		// When we left the left mouse button up, we have completed the "grab and release" type firing found in Angry Birds.
		if (Input.GetKeyUp(KeyCode.Mouse0)) 
		{
			// Calculate direction based on released mouse position and the position the mouse was "grabbed" from.

			direction = new Vector2 (Mathf.Clamp(currentLoadedProjectile.transform.position.x - gameObject.transform.position.x, -maxForce, maxForce)/maxForce,
			                         Mathf.Clamp(currentLoadedProjectile.transform.position.y - gameObject.transform.position.y, -maxForce, maxForce)/maxForce);
			// Create the projectile using the specified projectile prefab, setting its position to the object that holds this script (probably a Transform above our Player Tower).

			// Cache the RigidBody component of the projectile, as GetComponent calls are quite expensive, and we'll be needing it twice.
			Rigidbody projectileRigidBody = currentLoadedProjectile.GetComponent<Rigidbody>();
			projectileRigidBody.useGravity = true;
			// Now add our upward and directional force to the projectile's RigidBody. Projectile is set in motion with Physics now!
			projectileRigidBody.AddForce(Vector3.up * upwardForce);

			sideForce = Vector3.Distance(currentLoadedProjectile.transform.position, gameObject.transform.position) * 10000;
			projectileRigidBody.AddForce(new Vector3(-direction.x, 0, -direction.y) * sideForce);

			currentLoadedProjectile = null;
		}
	}
}
