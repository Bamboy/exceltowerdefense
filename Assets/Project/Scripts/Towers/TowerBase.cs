using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Excelsion.Inventory;

namespace Excelsion.Towers
{
	//All towers have this. Manages items, UI, and creates a TurretBase component depending on items.
	[RequireComponent(typeof(Collider))]
	public class TowerBase : MonoBehaviour 
	{
		public Turret turret;
		public Bag inventory;

		private bool isBuilt = false;

		void Start () 
		{
			isBuilt = false;
			inventory = new Bag( 0 ); //Start with an empty inventory because at the start of a game we wont have a working tower.
		}

		void Update () 
		{
			
		}

		//Called when the mouse is over our collider and is clicked.
		void OnMouseDown()
		{
			this.GetComponent<Renderer>().material.color = Color.blue;
		}
	}
}