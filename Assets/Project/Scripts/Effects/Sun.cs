using UnityEngine;
using System.Collections;
using Excelsion.GameManagers;

//Stephan Ennen - 4/11/2015

namespace Excelsion.Effects
{
	//Controls the directional light representing the sun to simulate day/night cycle. Reads values from WorldClock.cs

	[RequireComponent(typeof(Light))]
	public class Sun : MonoBehaviour 
	{
		public Gradient daySpectrum;
		public Gradient nightSpectrum;

		private Light dLight;
		void Start () {
			dLight = GetComponent<Light>();
			dLight.type = LightType.Directional;
		}

		void Update () //TODO - Make the sun rotate so that shadows are cast.
		{
			dLight.color = GetLightColor();
		}

		Color GetLightColor()
		{
			if( WorldClock.isDaytime )
				return daySpectrum.Evaluate( WorldClock.timeUntilDayNightSwitch / WorldClock.dayLength );
			else
				return nightSpectrum.Evaluate( WorldClock.timeUntilDayNightSwitch / WorldClock.nightLength );
		}
	}
}
