/// <summary>
/// Skills.cs
/// Sergey Bedov
/// 03/27/2015
/// ----------------------
/// General purpose: Holds Villager Skills
/// ----------------------
/// </summary>
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Skills : MonoBehaviour, ISkills
{
	public int Breadwinner;// Skill #1 - for gathering food
	public int Forrester;// Skill #2 - for gathering wood
	public int Miner;// Skill #3 - for gathering stone & metal
	public int Warrior;// Skill #4 - for killing enemies by his own
	public int Operator;// Skill #5 - for operating towers

	#region ISkills implementation

	public void SetSkills (int SkillsValue)
	{
		Breadwinner = SkillsValue;
		Forrester = SkillsValue;
		Miner = SkillsValue;
		Warrior = SkillsValue;
		Operator = SkillsValue;
	}

	#endregion
}
