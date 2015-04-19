using UnityEngine;
using System.Collections;

// Matt McGrath 4/18/2015

// A Notification is a message we will use to push onto our Notification Log.
// It will have properties such as how long to display, font color, etc, as well 
// as functionality for handling when it is time to "die" off from the Notification Log.
using UnityEngine.UI;

public class Notification
{
	public string message = string.Empty;
	public Color fontColor = Color.white;		// The color of the font. Probably white by default -- maybe important things will be red or whatever.
	public float displayTime = 5f;				// How long to display in the Notification log.

	public bool IsDead { get { return displayTime <= 0f; } }
	
	public Notification(string text, Color color, float time)
	{
		message = text;
		fontColor = color;
		displayTime = time;
	}

	// Update is called once per frame
	public void Update () 
	{
		displayTime -= Time.deltaTime;
		if (IsDead)
		{
			// Probably do nothing here: Let the Notification Log manage it? Have an OnDead delegate?
		}
	}
}
