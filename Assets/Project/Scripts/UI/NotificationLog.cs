using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

// Matt McGrath - 4/18/2015

// Notification Log will display "Important Events" such as villagers dying.
// It will do so in a typical MMO chat-log type way, where messages slowly 
// disappear after a given amount of time.
// TODO bug where the last full log of messages all show up as the last message sent rather than appropriately deleting and shifting downward.
// TODO make singleton
// TODO error checking and prevention
public class NotificationLog : MonoBehaviour 
{
	#region Access Instance Anywhere
	private static NotificationLog notControl;

	public static NotificationLog Get()
	{
		if( notControl != null )
			return notControl;
		else
		{
			GameObject go = Instantiate(Resources.Load("Notification Log")) as GameObject; 
			go.tag = "NotificationLog";
			return notControl;
		}
	}

	void Awake() 
	{
		if( notControl == null )
			notControl = this;
		else
			GameObject.Destroy( this.gameObject );
	}
	#endregion

	// A List (Array or other data structure later if you want) of logged notifications.
	public List<Notification> loggedNotifications = new List<Notification>();	
	public int displayLimit = 5;					// How many notifications to display at once?

	// How long to display notification? (Or should we allow Notification to customize their own timings, so very important messages stay longer? 
	// Would probably need a different data structure and approach since we no longer assume we're removing the first index from the List.
	public float displayTime = 5f;					
	public bool playNotificationSounds = true;		// Should we play the Notification sounds?

	// Variables for testing.
	//private float timeSinceLastNotification = 0.5f;
	//private float autoNotificationTestTimer = 0.5f;
	// int maxAutoMessages = 10;
	private int notificationNumber = 0;
	
	public List<Text> textReferences = new List<Text>();

	Color defaultImageColor = new Color(0f, 0f, 0f, 0.25f);

	public AudioSource audioSource;

	// Use this for initialization
	void Start () 
	{
		loggedNotifications = new List<Notification>();
		
		foreach (Text text in textReferences)
		{
			text.text = string.Empty;
		}

		audioSource = this.gameObject.AddComponent<AudioSource>();
		audioSource.volume = 0.25f;
	}
	
	// Update is called once per frame. Here we will manage the Notifications on the Log.
	void Update () 
	{
		// TEMP TESTING
		if (Input.GetKeyDown (KeyCode.L))
		{
			notificationNumber++;
			float randomDisplayTime = UnityEngine.Random.Range (2f, 10f);
			Notification notification = new Notification("Testing Notifications! Message #" + notificationNumber.ToString (), Color.white, 4.0f);
			PushNotification(notification);
		}

		if (loggedNotifications.Count > 0)
		{
			// Update  each Notification in the Notification Log.
			foreach (Notification notification in loggedNotifications)
			{
				notification.Update();

				// If any have now expired, remove them.
				if (notification.IsDead)
				{
					// Remove then break (I heard issues arise otherwise).
					RemoveNotification(notification);
					break;
				}
			}
		}

		// Check if there's no Notifications on the Notification Log.
		if  (loggedNotifications.Count == 0)
		{
			// If so, probably remove the UI backdrop. Eventually fade it out or something fancy.
			// TODO grab reference to the background image and disable its rendering.
			this.gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
		}
		else
		{
			// Keep it enabled. Would be better served through an OnFirstNotificationAdded event but I'll keep it simple for now.
			// TODO grab reference to the background image and keep it enabled or re-enable it.
			this.gameObject.GetComponent<Image>().color = defaultImageColor;
		}
	}

	void LateUpdate()
	{
		UpdateText();
	}


	int pushIndex = -1;

	// Adds a notification to the Notification Log.
	public void PushNotification(Notification notification)
	{
		// Do we prematurely remove the oldest message if we need room for this new one? What if we get 10 notifications all at once? Then we'll never have time to read the first 5.
		// For now let's pretend we remove the oldest message anyways, even if it's not ready to "die" off.
		if (loggedNotifications.Count >= displayLimit)
		{
			//Debug.Log ("We're at max capacity! We'll push new message anyways?");

			// Remove  the oldest.
			//if (loggedNotifications[loggedNotifications.Count - 1] != null)
			{
				Notification oldestNotification = loggedNotifications[0];
				for (int i = 1; i < loggedNotifications.Count - 1; i++)
				{
					if (loggedNotifications[i].displayTime < oldestNotification.displayTime)
						oldestNotification = loggedNotifications[i];
				}

				RemoveNotification(oldestNotification);

				// Now push the new notification regularly.
				// DONT SEEM TO NEED
				pushIndex = loggedNotifications.Count;
				pushIndex = Mathf.Clamp (pushIndex, 0, displayLimit - 1);
				textReferences[pushIndex].text = notification.message;
				// END DONT SEEM TO NEED

				loggedNotifications.Add (notification);

				audioSource.PlayOneShot(notification.notificationSound);
			}
		}

		// Otherwise, push the new notification without removing.
		else
		{
			pushIndex = loggedNotifications.Count;
			pushIndex = Mathf.Clamp (pushIndex, 0, displayLimit - 1);
			if (textReferences == null)
				Debug.Log ("TextReferences Is Null");
			textReferences[pushIndex].text = notification.message;
			loggedNotifications.Add (notification);

			audioSource.PlayOneShot(notification.notificationSound);
		}

		//UpdateText();
	}

	// Removes a notification from the Notification Log.
	// Since we do our update logic here (for efficiency) there are sometimes brief moments between removals where font colors aren't given to the right messages.
	public void RemoveNotification(Notification notification)
	{
		// Remove this notification from our logger.
		loggedNotifications.Remove (notification);
		notification = null;							// Clean-up, just in case.

		//UpdateText();
	}

	private void UpdateText()
	{
		// Make all our referenced Text UI's empty.
		for (int i = 0; i < textReferences.Count; i++)
		{
			textReferences[i].text = string.Empty;
		}
		
		// Now fill up the referenced Text UI's with our logged Notifications.
		for (int i = 0; i < loggedNotifications.Count; i++)
		{
			// If somehow this happens between updates,  prevent it. It will resolve itself next frame.
//			if (i >= textReferences.Count)
//				break;

			textReferences[i].text = loggedNotifications[i].message;
			textReferences[i].color = loggedNotifications[i].fontColor;
		}
	}
}
