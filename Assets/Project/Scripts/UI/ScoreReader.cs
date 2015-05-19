using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Matt McGrath - 5/18/2015

// Displays the score for the new Prototype. Score for now is simply number of kills.
public class ScoreReader : MonoBehaviour 
{
	public Text scoreText;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateScore();
	}

	// Updates the UI Text elements all Structures [may] share.
	void UpdateScore()
	{
		scoreText.text = "Kills: " + EnemyKillCounter.NumberOfKills;

		// this will enable the script on the enemy  
		GameObject.Find("Projectile").GetComponent("EnemyKillCounter");
	}
}
