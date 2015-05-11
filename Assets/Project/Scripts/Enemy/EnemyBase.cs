using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Excelsion.GameManagers;
using Excelsion.UI;

// Matt McGrath - 5/10/2015
namespace Excelsion.Enemies
{
	// Base class for an Enemy unit. Includes functionality all enemy types may have in common.
	[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(NavMeshAgent))]
	public abstract class EnemyBase : MonoBehaviour 
	{
		#region Fields
		public float chanceToDropReward = 0.05f;								// A PERCENTAGE (0-1) chance this Enemy will drop an item upon death.
		public List<StatusEffect> statusEffects = new List<StatusEffect>();		// A List of the Status Effects plaguing this Enemy.

		// Random Enemy names: For fun!
		public string[] names = new string[]{ "Bryan", "Sergey", "Tristan", "Stephan", "Bryan", "Dann", "David", "Imran", "Jake", "Jessin", "Matt", "Jimmy", "Joshua" };
		
		protected bool DO_DEBUG = false;
		protected bool faceHeading = true;
		protected float damping = 0.4f;
		protected float speed = 8.0f;
		public float Speed
		{ 
			get { return this.speed; } 
			set
			{ 
				this.speed = Mathf.Max(0f, value); 
				navigation.speed = this.speed;
			} 
		}
		
		public float defaultSpeed; 				// We use this to return Enemy to normal Speed after a status effect that affects Speed is over.
		protected NavMeshAgent navigation;
		
		protected Vector3 targetPosition;
		protected Vector3 targetInRangePosition;
		protected Vector3 targetHeading;
		protected Vector3 currentHeading;
		
		public int health = 30; 				//TODO - Add health percentage display.
		public int maxHealth;					// We need this public for the DefenseController.
		public HealthBar healthDisplay;
		public int moneyValue = 3;
		#endregion

		#region Overrideable Methods
		public abstract void Start();						// Overrides the MonoBehavior Start
		public abstract void Update();						// Overrides the MonoBehavior Update
		public abstract void DoMovement();
		public abstract void Damage(int val);
		public abstract void Kill(bool giveRewards);
		public abstract void OnKilled();
		protected abstract void GiveRandomRewards(out GameResources rewards, out ResourceType typeGiven);
		protected abstract Vector3 GetClosestTarget(List<GameObject> houses);
		#endregion
	}
}
