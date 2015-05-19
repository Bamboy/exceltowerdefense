using System.Collections;
using UnityEngine;

// Coding by Asai - 5/18/2015. Commenting & very minor tweaks by Matt.
public class FlingProjectile : MonoBehaviour 
{
	public GameObject projectilePrefab;			// Projectile we wish to "fling."
	public float upwardForce;					// The upward force our flings will have.
	public float sideForce;						// The forward force our flings will have.
	public Vector2 lastMousePosition;			// Store the mouse position when we click the mouse down. This will help us with direction calculation.
	public Vector2 direction;					// TODO: Private eventually. This is calculated and stores the direction the projectile will fling in.
	public float maxForce;						// The maximum force we can exert onto the projectile to launch it. In Angry Bird's, this would be how far back the slingshot can be pulled back.

	
	// Use this for initialization
	void Start () 
	{

	}
	
	// During update, we check for mouse down and up event. Using those, we determine when and in what direction / force to fling the projectile.
	void Update () 
	{
		// When we press the left mouse button down, we set the last mouse position to the current position of the mouse.
		if (Input.GetKeyDown (KeyCode.Mouse0)) 
		{
			lastMousePosition = Input.mousePosition;
		}

		// When we left the left mouse button up, we have completed the "grab and release" type firing found in Angry Birds.
		if (Input.GetKeyUp(KeyCode.Mouse0)) 
		{
			// Calculate direction based on released mouse position and the position the mouse was "grabbed" from.
			direction = new Vector2 (Mathf.Clamp(Input.mousePosition.x - lastMousePosition.x, -maxForce, maxForce)/maxForce,
			                         Mathf.Clamp(Input.mousePosition.y - lastMousePosition.y, -maxForce, maxForce)/maxForce);

			// Create the projectile using the specified projectile prefab, setting its position to the object that holds this script (probably a Transform above our Player Tower).
			GameObject newProjectile = (GameObject)Instantiate(projectilePrefab, gameObject.transform.position, projectilePrefab.transform.rotation);

			// Cache the RigidBody component of the projectile, as GetComponent calls are quite expensive, and we'll be needing it twice.
			Rigidbody projectileRigidBody = newProjectile.GetComponent<Rigidbody>();

			// Now add our upward and directional force to the projectile's RigidBody. Projectile is set in motion with Physics now!
			projectileRigidBody.AddForce(Vector3.up * upwardForce);
			projectileRigidBody.AddForce(new Vector3(-direction.x, 0, -direction.y) * sideForce);
		}
	}
}
