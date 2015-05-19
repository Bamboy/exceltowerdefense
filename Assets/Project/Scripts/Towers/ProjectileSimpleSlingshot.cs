using System.Collections;
using UnityEngine;

// Matt McGrath - 5/17/2015

// This is our Angry Bird's style projectile mechanism. Technically the projectile isn't doing this work: Something like a catapult or a slingshot would, but
// we'll put it in this class for now. This is similar to ProjectileSlingshot but will be simpler: It will not use a SpringJoint. Rather, use a grab and release method
// to send the projectile in the opposite direction of the drag.
public class ProjectileSimpleSlingshot : MonoBehaviour 
{
	#region Fields
	public float maxStretch = 50.0f;				// Maximum distance we can stretch the Player Tower "slingshot" back.
	private float maxStretchSquared;				// The square of this stretch, for efficiency purposes.
	public LineRenderer catapultLeftLine;			// A line renderer so we can see our projectile attached to the Player Tower.
	//public LineRenderer catapultRightLine;
	
	private Transform playerTower;					// A reference to the position of the Player Tower. We want the projectile connected to it via the spring.
	
	private Rigidbody rigidBody;				
	private SpringJoint spring;						// The SpringJoint we will  use to allow for an Angry Bird's type firing system.
	private SpringJoint defaultSpring;
	
	private bool clickedOn;							// Boolean flag to indicate if the projectile is currently being clicked on.
	
	private Ray rayToMouse;							// A ray leading to our mouse.
	private Ray leftCatapultToProjectile;			// A ray from the left side of the "slingshot" band to the projectile.
	
	private Vector3 previousVelocity;				// Our previous velocity. When our new velocity is slower we know the spring must have been launched.
	private float sphereRadius;						// We need to know the radius of our Player Tower.
	
	private float springReattachmentTime = 5.0f;
	private float timeUntilSpringReattached = 5.0f;
	private bool springActive;
	#endregion
	
	#region Initialization
	// On awake let's grab some of our components we'll be referencing a bit.
	void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
		spring = GetComponent<SpringJoint>();
		playerTower = spring.connectedBody.transform;
	}
	
	// Initializes our Line Renderers, Rays, collider, sphere radius, and calculates the square of our max stretch distance.
	void Start () 
	{
		LineRendererSetup();
		rayToMouse = new Ray(playerTower.transform.position, Vector3.zero);
		leftCatapultToProjectile = new Ray(catapultLeftLine.transform.position, Vector3.zero);
		maxStretchSquared = maxStretch * maxStretch;
		SphereCollider sphere = GetComponent<SphereCollider>();
		sphereRadius = sphere.radius;
		springActive = true;
	}
	#endregion
	
	#region Update
	// Updates the projectile with logic for dragging it and launching it through mouse click flags.
	void Update () 
	{
		CastRayToWorld();				// Debugging: Seeing if Mouse-To-World Position was working.
		
		// If our "slingshot" projectile is being clicked on, we want to handle the dragging logic.
		if (clickedOn)
			Dragging();
		
		if (spring != null)
		{
			if (!springActive)
			{
				timeUntilSpringReattached -= Time.deltaTime;
				if (timeUntilSpringReattached <= 0)
				{
					// Re-attach the spring.
				}
			}
			
			// This check sees if it is time to release the projectile (and, for testing, destroy the spring).
			if (!rigidBody.isKinematic && previousVelocity.sqrMagnitude > rigidBody.velocity.sqrMagnitude)
			{
				Destroy(this.gameObject, 10f);
				Destroy(spring);							// Need to destroy spring for projectile to launch.
				springActive = false;
				rigidBody.velocity = previousVelocity;
			}
			
			if (!clickedOn)
			{
				previousVelocity = rigidBody.velocity;
			}
			
			// Update the line renderers if our spring is still attached.
			LineRendererUpdate();
		}
		else
		{
			catapultLeftLine.enabled = false;
			//catapultRightLine.enabled = false;
		}
		
		Debug.Log("RigidBody Velocity " + rigidBody.velocity.ToString());
	}
	#endregion
	
	#region Mouse Down and Up MonoBehavior Events
	// MonoBehavior for when we click our mouse button. We want to set our clickedOn flag here.
	void OnMouseDown()
	{
		if (spring != null)
			spring.enableCollision = false;
		
		// Flag that the projectile is being clicked on.
		clickedOn = true;
	}
	
	// MonoBehavior for when we release our mouse button. We want release the projectile from the "slingshot".
	void OnMouseUp()
	{
		if (spring != null)
			spring.enableCollision = false;
		if (rigidBody != null)
			rigidBody.isKinematic = false;			// We now want to use the Physics system, since the projectile has been launched.
		
		// Flag that the projectile is not being clicked on.
		clickedOn = false;
	}
	#endregion
	
	#region Dragging (pulling back) the "slingshot"
	// Handles dragging the "slingshot" around.
	void Dragging()
	{
		//Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		distance = Vector3.Distance (Camera.main.transform.position, playerTower.position);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
		Vector3 mouseWorldPoint = ray.origin + (ray.direction * distance);   
		Vector3 towerToMouse = mouseWorldPoint - playerTower.transform.position;
		
		// If the distance from the Player Tower to our mouse is greater than our maximum stretch limit...
		if (towerToMouse.sqrMagnitude > maxStretchSquared)
		{
			// Keep the direction of our drag...
			rayToMouse.direction = towerToMouse;
			
			// ...But clamp the projectile to the maximum stretching distance (even if our mouse is further back on the screen).
			mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
		}
		
		// Set the position of our projectile to the mouse world point.
		transform.position = mouseWorldPoint;
	}
	#endregion
	
	#region Line Rendering (To see spring / "Slingshot" visually) -- NOT WORKING?!
	// Handles updating the line renderers.
	void LineRendererUpdate()
	{
		Vector3 towerToProjectile = transform.position - catapultLeftLine.transform.position;
		leftCatapultToProjectile.direction = towerToProjectile;
		Vector3 holdPoint = leftCatapultToProjectile.GetPoint(towerToProjectile.magnitude + sphereRadius);
		catapultLeftLine.SetPosition(0, holdPoint);
		//catapultRightLine.SetPosition(1, holdPoint);
		
		Debug.DrawRay(catapultLeftLine.transform.position, leftCatapultToProjectile.direction);
	}
	
	// Sets up our line renderers, which we will use to represent the "slingshot" strings of our Player Tower.
	void LineRendererSetup()
	{
		// Starting position for the "slingshot" of the Player Tower.
		catapultLeftLine.SetPosition(0, catapultLeftLine.transform.position);
	}
	#endregion
	
	public float distance;
	void CastRayToWorld() 
	{
		distance = Vector3.Distance (Camera.main.transform.position, playerTower.position);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
		Vector3 point = ray.origin + (ray.direction * distance);    
		//Debug.Log( "World point " + point );
	}
}