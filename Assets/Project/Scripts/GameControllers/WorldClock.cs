﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Stephan Ennen - 4/11/2015

namespace Excelsion.GameManagers
{
	// Note that all float variables are representative of seconds.
	public class WorldClock : MonoBehaviour 
	{
		#region Access Instance Anywhere
		private static WorldClock wClock;
		public static WorldClock Get()
		{
			if( wClock != null )
				return wClock;
			else
			{
				GameObject obj = new GameObject("_WorldClock");
				obj.tag = "GameController";
				wClock = obj.AddComponent< WorldClock >();

				// Let's child any Controller with a _Controllers object, creating it if it's not already present.
//				if (GameObject.Find("_Controllers") == null) {new GameObject("_Controllers");}
//				obj.transform.parent = GameObject.Find("_Controllers").transform;

				return wClock;
			}
		}
		void Awake()
		{
			if( wClock == null )
				wClock = this;
			else
				GameObject.Destroy( this.gameObject );
		}
		#endregion

		#region Events
		//Time related events for the start of a new day and the start of a new night.
		public delegate void OnDawn( int day );
		public delegate void OnDusk();
		public static OnDawn onDawn;
		public static OnDusk onDusk;
		#endregion

		#region Public Time Variables
		public static float rawTime = 0.0f; //Raw time, in seconds.
		public static int day { //What day it is.
			get{ return Mathf.CeilToInt(rawTime / totalDayLength); }
		}
		public static float totalDayProgress { //Progress of the day. (Not a percentage!)
			get{ return rawTime % totalDayLength; }
		}
		private static bool _lastDaytimeValue;
		public static bool isDaytime { //Is it day or night time?
			get{ 
				if( totalDayProgress <= dayLength )
				{
					if( _lastDaytimeValue == false )
						if( onDawn != null )
							onDawn( day );
					_lastDaytimeValue = true;
					return true;
				}
				else
				{
					if( _lastDaytimeValue == true )
						if( onDusk != null )
							onDusk();
					_lastDaytimeValue = false;
					return false;
				}
			}
		}
		public static float timeUntilDayNightSwitch { //Time in seconds until dawn or dusk
			get{ 
				if( isDaytime )
					return dayLength - totalDayProgress;
				else
					return nightLength - (totalDayProgress - dayLength);
			}
		}

		public static bool isClockBeingUsed = false;	// Adding this so we can use this script for prototype without completely altering it. We don't want a time cycle in prototype.

		//TODO - consider adding a percentage day/night progress
		#endregion

		#region Pause
		public delegate void OnPause();
		public delegate void OnUnpause();
		public static OnPause onPause;
		public static OnUnpause onUnpause;

		private static bool paused;
		public static bool Pause
		{
			get{ return paused; }
			set
			{ 
				paused = value;
				//if( value )
				//	Time.timeScale = 0f; //TODO - make our own timescale var instead of modifying the global one?
				//else
				//	Time.timeScale = 1f;
				if (paused)
					if (onPause != null)
						onPause();
				if (!paused)
					if (onUnpause != null)
						onUnpause();
			}
		}
		#endregion

		#region Private Time Settings
		public static float dayLength = 20.0f;
		public static float nightLength = 20.0f;
		public static float totalDayLength 
		{
			get{ return dayLength + nightLength; }
		}
		#endregion

		public static void WipeStatics() //Wipe static variables. Call this when loading a new game.
		{
			rawTime = 0f;
			Pause = false;
			onDawn = null;
			onDawn += DawnPause;
			onDusk = null;
			onDusk += DuskTest;
		}
		private static void DawnPause( int day )
		{ 
			// Next night enemies will spawn more often.
			//if (GameManager.InitializeWorldClock)				Matt: I need this in conjunction with my GameManager, so it doesn't create a defense controller when I'm not wanting one.
			// I'll comment it out for now until everyone starts using the GameManager prefab for their scenes.
			{
				DefenseController.Get().enemySpawnDelay -= 0.2f;					// Reduce spawn time by 20% of a second.
				Mathf.Clamp(DefenseController.Get().enemySpawnDelay, 0.5f, 5f);		// Clamp, real clamp values not given in design docs.
			}
			Debug.Log("New dawn! " + day); /*Pause = true;*/ 
			//TODO - dawn will be unpaused elsewhere via UI.
		} 

		private static void DuskTest()
		{ 
			Debug.Log("New night!"); 
		}


		[SerializeField] private Transform clockSpinner;
		[SerializeField] private Text clockDayCounter;
		void Start ()
		{
			if( clockSpinner == null )
				Debug.LogError ("ClockSpinner variable is null!");

			WipeStatics();
		}

		void Update () 
		{
			if (isClockBeingUsed)
			{
				if( Pause == false )
				{
					//Calculate time...
					rawTime += Time.deltaTime;
					UpdateVisuals();
				}
				else
				{
					//Dont calculate time, the game is paused.
				}
			}
		}
		void UpdateVisuals()
		{
			clockDayCounter.text = "Day: \n"+ day;
			clockSpinner.localRotation = Quaternion.Euler(0,0, (totalDayProgress / totalDayLength) * -360.0f );
			Debug.Log("Daytime?: "+ isDaytime +", Time unitl next switch: "+ timeUntilDayNightSwitch );

			if( isDaytime ) //This will trigger any scripts interested in changes of this value. (OnDawn, OnDusk)
				return;
		}
	}
}
