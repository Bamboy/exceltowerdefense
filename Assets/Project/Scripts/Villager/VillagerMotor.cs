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
public class VillagerMotor : MonoBehaviour
{
	private NavMeshAgent agent;
	private Transform trans;

	public float MinDistanceToGoal = 1.5F;

	public Vector3 Goal;

	void Awake ()
	{
		agent = GetComponent<NavMeshAgent>();
		trans = GetComponent<Transform>();
		Goal = trans.position; //initiate Goal in Person's position (to make him stay)
	}

	#region Move methods
	public void MoveTo (Vector3 goal)
	{
		Goal = goal;
		agent.SetDestination(Goal);
	}

	public void MoveTo ()
	{
		MoveTo(Goal);
	}
	#endregion

}
