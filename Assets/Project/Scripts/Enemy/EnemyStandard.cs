using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Excelsion.GameManagers;
using Excelsion.UI;

// Matt McGrath - 5/10/2015
namespace Excelsion.Enemies
{
	// The Standard Enemy type is one which is the most prevelant and basic -- the ones we have been working with so far.
	[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(NavMeshAgent))]
	public class EnemyStandard : EnemyBase 
	{
		#region Fields

		#endregion
		
		#region Initialization
		public override void Start() 
		{
			maxHealth = health;
			healthDisplay.MaxHealth = health;
			GetComponent<Rigidbody>().isKinematic = true;			// ISSUE: Why isn't this staying set?!
			
			currentHeading = transform.forward;
			
			//Set the target position based on the closest house thats setup.
			if (DefenseController.Get().houses.Count > 1 )
			{
				targetPosition = GetClosestTarget(DefenseController.Get().houses);
			}
			else 
			{
				targetPosition = DefenseController.Get().enemyObjective.transform.position;
			}
			navigation = GetComponent<NavMeshAgent>();
			navigation.destination = targetPosition;
			Speed = speed;
			defaultSpeed = speed;
		}
		#endregion

		#region Update
		public override void Update()
		{
			Debug.Log ("Updating");
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
		
		#region Movement
		public override void DoMovement()
		{
			//targetPosition = DefenseController.Get().enemyObjective.transform.position;
			targetPosition = GetClosestTarget(DefenseController.Get().houses);
			targetInRangePosition = VectorExtras.OffsetPosInPointDirection(new Vector3(targetPosition.x, transform.position.y, targetPosition.z), 
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
				if (Physics.Raycast(transform.position, Vector3.down, out data, 250.0f))
				{
					transform.position = new Vector3(transform.position.x, data.point.y + 1.0f, transform.position.z);
					currentHeading.y = 0.0f;
				}
				if (faceHeading)
					transform.LookAt(transform.position + currentHeading);
			}
		}
		#endregion
		
		#region Health
		// Damage the Enemy.
		public override void Damage(int val)
		{
			// Reduce Enemy's health, then kill if it's 0 or below.
			health -= val;
			if (health <= 0)
			{
				Kill(true);
			}

			// Update enemy's health bar to show his new health.
			healthDisplay.CurrentHealth = health;
		}

		// Kill the Enemy, giving rewards if true is passed as a parameter.
		public override void Kill(bool giveRewards)
		{
			if (giveRewards)
			{
				OnKilled();
			}
			// Remove this Enemy from our DefenseController and destroy it.
			//DefenseController.Get().enemies.Remove(this);
			Destroy(this.gameObject);
		}

		// Called during the Kill method; specifies what happens upon the Enemy's death. For now, have a reward dropped chance.
		public override void OnKilled()
		{
			// Reward money (to the player?) based on the enemy's death value amount. 
			DefenseController.money += moneyValue;
			
			// See if we should reward an item; if not, return immediately.
			if (Random.Range(0f, 1f) > chanceToDropReward)
				return;

			// Create GameResources and a ResourceType, since we will only return ONE type of reward.
			GameResources reward = new GameResources();
			ResourceType resourceType = ResourceType.None;
			GiveRandomRewards(out reward, out resourceType);

			// If a reward was dropped, notify the logger. (This could be part of a method check instead.)
			if (reward.Food > 0 || reward.Wood > 0 || reward.Stone > 0 || reward.Metal > 0)
			{
				string randomName = names[Random.Range (0, names.Length)];
				NotificationLog.Get ().PushNotification(new Notification(randomName + " Died!" + " Dropped 1 " + resourceType.ToString(), Color.yellow, 5.0f));
			}
		}
		#endregion
		
		#region Reward Giving
		// Matt McGrath 4/21/2015: Give a random reward (reward for now). Added second parameter for use with notification system. This of course assumes only one reward can be given.
		protected override void GiveRandomRewards(out GameResources rewards, out ResourceType typeGiven)
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
		protected override Vector3 GetClosestTarget(List<GameObject> houses) 
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