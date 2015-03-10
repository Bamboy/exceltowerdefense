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
        public float speed;

        public Vector3 targetInitPosition;

		public void Initalize( TowerBase owner, Enemy target, int damageOnImpact ) //Delegates are passed seperately.
		{
			this.owner = owner;
			this.target = target;
			this.dmg = damageOnImpact;
            
			//GetComponent<Rigidbody>().AddForce( VectorExtras.Direction( this.transform.position, target.transform.position ) * 30.0f, ForceMode.Impulse );
		}

		void Start()
		{
            //target.GetComponent<NavMeshAgent>().speed;
            //targetInitPosition = target.GetComponent<NavMeshAgent>().nextPosition * target.GetComponent<NavMeshAgent>().speed * (target.transform.position - transform.position).sqrMagnitude*10000;
            //targetInitPosition = target.transform.position;
            //transform.Rotate(VectorExtras.Direction(this.transform.position, target.transform.position));
			StartCoroutine("TimedDestroy"); //We might go flying off into oblivion. Clean ourselves up if so.
		}
		void Update () 
		{
            if (target != null)
            {
                transform.Translate(Vector3.forward*speed*Time.deltaTime);
                //transform.Translate(VectorExtras.Direction(this.transform.position, target.transform.position)*speed*Time.deltaTime);
                //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            }
            else
            {
                Destroy(gameObject);
            }

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