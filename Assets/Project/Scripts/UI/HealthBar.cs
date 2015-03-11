using UnityEngine;
using System.Collections;

namespace Excelsion.UI
{
	public class HealthBar : MonoBehaviour 
	{
		private Camera cam;
		public Transform parent;
		public Transform healthAnchor;
		public float hoverHeight = 2.0f;

		//Scale relative to screen.
		public Vector2 minScale;
		public Vector2 maxScale;

		[Tooltip("X - Min distance where drawing starts. Y - Min distance where object is fully opaque. " +
			"Z - Max distance where object is fully opaque. W - Max distance where drawing ends.")]
		public Vector4 fadeDistances = new Vector4( 5.0f, 10.0f, 150.0f, 175.0f );

		void Start () 
		{
			cam = Camera.main;
			if( parent == null )
			{
				parent = transform.parent;
				hoverHeight = transform.localPosition.y;
			}
			else
				transform.localPosition = new Vector3( 0.0f, hoverHeight, 0.0f );
		}

		#region Health values
		private int healthMax;
		private int healthCur;
		private float healthPer;
		public int MaxHealth {
			get{ return healthMax; }
			set{
				healthMax = value;
				if( healthCur == 0 )
					CurrentHealth = healthMax; //This also sets percentage and updates display.
				else
				{
					healthPer = (float)healthCur / (float)healthMax;
					UpdateValues();
				}
			}
		}
		public int CurrentHealth {
			get{ return healthCur; }
			set{ 
				healthCur = value;
				healthPer = (float)healthCur / (float)healthMax;
				UpdateValues();
			}
		}

		//This updates our bars to display value(s).
		public void UpdateValues()
		{
			healthAnchor.localScale = new Vector3( healthPer, 1.0f, 1.0f );
		}
		#endregion

		void Update() //TODO make this onprerender
		{
			UpdateDisplay();
		}


		//This updates our transform relative to the main camera. (NOT THE VALUES)
		public void UpdateDisplay()
		{
			float camDistance = Vector3.Distance( transform.position, cam.transform.position );

			Vector2 scale;
			float alpha;
			int state = CamState( camDistance );
			switch( state )
			{
			case -2:
				alpha = 0.0f;
				scale = minScale;
				break;
			case -1: //Fade in
				alpha = VectorExtras.ReverseLerp( camDistance, fadeDistances.x, fadeDistances.y );
				scale = Vector2.Lerp( minScale, maxScale, VectorExtras.ReverseLerp( camDistance, fadeDistances.x, fadeDistances.z ));
				break;
			case 0:
				alpha = 1.0f;
				scale = Vector2.Lerp( minScale, maxScale, VectorExtras.ReverseLerp( camDistance, fadeDistances.x, fadeDistances.z ));
				break;
			case 1: //Fade out
				alpha = VectorExtras.ReverseLerp( camDistance, fadeDistances.z, fadeDistances.w );
				alpha = Mathf.Lerp( 1.0f, 0.0f, alpha ); //This "flips" the value. (0 --> 1, 1 --> 0; 0.2 --> 0.8, 0.8 --> 0.2)
				scale = maxScale;
				break;
			case 2:
				alpha = 0.0f;
				scale = maxScale;
				break;
			default:
				alpha = 0.5f;
				scale = Vector2.one;
				Debug.LogError("Wtf? State: " + state, this);
				return;
			}

			//Set fade
			foreach( Renderer r in gameObject.GetComponentsInChildren< Renderer >() )
			{
				Color tempColor = r.material.color;
				tempColor.a = alpha;
				r.material.color = tempColor;
			}
			//Set height

			//Set rotation
			transform.LookAt( cam.transform );

			//Set scale
			transform.localScale = new Vector3( scale.x, scale.y, 1.0f );
		}
		int CamState( float distance )
		{
			if( distance <= fadeDistances.x ) //Cam cannot see display at all - too close
				return -2;
			else if( distance <= fadeDistances.y ) //Cam can partially see display but is still really close
				return -1;
			else if( distance > fadeDistances.y && distance < fadeDistances.z ) //Cam is in the range where it can fully see display.
				return 0;
			else if( distance <= fadeDistances.w ) //Cam is starting to get too far away and display is fading.
				return 1;
			else //Cam cannot see display at all - too far
				return 2;
		}

		#region gizmos
		void OnDrawGizmos()
		{
			//Visualization of the fade distances (for debug)
			/*
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere( transform.position, fadeDistances.y );
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere( transform.position, fadeDistances.z );
			Gizmos.color = Color.blue;
			Gizmos.DrawRay( new Ray( transform.position, transform.forward ) ); 
			 */
		}
		#endregion

































	}
}