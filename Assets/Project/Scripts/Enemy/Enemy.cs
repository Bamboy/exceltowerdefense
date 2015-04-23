using UnityEngine;
using System.Collections;
using Excelsion.GameManagers;
using Excelsion.UI;

//Stephan Ennen - 3/17/2015
using System.Collections.Generic;
using UnityEngine.UI;

namespace Excelsion.Enemies
{
	[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(NavMeshAgent))]
	public class Enemy : MonoBehaviour 
	{
		// Matt McGrath 4/19
		public List<StatusEffect> statusEffects = new List<StatusEffect>();
		// Matt McGrath 4/21: Adding this for funsies
		public string[] names = new string[]{ "Bryan", "Sergey", "Tristan", "Stephan", "Bryan", "Dann", "David", "Imran", "Jake", "Jessin", "Matt", "Jimmy", "Joshua" };

		private bool DO_DEBUG = false;
		private bool faceHeading = true;
		private float damping = 0.4f;
		private float speed = 8.0f;
		public float Speed
		{ 
			get{ return this.speed; } 
			set{ 
				this.speed = Mathf.Max(0f, value); 
				navigation.speed = this.speed;
			} 
		}

		// MATT: TEMPORARILY MAKING THIS PUBLIC FOR TESTING SOMETHING -- was private
		public float defaultSpeed; 	// We use this to remove status effects.
		private NavMeshAgent navigation;

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
			GetComponent<Rigidbody>().isKinematic = true;
			currentHeading = transform.forward;

			//Set the target position based on the closest house thats setup.
			if (DefenseController.Get().houses.Count > 1 ){
				targetPosition = GetClosestTarget(DefenseController.Get().houses);
			}
			else {
				targetPosition = DefenseController.Get().enemyObjective.transform.position;
			}
            navigation = GetComponent<NavMeshAgent>();
            navigation.destination = targetPosition;
			Speed = speed;
			defaultSpeed = speed;
		}

		#region Movement

		public virtual void DoMovement()
		{
			//targetPosition = DefenseController.Get().enemyObjective.transform.position;
			targetPosition = GetClosestTarget(DefenseController.Get().houses);
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
			foreach (StatusEffect effect in statusEffects)
			{
				effect.EvaluateStatusEffect();
			}
			//If enemy reaches the target, destroy itself.
			if (Vector3.Distance(transform.position,targetPosition) <= 5f) {
				Kill (false);
				//Here we will deduct population or whatever else we want to happen when enemy reaches a house.
			}

		}

		#region Health
		public virtual void Damage( int val )
		{
			health -= val;
			if( health <= 0 )
			{
				Kill(true);
			}
			healthDisplay.CurrentHealth = health;
		}
		public void Kill(bool giveRewards)
		{
			if (giveRewards){
				OnKilled();
			}
			DefenseController.Get().enemies.Remove( this );
			Destroy( this.gameObject );
		}
		public int moneyValue = 3;
		public virtual void OnKilled()
		{
			DefenseController.money += moneyValue;

			Reward reward = null;
			GiveRandomRewards(out reward);

			string randomName = names[Random.Range (0, names.Length)];
			NotificationLog.Get ().PushNotification(new Notification(randomName + " Died!" + " Dropped: " + reward.food + "f, " + reward.wood + "w, " 
			                                                         + reward.stone + "s, " + reward.metal + "m", Color.yellow, 7.0f));

			return; //TODO add chance to drop resources here.
		}
		#endregion

		// Matt McGrath 4/21/2015: Temporary for testing usability of ResourceController from other scripts.
		private void GiveRandomRewards(out Reward rewards)
		{
			// Defaults our rewards to one of each resource type.
			Reward reward = new Reward();

			reward.pop = 0;				// We don't reward population.

			if (Random.Range (0f, 100f) <= ResourceFood.baseDropChance)
		    {
				// Reward 1 food.
				reward.food = 1;
			}

			if (Random.Range (0f, 100f) <= ResourceWood.baseDropChance)
			{
				reward.wood = 1;
			}

			if (Random.Range (0f, 100f) <= ResourceStone.baseDropChance)
			{
				reward.stone = 1;
			}

			if (Random.Range (0f, 100f) <= ResourceMetal.baseDropChance)
			{
				reward.metal = 1;
			}

			ResourceController.Get().GainReward(reward);

			rewards = reward;
		}

		// Jimmy Westcott Apr 22,2015 - Evaluate what the closest target is
		private Vector3 GetClosestTarget(List<GameObject> houses) 
		{
			Vector3 position = new Vector3();
			float shortestDistance = 0f;
			float currentDistance = 0f;
			bool first = true;

			foreach( GameObject house in houses) {

				currentDistance = Vector3.Distance(transform.position,house.transform.position);
				if (first){
					shortestDistance = currentDistance;
					position = house.transform.position;
					first = false;
				}

				if (currentDistance < shortestDistance) {
					position = house.transform.position;
					shortestDistance = currentDistance;
					//Debug.Log ("Closest house is: " + house.name);
				}
			}

			return position;
		}
	}
}