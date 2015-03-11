using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Excelsion.Enemies;
using Excelsion.Inventory;

//Tristan Kidder - 3/11/2015

namespace Excelsion.GameManagers
{
	public class InventoryController : MonoBehaviour
	{
		#region Access Instance Anywhere
		private static InventoryController invControl;
		public static InventoryController Get()
		{
			if( invControl != null )
				return invControl;
			else
			{
				GameObject obj = new GameObject("_InventoryController");
				obj.tag = "GameController";
				invControl = obj.AddComponent< InventoryController >();
				return invControl;
			}
		}
		#endregion
		
		
		void Awake()
		{
			if( invControl == null )
				invControl = this;
			else
				GameObject.Destroy( this.gameObject );
		}

		void Start()
		{

		}

		void Update()
		{

		}
	}
}

