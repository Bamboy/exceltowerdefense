using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

// Matt McGrath - 4/18/2015

// Notification Log will display "Important Events" such as villagers dying. Messages will slowly disappear over time. TODO: Perhaps a scroll wheel to view older messages.
public class NotificationLog : MonoBehaviour 
{
	#region Access Instance Anywhere
	private static NotificationLog notControl;

	public static NotificationLog Get()
	{
		if( notControl != null )
		{
			return notControl;
		}
		else
		{
			GameObject go = Instantiate(Resources.Load("Prefabs/UI/Notification Log")) as GameObject; 
			go.tag = "NotificationLog";
			return notControl;
		}
	}

	void Awake() 
	{
		if( notControl == null )
		{
			notControl = this;
		}
		else
			GameObject.Destroy( this.gameObject );
	}
	#endregion

	#region Fields
	// A List (Array or other data structure later if you want) of logged notifications.
	public List<Notification> loggedNotifications = new List<Notification>();	
	public int displayLimit = 10;					// How many notifications to display at once?

	// How long to display notification? (Or should we allow Notification to customize their own timings, so very important messages stay longer? 
	// Would probably need a different data structure and approach since we no longer assume we're removing the first index from the List.
	public float displayTime = 5f;					
	public bool playNotificationSounds = true;		// Should we play the Notification sounds?
	
	private int notificationNumber = 0;
	
	public List<Text> textReferences = new List<Text>();

	Color defaultImageColor = new Color(0f, 0f, 0f, (150f / 255f));

	public bool autoExpandBox = false;

	public AudioSource audioSource;

	// Reference to the parent of the Text (a background Image).
	public Image backgroundImage;
	#endregion

	#region Initialization
	// Use this for initialization
	void Start () 
	{
		// During prototype, where we're just in the gameplay scene, we always want the Log to be Active.
		this.gameObject.SetActive(true);

		loggedNotifications = new List<Notification>();

		// Set each text reference to empty, so we don't display anything until needed.
		foreach (Text text in textReferences)
		{
			text.text = string.Empty;
		}

		audioSource = this.gameObject.AddComponent<AudioSource>();
		audioSource.volume = 0.25f;

		backgroundImage = this.gameObject.GetComponent<Image>();
	}
	#endregion

	#region Updates for Log logic
	// Update is called once per frame. Here we will manage the Notifications on the Log.
	void Update () 
	{
		// TEMP testing of pushing notifications into the log.
		if (Input.GetKeyDown (KeyCode.L))
		{
			notificationNumber++;
			float randomDisplayTime = UnityEngine.Random.Range (2f, 10f);
			Notification notification = new Notification("Testing Notifications! Message #" + notificationNumber.ToString (), Color.white, 4.0f);
			PushNotification(notification);
		}
		// End TEMP testing.

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
			backgroundImage.color = new Color(0f, 0f, 0f, 0f);
		}
		else
		{
			// Keep it enabled. Would be better served through an OnFirstNotificationAdded event but I'll keep it simple for now.
			backgroundImage.color = defaultImageColor;
		}
	}

	// We use LateUpdate to update the text of any newly-pushed or pulled notifications.
	void LateUpdate()
	{
		UpdateText();

		// If we're using the auto expand feature, expand the UI to fit the amount of notifications we have.
		if (autoExpandBox)
		{
			ExpandNotificationLog();
		}
		// Otherwise, set the height to our default (which is hardcoded for now).
		else
		{
			backgroundImage.rectTransform.SetHeight (112f);
		}
	}
	#endregion

	#region Updates for UI logic
	// Updates the Text UI elements we are references in the scene.
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
	// Expands the log's height so the background image (black border for now) is only as big as it needs to be.
	void ExpandNotificationLog()
	{
		//backgroundImage.rectTransform.sizeDelta = new Vector2(backgroundImage.rectTransform.w, rectTransform.sizeDelta.y); 
		backgroundImage.rectTransform.SetHeight (12f + loggedNotifications.Count * 10f);
	}
	#endregion

	#region Removing and Adding Notifications
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
	#endregion
}
