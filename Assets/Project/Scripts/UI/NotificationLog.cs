using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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












	// A List (Array or other data structure later if you want lol) of logged notifications. We drag these from the scene and into our NotificationLog in the hierarchy.
	public List<Notification> loggedNotifications = new List<Notification>();	
	public int displayLimit = 3;	// How many notifications to display at once?

	// Variables for testing.
	//private float timeSinceLastNotification = 0.5f;
	//private float autoNotificationTestTimer = 0.5f;
	// int maxAutoMessages = 10;
	private int notificationNumber = 0;
	
	public List<Text> textReferences = new List<Text>();

	Color defaultImageColor = new Color(0f, 0f, 0f, 0.5f);

	// Use this for initialization
	void Start () 
	{
		loggedNotifications = new List<Notification>();
		
		foreach (Text text in textReferences)
		{
			text.text = string.Empty;
		}
	}
	
	// Update is called once per frame. Here we will manage the Notifications on the Log.
	void Update () 
	{
//		timeSinceLastNotification -= Time.deltaTime;
//		if (timeSinceLastNotification <= 0f)
//		{
//			//if (loggedNotifications.Count < displayLimit &&  notificationNumber < maxAutoMessages)
//			if (notificationNumber < maxAutoMessages)
//			{
//				notificationNumber++;
//				Notification notification = new Notification("Message #" + notificationNumber.ToString (), Color.white, 7.5f);
//				PushNotification(notification);
//
//				timeSinceLastNotification = autoNotificationTestTimer;
//			}
//		}

		// TEMP TESTING
		if (Input.GetKeyDown (KeyCode.L))
		{
			notificationNumber++;
			Notification notification = new Notification("Testing Notifications! Message #" + notificationNumber.ToString (), Color.yellow, 4.0f);
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


	int pushIndex = -1;

	// Adds a notification to the Notification Log.
	public void PushNotification(Notification notification)
	{
		// Do we prematurely remove the oldest message if we need room for this new one? What if we get 10 notifications all at once? Then we'll never have time to read the first 5.
		// For now let's pretend we remove the oldest message anyways, even if it's not ready to "die" off.
		if (loggedNotifications.Count >= displayLimit)
		{
			Debug.Log ("We're at max capacity! We'll push new message anyways?");
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
				pushIndex = loggedNotifications.Count;
				pushIndex = Mathf.Clamp (pushIndex, 0, displayLimit - 1);
				textReferences[pushIndex].text = notification.message;
				loggedNotifications.Add (notification);
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
		}
	}

	// Removes a notification from the Notification Log.
	public void RemoveNotification(Notification notification)
	{
		//Debug.Log ("Removing notification " + notification.message);
		notification.message = "";

		// Otherwise, blank out the bottom text field.
		textReferences[0].text = "";

		// Remove this notification from our logger.
		loggedNotifications.Remove (notification);
		notification = null;							// Clean-up, just in case.

		for (int i = 0; i < displayLimit - 1; i++)
		{
			if (i > displayLimit)
				break;

			textReferences[i].text = textReferences[i + 1].text;
		}

		if (loggedNotifications.Count == 0)
		{
			foreach (Text text in textReferences)
			{
				text.text = "";
			}
		}
	}
}
