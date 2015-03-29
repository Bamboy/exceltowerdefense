/// <summary>
/// Person.cs
/// Sergey Bedov
/// 03/24/2015
/// ----------------------
/// General purpose: have goal, knows how to get there
/// ----------------------
/// </summary>
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(NavMeshAgent))]
public class Person : MonoBehaviour, IPerson
{
	private NavMeshAgent agent;
	private Transform trans;

	private Vector3 goal;

	void Awake ()
	{
		// ---------- GetComponents ----------
		agent = GetComponent<NavMeshAgent>();
		trans = GetComponent<Transform>();
		// ^^^^^^^^^^ GetComponents  ^^^^^^^^^^
		// ---------- Initiations ----------
		Goal = trans.position;
		// ^^^^^^^^^^ Initiations ^^^^^^^^^^
	}

	void FixedUpdate ()
	{
	
	}

	#region IPerson implementation
	public virtual void MoveTo (Vector3 goal)
	{
		agent.destination = goal;
	}

	public virtual void MoveTo ()
	{
		MoveTo(Goal);
	}

	public virtual Vector3 Goal
	{
		get { return goal; }
		set { goal = value; }
	}
	#endregion
}
