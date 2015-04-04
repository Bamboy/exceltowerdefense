using UnityEngine;
using System.Collections;

//Stephan Ennen - 4/2/2015

namespace Excelsion.GameManagers
{
	public class ResourceController : MonoBehaviour 
	{
		#region Access Instance Anywhere
		private static ResourceController resControl;
		public static ResourceController Get()
		{
			if( resControl != null )
				return resControl;
			else
			{
				GameObject obj = new GameObject("_ResourceController");
				obj.tag = "GameController";
				resControl = obj.AddComponent< ResourceController >();
				return resControl;
			}
		}
		void Awake() 
		{
			if( resControl == null )
				resControl = this;
			else
				GameObject.Destroy( this.gameObject );
		}
		#endregion

		/* wood - early resource material
		 * stone - mostly for early-mid game upgrades 
		 * metal - mostly late game upgrades - 2 stone -> 1 metal
		 * food - used to maintain population
		 * population - your lifeline
		*/


		//Keep these private from other scripts. Make it so other scripts use functions in order to get or change these values.
		private int wood  = 0;
		private int stone = 0;
		private int metal = 0;
		private int food  = 30;
		private int pop   = 5;

		void Start()
		{
			wood = 5;
			stone = 5;
			metal = 3;
			food = 30;
			pop = 5;
		}


		//Return true if resource isn't negative if we subtract 'amount' from it. Used if the player tries to buy something, for example.
		public bool CanAffordWood( int amount )
		{
			return (wood - amount >= 0) ? true : false;
		}




















	}
}