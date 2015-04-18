using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class ModularWall : MonoBehaviour 
{
	public GameObject[] wallspikes; //Our prefabs. We will need to get the width

	//Set of points that act as corners. The last entry in the array will draw to the first entry.
	public Vector3[] wallpoints = new Vector3[2]; 
	//TODO - we will want to do a raycast downward so that our walls are created on top of any hills.
	

	void OnDrawGizmos()
	{
		if( wallpoints.Length < 2 )
			return;

		Gizmos.color = Color.red;
		//Draw point to point. The lines created will be where our walls will be.
		for(int i = 0; i < wallpoints.Length; i++)
		{
			if( i == wallpoints.Length - 1 )
				Gizmos.DrawLine( wallpoints[i], wallpoints[0] );
			else
				Gizmos.DrawLine( wallpoints[i], wallpoints[i + 1] );
		}

		Gizmos.color = Color.yellow;
		for(int j = 0; j < wallpoints.Length; j++)
		{
			Gizmos.DrawWireCube( wallpoints[j], Vector3.one );
		}

		Gizmos.color = Color.black;
		for(int k = 0; k < spikePoints.Length; k++)
		{
			Gizmos.DrawWireSphere( spikePoints[k], baseRadius / 2.0f );
		}


	}
	private Vector3[] spikePoints;

	[Range(0.1f, 10.0f)] public float baseRadius = 0.25f;
	public int totalSpawnCount;
	private int spawnCount;
	private float extraDistance;

	private float extraRadius;
	void Start () 
	{
		CalculateSpikes();
		SpawnSpikes();
	}
	

	void CalculateSpikes()
	{
		totalSpawnCount = 0;
		spikePoints = new Vector3[0];
		if( wallspikes.Length > 0 && wallspikes[0] != null )
		{
			//Calculate spawn points for our spikes!
			for(int line = 0; line < wallpoints.Length; line++)
			{
				#region numtospawn
				int toPoint;
				if( line == wallpoints.Length - 1 )
					toPoint = 0;
				else
					toPoint = line + 1;
				
				//We aren't interested in height during this calculation.
				float flatDist = Vector3.Distance(
					new Vector3( wallpoints[line].x, 0f, wallpoints[line].z ),
					new Vector3( wallpoints[toPoint].x, 0f, wallpoints[toPoint].z ) );
				
				if( flatDist < baseRadius )
					return;
				
				spawnCount = Mathf.FloorToInt( flatDist / baseRadius );
				totalSpawnCount += spawnCount;
				Debug.Log("Spawncount: "+ spawnCount);
				
				extraDistance = flatDist % baseRadius;
				extraRadius = extraDistance / spawnCount;
				
				#endregion
				
				
				float progressPerSpike = 1.0f / spawnCount;
				for(int spike = 0; spike < spawnCount; spike++)
				{
					Vector3 spawnPos = Vector3.Lerp(
						new Vector3( wallpoints[line].x, 0f, wallpoints[line].z ),
						new Vector3( wallpoints[toPoint].x, 0f, wallpoints[toPoint].z ),
						(float)spike * progressPerSpike );
					
					spikePoints = ArrayTools.Push<Vector3>( spikePoints, spawnPos );
				}
				
			}
			Debug.Log("Total created: "+ totalSpawnCount);
		}
	}

	void SpawnSpikes()
	{
		//Spawn our spikes!
		foreach( Vector3 point in spikePoints )
		{
			wallspikes = ArrayTools.Shuffle<GameObject>( wallspikes );
			int r = Random.Range( 0, 3 );
			GameObject newSpike = (GameObject)Instantiate( wallspikes[r], point, 
			           wallspikes[r].transform.rotation * Quaternion.Euler(0f,0f,Random.Range(0f, 360f)) );

			newSpike.transform.localScale = new Vector3( 1f, 1f, Random.Range(0.9f, 1.25f) );
			newSpike.transform.parent = this.transform;
		}
	}
























}
