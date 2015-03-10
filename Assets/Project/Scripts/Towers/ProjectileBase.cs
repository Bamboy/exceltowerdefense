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
		public delegate void OnUpdateEvent( ProjectileBase projectile );
		public delegate void OnEnemySelection( out Enemy[] enemies, ProjectileBase projectile );
		public delegate void OnEnemiesHit( Enemy[] enemies );

		public OnUpdateEvent onUpdateEvent;
		public OnEnemySelection onEnemySelection;
		public OnEnemiesHit onEnemiesHit;

		public TowerBase owner;
		public Enemy target;
		public int dmg;
        public float speed;

        public Vector3 travelDir;

		public void Initalize( TowerBase owner, Enemy target, int damageOnImpact ) //Delegates are passed seperately.
		{
			this.owner = owner;
			this.target = target;
			this.dmg = damageOnImpact;
		}

		void Start()
		{
			StartCoroutine("TimedDestroy"); //We might go flying off into oblivion. Clean ourselves up if so.
		}
		void Update () 
		{



			if( onUpdateEvent != null )
				onUpdateEvent( this );
			else
				transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}

		//Do things like explode here.
		public virtual void OnCollisionEnter( Collision col )
		{
			//Array of enemies that will be effected. Start empty
			Enemy[] enemiesHit = new Enemy[0];

			//If we have any enemy selection delegates, have them modify our array.
			if( onEnemySelection != null )
				onEnemySelection( out enemiesHit, this );
			else //..Otherwise just use the single enemy that was collided with.
			{
				if( col.gameObject.tag == "Enemy" )
				{
					Enemy e = col.gameObject.GetComponent< Enemy >(); //TODO - make this more fail-safe
					if( e != null )
						enemiesHit = ArrayTools.Push< Enemy >( enemiesHit, e );
					else
					{
						GameObject.Destroy( this.gameObject );
						return;
					}
				}
			}

			//Pass all enemies that should be effected to our items. (So they may apply status effects or whatever)
			if( onEnemiesHit != null )
				onEnemiesHit( enemiesHit );

			//Deal damage to all enemies effected.
			foreach( Enemy h in enemiesHit )
			{
				h.Damage( dmg );
			}


			GameObject.Destroy( this.gameObject );
		}

		IEnumerable TimedDestroy()
		{
			yield return new WaitForSeconds( 10.0f );
			GameObject.Destroy( this.gameObject );
		}











	}
}