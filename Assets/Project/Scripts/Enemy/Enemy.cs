﻿using UnityEngine;
using System.Collections;
using Excelsion.GameManagers;
using Excelsion.UI;
using System.Collections.Generic;
using UnityEngine.UI;


//Stephan Ennen - 3/17/2015


namespace Excelsion.Enemies
{
	[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(NavMeshAgent))]
	public class Enemy : MonoBehaviour 
	{
		#region Fields
		// Matt McGrath 4/24/2015
		public float chanceToDropReward = 0.05f;								// A PERCENTAGE chance this Enemy will drop an item upon death.
		public List<StatusEffect> statusEffects = new List<StatusEffect>();		// A List of the Status Effects plaguing this Enemy.
		// Random Enemy names: For fun!
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

		public float defaultSpeed; 				// We use this to return Enemy to normal Speed after a status effect that affects Speed is over.
		private NavMeshAgent navigation;

		private Vector3 targetPosition;
		private Vector3 targetInRangePosition;
		private Vector3 targetHeading;
		private Vector3 currentHeading;

		public int health = 30; 			//TODO - Add health percentage display.
		public int maxHealth;				// We need this public for the DefenseController.
		public HealthBar healthDisplay;

		public int moneyValue = 3;
		#endregion

		#region Initialization
		public virtual void Start() 
		{
			maxHealth = health;
			healthDisplay.MaxHealth = health;
			GetComponent<Rigidbody>().isKinematic = true;
			currentHeading = transform.forward;

			//Set the target position based on the closest house thats setup.
//			if (DefenseController.Get().houses.Count > 1 )
//			{
//				targetPosition = GetClosestTarget(DefenseController.Get().houses);
//			}
//			else 
			{
				targetPosition = DefenseController.Get().enemyObjective.transform.position;
			}
            navigation = GetComponent<NavMeshAgent>();
            navigation.destination = targetPosition;
			Speed = speed;
			defaultSpeed = speed;
		}
		#endregion

		#region Movement

		public virtual void DoMovement()
		{
			targetPosition = DefenseController.Get().enemyObjective.transform.position;
			//targetPosition = GetClosestTarget(DefenseController.Get().houses);
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

		#region Update
		void Update()
		{
			// Evaluate status effects.
			foreach (StatusEffect effect in statusEffects)
			{
				effect.EvaluateStatusEffect();
			}

			// If enemy reaches the target, destroy itself.

			// Note from Matt: This never happens, because the target has a radius of 7.5. Use collisions or take into account the target's actual radius.
			if (Vector3.Distance(transform.position, targetPosition) <= 7.75)//5f) 
			{
				Kill (false);

				// Here we will deduct population or whatever else we want to happen when enemy reaches a house.
				ResourceController.Get().RemoveResource (ResourceType.Population, 1);
				if (ResourceController.Get ().ResourceAmount(ResourceType.Population) <= 0)
				{
					// This message got annoying lol
//				    NotificationLog.Get().PushNotification(new Notification("NO MORE VILLAGERS LEFT: GAME OVER!", Color.red, 5.0f));
				}
				else
					NotificationLog.Get().PushNotification(new Notification("Enemy reached target! Villager lost!", Color.red, 5.0f));
			}

		}
		#endregion

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
			if (giveRewards)
			{
				OnKilled();
			}
			DefenseController.Get().enemies.Remove(this);
			Destroy(this.gameObject);
		}

		public virtual void OnKilled()
		{
			DefenseController.money += moneyValue;

			// See if we should reward an item; if not, leave OnKilled().
			if (Random.Range (0f, 1f) > chanceToDropReward)
				return;

			GameResources reward = new GameResources();
			ResourceType resourceType = ResourceType.None;
			GiveRandomRewards(out reward, out resourceType);

			if (reward.Food > 0 || reward.Wood > 0 || reward.Stone > 0 || reward.Metal > 0)
			{
				string randomName = names[Random.Range (0, names.Length)];
				NotificationLog.Get ().PushNotification(new Notification(randomName + " Died!" + " Dropped 1 " + resourceType.ToString(), Color.yellow, 5.0f));
			}

		}
		#endregion

		#region Reward Giving
		// Matt McGrath 4/21/2015: Give a random reward (reward for now). Added second parameter for use with notification system. This of course assumes only one reward can be given.
		private void GiveRandomRewards(out GameResources rewards, out ResourceType typeGiven)
		{
			// Defaults our rewards to zero of each resource type.
			GameResources reward = new GameResources();
			ResourceType resourceGiven = ResourceType.None;

			// Sloppy method for testing purposes and notifying what reward was given.
			while (resourceGiven == ResourceType.None)
			{
				if (Random.Range (0f, 1f) <= ResourceController.Get().metalDropChance)
				{
					reward.Metal = 1;
					resourceGiven = ResourceType.Metal;
					break;
				}

				if (Random.Range (0f, 1f) <= ResourceController.Get().stoneDropChance)
				{
					reward.Stone = 1;
					resourceGiven = ResourceType.Stone;
					break;
				}

				if (Random.Range (0f, 1f) <= ResourceController.Get().woodDropChance)
				{
					reward.Wood = 1;
					resourceGiven = ResourceType.Wood;
					break;
				}

				if (Random.Range (0f, 1f) <= ResourceController.Get().foodDropChance)
				{
					reward.Food = 1;
					resourceGiven = ResourceType.Food;
					break;
				}
			}

			ResourceController.Get().AddResources(reward);

			rewards = reward;
			typeGiven = resourceGiven;
		}
		#endregion

		#region Closest Enemy Tracking
		// Jimmy Westcott Apr 22,2015 - Evaluate what the closest target is
		private Vector3 GetClosestTarget(List<GameObject> houses) 
		{
			Vector3 position = new Vector3();
			float shortestDistance = 0f;
			float currentDistance = 0f;
			bool first = true;

			foreach( GameObject house in houses) 
			{
				currentDistance = Vector3.Distance(transform.position,house.transform.position);
				if (first)
				{
					shortestDistance = currentDistance;
					position = house.transform.position;
					first = false;
				}

				if (currentDistance < shortestDistance) 
				{
					position = house.transform.position;
					shortestDistance = currentDistance;
					//Debug.Log ("Closest house is: " + house.name);
				}
			}

			return position;
		}
		#endregion
	}
}