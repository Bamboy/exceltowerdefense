/// <summary>
/// IPerson.cs
/// Sergey Bedov
/// 03/24/2015
/// ----------------------
/// General purpose: Declare all the public methods of Person.cs
/// ----------------------
/// </summary>
using UnityEngine;

public interface IPerson
{
	// in case we want to send Villaget somewhere
	void MoveTo(Vector3 goal);
	// in case we want to setup Goal first
	Vector3 Goal{get; set;}
	// and send Villager to the Goal after some time
	void MoveTo();
}
