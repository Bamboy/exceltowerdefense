using UnityEngine;
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

		#region Public Time Variables
		public static float rawTime = 0.0f; //Raw time, in seconds.
		public static int day { //What day it is.
			get{ return Mathf.CeilToInt(rawTime / totalDayLength); }
		}
		public static float totalDayProgress { //Progress of the day. (Not a percentage!)
			get{ return rawTime % totalDayLength; }
		}
		public static bool isDaytime { //Is it day or night time?
			get{ 
				if( totalDayProgress <= dayLength )
					return true;
				else
					return false;
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
		//TODO - consider adding a percentage day/night progress
		#endregion

		private static bool paused;
		public static bool Pause{
			get{ return paused; }
			set{ 
				paused = value; 
			}
		}

		#region Private Time Settings
		public static float dayLength = 15.0f;
		public static float nightLength = 15.0f;
		public static float totalDayLength {
			get{ return dayLength + nightLength; }
		}
		#endregion

		public static void WipeStatics() //Wipe static variables. Call this when loading a new game.
		{
			rawTime = 0f;
		}

		[SerializeField] private Transform clockSpinner;
		[SerializeField] private Text clockDayCounter;
		void Start ()
		{
			if( clockSpinner == null )
				Debug.LogError ("ClockSpinner variable is null!");
		}

		void Update () 
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
		void UpdateVisuals()
		{
			clockDayCounter.text = "Day: \n"+ day;
			clockSpinner.localRotation = Quaternion.Euler(0,0, (totalDayProgress / totalDayLength) * -360.0f );
			//Debug.Log("Daytime?: "+ isDaytime +", Time unitl next switch: "+ timeUntilDayNightSwitch );
		}
	}
}
