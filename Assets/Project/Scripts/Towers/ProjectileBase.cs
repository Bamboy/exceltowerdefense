using UnityEngine;
using System.Collections;

//Stephan Ennen - 3/3/2015

namespace Excelsion.Towers.Projectiles
{
	//This is created by TurretBase.
	[RequireComponent(typeof(Rigidbody), typeof(Collider))]
	public class ProjectileBase : MonoBehaviour 
	{
		public delegate void onImpactEvent();
		public delegate void onUpdateEvent();

		public TowerBase owner;
		public Vector3 target;

		void Start () 
		{
			
		}
		public void Initalize( TowerBase owner, Vector3 target ) //Delegates are passed seperately.
		{
			this.owner = owner;
			this.target = target;
		}
		void Update () 
		{
			
		}


		//Do things like explode here.
		public virtual void OnCollisionEnter( Collision col )
		{
			return;
		}

	}
}