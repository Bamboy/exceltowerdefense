using UnityEngine;
using System.Collections;
using Excelsion.GameManagers;
using Excelsion.UI;

//Stephan Ennen - 3/17/2015

namespace Excelsion.Enemies
{
	[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(NavMeshAgent))]
	public class Enemy : MonoBehaviour 
	{
		private bool DO_DEBUG = false;
		private bool faceHeading = true;
		private float damping = 0.4f;
		private float speed = 5.0f;
		public float Speed{ get{ return this.speed; } set{ this.speed = Mathf.Max(0f, value); } }
		private float defaultSpeed; //We use this to remove status effects.

		private Vector3 targetPosition;
		private Vector3 targetInRangePosition;
		private Vector3 targetHeading;
		private Vector3 currentHeading;

		public int health = 30; //TODO - Add status effects TODO - Add health percentage display.
		private int maxHealth;
		public HealthBar healthDisplay;
		public virtual void Start () 
		{
			maxHealth = health;
			healthDisplay.MaxHealth = health;
			defaultSpeed = speed;
			GetComponent<Rigidbody>().isKinematic = true;
			currentHeading = transform.forward;

            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            targetPosition = DefenseController.Get().enemyObjective.transform.position;
            agent.destination = targetPosition;
		}

		#region Movement

		public virtual void DoMovement()
		{
			targetPosition = DefenseController.Get().enemyObjective.transform.position;
			targetInRangePosition = VectorExtras.OffsetPosInPointDirection( new Vector3(targetPosition.x, transform.position.y, targetPosition.z), 
			                        transform.position, 8.0f );
			if( Vector3.Distance( transform.position, targetInRangePosition ) <= 1.0f )
			{
				if( faceHeading )
					transform.LookAt( new Vector3(targetPosition.x, transform.position.y, targetPosition.z) );
				return;
			}
			else
			{

				/*DEBUG*/ if( DO_DEBUG ) Debug.DrawLine(transform.position, targetInRangePosition, Color.yellow, 0.25f);

				targetHeading = VectorExtras.Direction( transform.position, targetInRangePosition );

				/*DEBUG*/ if( DO_DEBUG ) Debug.DrawRay(transform.position, targetHeading, Color.blue, 0.25f);

				currentHeading = Vector3.Lerp( currentHeading, targetHeading, Time.deltaTime * damping );

				/*DEBUG*/ if( DO_DEBUG ) Debug.DrawRay(transform.position, currentHeading, Color.red, 0.25f);

				transform.position += ( currentHeading * speed * Time.deltaTime );
				
				RaycastHit data;
				if( Physics.Raycast(transform.position, Vector3.down, out data, 250.0f ) )
				{
					transform.position = new Vector3( transform.position.x, data.point.y + 1.0f, transform.position.z );
					currentHeading.y = 0.0f;
				}
				if( faceHeading )
					transform.LookAt( transform.position + currentHeading );
			}
		}
		#endregion



		void Update()
		{
			//Evaluate status effects.
			EvaluateFire();
			EvaluateCold();
		}

		#region Health
		public virtual void Damage( int val )
		{
			health -= val;
			if( health <= 0 )
			{
				Kill();
			}
			healthDisplay.CurrentHealth = health;
		}
		public void Kill()
		{
			OnKilled();
			DefenseController.Get().enemies.Remove( this );
			Destroy( this.gameObject );
		}
		public int moneyValue = 3;
		public virtual void OnKilled()
		{
			DefenseController.money += moneyValue;
			Debug.Log("BLERHGhnsfm...");
			return; //TODO add chance to drop resources here.
		}
		#endregion





		#region Status Effects
		//=========== FIRE ===========
		public float fire = 0.0f; //This acts as our timer.
		public bool OnFire{ get{ return fire > 0.0f; } } //Return true if our timer is active.
		public int fireTickDamage = 2; //Damage to do every second
		public void SetFire( float duration )
		{
			if( OnFire )//Add time to existing fire.
				fire += duration;
			else
			{ //Start new fire.
				fire = duration;
				StartCoroutine("FireTick", 1.0f);
				healthDisplay.ShowFire(true);
			}
		}
		void EvaluateFire()
		{
			if( OnFire )
			{
				fire -= Time.deltaTime;
				if( OnFire == false )
				{
					fire = 0.0f; //Stop fire.
					StopCoroutine("FireTick");
					healthDisplay.ShowFire(false);
				}
			}
		}
		IEnumerator FireTick( float tickTime )
		{
			yield return null;
			while( OnFire == true )
			{
				Damage( fireTickDamage ); //Deal damage then wait 'tickTime'.
				yield return new WaitForSeconds( tickTime );
			}
			Debug.Log("Escaped the flames!", this);
		}

		//=========== COLD ===========
		public float cold = 0.0f;
		public bool IsCold{ get{ return cold > 0.0f; } }
		public float coldMovementModifier = 0.50f; //Percentage at which effected enemies will move at. (50% normal speed, here)
		public void SetCold( float duration )
		{
			if( IsCold )
				cold += duration;
			else
			{   //Start new cold
				speed = coldMovementModifier * defaultSpeed;
				cold = duration;
				healthDisplay.ShowCold(true);
			}
		}
		void EvaluateCold()
		{
			if( IsCold )
			{
				cold -= Time.deltaTime;
				if( IsCold == false ) //End cold.
				{
					cold = 0.0f;
					speed = defaultSpeed; //Restore our normal speed.
					healthDisplay.ShowCold(false);
				}
			}
		}
		#endregion




	}
}