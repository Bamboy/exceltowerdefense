using UnityEngine;
using System.Collections;
using Excelsion.Enemies;

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
		public Enemy target;
		public int dmg;

		public void Initalize( TowerBase owner, Enemy target, int damageOnImpact ) //Delegates are passed seperately.
		{
			this.owner = owner;
			this.target = target;
			this.dmg = damageOnImpact;
			GetComponent<Rigidbody>().AddForce( VectorExtras.Direction( this.transform.position, target.transform.position ) * 30.0f, ForceMode.Impulse );
		}

		void Start()
		{
			StartCoroutine("TimedDestroy"); //We might go flying off into oblivion. Clean ourselves up if so.
		}
		void Update () 
		{
			//if( onUpdateEvent != null )
			//	onUpdateEvent();
		}

		//Do things like explode here.
		public virtual void OnCollisionEnter( Collision col )
		{
			if( col.gameObject.tag == "Enemy" )
			{
				Enemy e = col.gameObject.GetComponent< Enemy >();
				e.Damage( dmg );
			}

			//if( onImpactEvent != null )
			//	onImpactEvent();

			GameObject.Destroy( this.gameObject );
		}

		IEnumerable TimedDestroy()
		{
			yield return new WaitForSeconds( 10.0f );
			GameObject.Destroy( this.gameObject );
		}











	}
}