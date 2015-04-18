using UnityEngine;
using System.Collections;

[ExecuteInEditMode] [RequireComponent(typeof(ModularWall))]
public class ModularWallEditor : MonoBehaviour 
{
	ModularWall walls;

	public Transform[] movers;

	// Use this for initialization
	void Start () 
	{
#if UNITY_EDITOR
		walls = GetComponent<ModularWall>();
		Sync();
		
#else
		Destroy( this );
#endif
	}

	void Sync()
	{
		if( walls.wallpoints == null || walls.wallpoints.Length < 2 )
		{
			walls.wallpoints = new Vector3[2];
			walls.wallpoints[0] = Vector3.zero; walls.wallpoints[1] = Vector3.one;
		}
		
		movers = new Transform[0];
		int id = 0;
		foreach( Vector3 point in walls.wallpoints )
		{
			GameObject newMover = GameObject.CreatePrimitive(PrimitiveType.Cube);
			newMover.transform.position = point;
			newMover.transform.parent = this.transform;
			newMover.name = "Wall Corner "+ id;
			movers = ArrayTools.InsertAt<Transform>( movers, newMover.transform, id );
			id++;
		}

	}

	void Update ()
	{
		if( walls.wallpoints.Length != movers.Length )
		{
			for(int i = 0; i < movers.Length; i++) //Remove old gameobjects
			{
				DestroyImmediate( movers[i].gameObject );
			}
			Sync();
		}
		else
		{
			for(int i = 0; i < movers.Length; i++)
			{
				walls.wallpoints[i] = movers[i].position;
			}
		}
	}
	void OnDestroy()
	{
		if( this.enabled == true )
			Debug.LogError("Warning: Delete excess children in the gameobject or disable the attached editor script if you aren't using it!", this);
	}















}
