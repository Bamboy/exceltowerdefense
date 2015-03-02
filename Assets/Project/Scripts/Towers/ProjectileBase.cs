using UnityEngine;
using System.Collections;

namespace Excelsion.Towers.Projectiles
{
	//This is created by TurretBase.
	[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
	public abstract class ProjectileBase : MonoBehaviour 
	{
		public float explosionRadius = 0.0f;

		void Start () 
		{
		
		}

		void Update () 
		{
		
		}


		//Do things like explode here.
		virtual void OnCollisionEnter2D( Collision2D col )
		{
			return;
		}

		virtual void OnExplosion(  ) //Pass objects affected here.
		{
			return;
		}

	}
}