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
using Excelsion.GameManagers;
using Excelsion.UI;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(NavMeshAgent))]
public class VillagerMotor : MonoBehaviour, ISelectable
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
		WorldClock.onPause += PauseMotor;
		WorldClock.onUnpause += UnpauseMotor;
	}

	bool isPause;
	void PauseMotor()
	{
		isPause = true;
		Stop();
		//print("PauseMotor");
	}
	void UnpauseMotor()
	{
		isPause = false;
		MoveTo();
		//print("UnpauseMotor");
	}

	#region Move methods
	public void MoveTo (Vector3 goal)
	{
		Goal = goal;
		if (!isPause)
		{
			agent.SetDestination(Goal);
			agent.Resume();
		}
	}
	public void MoveTo ()
	{
		MoveTo(Goal);
	}
	public void Wait ()
	{

	}
	public void Stop ()
	{
		agent.Stop();
	}
	#endregion

	#region ISelectable
	[SerializeField] protected Transform selectTrans;
	public virtual Transform SelectionTransform
	{
		get
		{
			return selectTrans;
		}
	}
	// This isn't implemented in SelectionController yet :(
	public virtual void OnDrawSelectedUI()
	{
		return;
	}
	#endregion

}
