using UnityEngine;
using System.Collections;

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
		#endregion
		void Awake() 
		{
			if( resControl == null )
				resControl = this;
			else
				GameObject.Destroy( this.gameObject );
		}



		private int wood  = 0;
		private int stone = 0;
		private int metal = 0;
		private int food  = 30;
		private int pop   = 5;


		/* wood - early resource material
		 * stone - mostly for early-mid game upgrades 
		 * metal - mostly late game upgrades - 2 stone -> 1 metal
		 * food - used to maintain population
		 * population - your lifeline
		*/


		public bool CanAfford( string resourceType, int amount )
		{

			return false; //TODO

		}



		//TODO resource class holder instead of ints
		private class Resource
		{
			string type;
			int amount;
			//Vector2 valueClamp;
			//TODO add values for max amount, exc
			public Resource( string type, int amount )
			{
				type = type.ToLower();
				amount = amount;
			}
		}


















	}
}