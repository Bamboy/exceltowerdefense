using System.Collections;
using UnityEngine;

// Matt McGrath 4/18/2015

// A Notification is a message we will use to push onto our Notification Log.
// It will have properties such as how long to display, font color, etc, as well 
// as functionality for handling when it is time to "die" off from the Notification Log.
public class Notification		// This is not a MonoBehavior because we will manage these through our Notification Log.
{
	#region Fields
	public string message = string.Empty;		// The message the notification will display to the notification log.
	public Color fontColor = Color.white;		// The color of the font. Probably white by default -- maybe important things will be red or whatever.
	public float displayTime = 5f;				// How long to display in the Notification log.
	public AudioClip notificationSound;			// Sound that plays when the Notification pops.

	// Returns true if the notification is dead (if it's time to remove it from log).
	public bool IsDead { get { return displayTime <= 0f; } }
	#endregion

	#region Intilization
	// Constructor for creating a new Notification with text, color, and display time.
	public Notification(string text, Color color, float time)
	{
		message = text;
		fontColor = color;
		displayTime = time;

		// Load the Audio Clip needed for the notification sound.
		notificationSound = (AudioClip)Resources.Load("Audio/Testing/NotificationPop01");
	}
	#endregion

	#region Update
	// Update is called once per frame.
	public void Update () 
	{
		// Reduce how much display time the notification has left.
		displayTime -= Time.deltaTime;
		if (IsDead)
		{
			// Probably do nothing here: Let the Notification Log manage it? Have an OnDead delegate?
		}
	}
	#endregion
}
