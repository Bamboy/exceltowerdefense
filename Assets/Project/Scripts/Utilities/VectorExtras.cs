using UnityEngine;
using System.Collections;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 4/2/15      ////
///////////////////////////////////

//This script just holds general functions we might want to use in our other scripts.
//Generally they have to do with vector manipulation.

public class VectorExtras : System.Object 
{
	//Tests 'val'. If at or below 'min', return 0. If at or above 'max', return 1.
	//Otherwise returns the percentage between min and max. Useful for determining progress.
	public static float ReverseLerp( float val, float min, float max )
	{
		if( val <= min ) { return 0.0f; }
		else if( val >= max ) { return 1.0f; }
		else
		{
			float offset;
			float offsetMax;
			float offsetVal;
			if( min == 0.0f )
			{
				offset = 0.0f;
			}
			else
			{
				offset = -min;
			}
			offsetMax = offset + max;
			offsetVal = offset + val;
			return offsetVal / offsetMax;
		}
	}
	//Returns true or false with a 50% chance.
	public static bool SplitChance()
	{
		return Random.Range(0, 2) == 0 ? true : false;
	}

	//A helper function for getting the speed multiplier needed to make 'defaultLength' equal 'duration'.
	//Mainly intended for animations as you can only change the playback speed by using multipliers and not the time in seconds.
	public static float GetDurationMultiplier( float defaultLength, float duration )
	{
		return defaultLength / duration;
	}

	//Rounds 'val' to the nearest step of 'snapValue'. Example: RoundTo( 1.2, 0.25 ) would return 1.25.
	public static float RoundTo( float val, float snapValue )
	{
		snapValue = Mathf.Abs(snapValue);
		if( snapValue != 0.0f )
			return snapValue * Mathf.Round(( val / snapValue ));
		else
			return 0.0f;
	}
	
	//Switch between types of vectors.
	public static Vector3 V3FromV2( Vector2 V2, float val )
	{
		return new Vector3( V2.x, V2.y, val );
	}
	public static Vector2 V2FromV3( Vector3 V3 )
	{
		return new Vector2( V3.x, V3.y );
	}
	//Mathf.Sign on Vectors
	public static Vector2 SignV2( Vector2 v2 )
	{
		return new Vector2( Mathf.Sign( v2.x ), Mathf.Sign(v2.y) );
	}
	public static Vector3 SignV3( Vector3 v3 )
	{
		return new Vector3( Mathf.Sign( v3.x ), Mathf.Sign(v3.y), Mathf.Sign(v3.z) );
	}
	public static Vector2 CreateVector2( float v )
	{
		return new Vector2( v, v );
	}
	public static Vector2 CreateVector3( float v )
	{
		return new Vector3( v, v, v );
	}
	
	//===========================================
	//=========== Directions ====================
	//===========================================
	
	// Returns the direction from point 'origin' to point 'target', with values between 0 and 1.
	public static Vector3 Direction( Vector3 origin, Vector3 target )
	{
		return ( target - origin ).normalized;
	}
	// Same as above except with 2D vectors.
	public static Vector2 Direction( Vector2 origin, Vector2 target )
	{
		return ( target - origin ).normalized;
	}
	
	//===========================================
	//=========== Position Manipulation =========
	//===========================================
	
	// Move point 'origin' in the direction of point 'target' by 'distance'. Useful for all sorts of things.
	public static Vector3 OffsetPosInPointDirection( Vector3 origin, Vector3 target, float distance )
	{
		return origin + (Direction( origin, target ) * distance);
	}
	// Move point 'origin' in the direction of point 'target' by 'distance'. Useful for all sorts of things.
	public static Vector2 OffsetPosInPointDirection( Vector2 origin, Vector2 target, float distance )
	{
		return origin + (Direction( origin, target ) * distance);
	}
	
	// Move point 'origin' in 'direction' by 'distance'. Return the new position.
	public static Vector3 OffsetPosInDirection( Vector3 origin, Vector3 direction, float distance )
	{
		return origin + (direction * distance);
	}
	// Move point 'origin' in 'direction' by 'distance'. Return the new position.
	public static Vector2 OffsetPosInDirection( Vector2 origin, Vector2 direction, float distance )
	{
		return origin + (direction * distance);
	}
	
	// Tries to move the point 'origin' to 'target' while staying within 'maxDistance' of 'origin'.
	// Returns the closest point to 'target' while staying within 'maxDistance' of 'origin'.
	public static Vector2 AnchoredMovePosTowardTarget( Vector2 origin, Vector2 target, float maxDistance )
	{
		float distanceToTarget = Vector2.Distance( origin, target );
		if( distanceToTarget > maxDistance )
			return OffsetPosInPointDirection( origin, target, maxDistance );
		else
			return target;
	}
	public static Vector3 AnchoredMovePosTowardTarget( Vector3 origin, Vector3 target, float maxDistance )
	{
		float distanceToTarget = Vector3.Distance( origin, target );
		if( distanceToTarget > maxDistance )
			return OffsetPosInPointDirection( origin, target, maxDistance );
		else
			return target;
	}
	
	// Same as above but also has a minimum value.
	public static Vector2 MinAnchoredMovePosTowardTarget( Vector2 origin, Vector2 target, float minDistance, float maxDistance )
	{
		float distanceToTarget = Vector2.Distance( origin, target );
		if( distanceToTarget > maxDistance )
			return OffsetPosInPointDirection( origin, target, maxDistance );
		else if( distanceToTarget < minDistance )
			return origin;
		else
			return target;
	}
	public static Vector3 MinAnchoredMovePosTowardTarget( Vector3 origin, Vector3 target, float minDistance, float maxDistance )
	{
		float distanceToTarget = Vector3.Distance( origin, target );
		if( distanceToTarget > maxDistance )
			return OffsetPosInPointDirection( origin, target, maxDistance );
		else if( distanceToTarget < minDistance )
			return origin;
		else
			return target;
	}
	
	//===========================================
	//=========== Randomizing Directions ========
	//===========================================
	
	// Randomize a direction starting at point 'origin', traveling in 'direction', by a scale of 'radius'. (Radius should never exceed 10.0f)
	// Return the new directional vector. This is useful for simulating bullet spread.
	// Basically it generates a random point inside a circle or sphere that is located in front of origin. (In the direction)
	// The returned direction is the direction from origin to the randomized point. Smaller values of radius will result in smaller spreads.
	public static Vector2 DirectionalCone( Vector2 origin, Vector2 direction, float radius )
	{
		Vector2 circlePoint = OffsetPosInDirection( origin, direction.normalized, 10.0f ) + (Random.insideUnitCircle * Mathf.Clamp(radius, 0.0f, 10.0f));
		
		// Comment or Uncomment these three lines for debug drawing. (Cyan is the actual bullet travel, yellow is where you were aiming)
		//Vector2 circleCenter = OffsetPosInDirection( origin, direction, 10.0f ); 
		//Debug.DrawLine( V3FromV2( origin, 0.0f ), V3FromV2( circleCenter, 0.0f ), Color.yellow, 1.0f );
		//Debug.DrawLine( V3FromV2( origin, 0.0f ), V3FromV2( circlePoint, 0.0f ), Color.cyan, 3.0f );
		
		return Direction( origin, circlePoint );
	}
	public static Vector3 DirectionalCone( Vector3 origin, Vector3 direction, float radius )
	{
		// ORIGIN IS A WORLD POS, DIRECTION IS A DIRECTIONAL VECTOR
		Vector3 spherePoint = OffsetPosInDirection( origin, direction.normalized, 10.0f ) + (Random.insideUnitSphere * Mathf.Clamp(radius, 0.0f, 10.0f));
		
		// Comment or Uncomment these three lines for debug drawing. (Cyan is the actual bullet travel, yellow is where you were aiming)
		//Vector3 sphereCenter = OffsetPosInDirection( origin, direction, 10.0f ); 
		//Debug.DrawLine( origin, sphereCenter, Color.yellow, 1.0f );
		//Debug.DrawLine( origin, spherePoint, Color.cyan, 3.0f );
		
		return Direction( origin, spherePoint );
	}
	
	// Same idea as above but it uses two world points instead.
	public static Vector2 DirectionalConeTowardPoint( Vector2 origin, Vector2 target, float radius )
	{
		// THIS TAKES TWO WORLD POSITIONS
		Vector2 circlePoint = OffsetPosInPointDirection(origin, target, 10.0f) + (Random.insideUnitCircle * Mathf.Clamp(radius, 0.0f, 10.0f));
		return Direction( origin, circlePoint );
	}
	public static Vector3 DirectionalConeTowardPoint( Vector3 origin, Vector3 target, float radius )
	{
		// THIS TAKES TWO WORLD POSITIONS
		Vector3 spherePoint = OffsetPosInPointDirection(origin, target, 10.0f) + (Random.insideUnitSphere * Mathf.Clamp(radius, 0.0f, 10.0f));
		return Direction( origin, spherePoint );
	}

	//===========================================
	//=========== Size of Object ================
	//===========================================
	//Credit to: http://answers.unity3d.com/questions/475641/gameobject-scale-as-world-coordinates-units.html
	//Gets the scale of the object in real world units
	/*
	public static Vector3 GetWorldScale(GameObject obj) 
	{
		Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
		if( mesh == null )
		{
			return new Vector3();
		}

		Vector3 min = Vector3.one * Mathf.Infinity;
		Vector3 max = Vector3.one * Mathf.NegativeInfinity;
		
		foreach(Vector3 vert in mesh.vertices)
		{
			min = Vector3.Min(vert);
			max = Vector3.Max(vert);
		}
		// the size is max-min multiplied by the object scale:
		return Vector3.Scale(max - min, obj.transform.localScale);
	} */


	//===========================================
	//=========== Degrees And Vectors ===========
	//===========================================
	
	public static Vector2 DegreesToVector( float angle )
	{
		return new Vector2(Mathf.Cos(angle) * Mathf.Rad2Deg, Mathf.Sin(angle) * Mathf.Rad2Deg);
	}
	public static float VectorToDegrees( Vector2 vector )
	{
		return Mathf.Atan2( vector.y, vector.x ) * Mathf.Rad2Deg;
	}

	//================================================
	//=========== Array Utilities ====================
	//================================================

	public static bool Contains<T>( object[] array, object val )
	{
		for(int i = 0; i < array.Length; i++)
		{
			if(val == array[i])
				return true;
		}
		return false;
	}
	
	//===========================================
	//=========== Mouse Info ====================
	//===========================================
	
	//Returns the mouse's position in the world
	/*public static Vector2 GetMouseWorldPos()
	{
		RaycastHit data;
		if(Physics.Raycast( Camera.main.ScreenPointToRay(Input.mousePosition), data, Mathf.Infinity )) 
		{
			return new Vector2( data.point.x, data.point.y );
		}
		else
		{
			return Vector2.zero;
		}
	}*/
	//Returns the mouse's movement speed relative to the screen. (Get magnitude for float speed)
	public static Vector2 GetMouseSpeedRaw()
	{
		return new Vector2( Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y") );
	}


	//===========================================
	//=========== Geometrics ====================
	//===========================================
	
	//Function taken from: http://wiki.unity3d.com/index.php/3d_Math_functions
	//Find the line of intersection between two planes.	The planes are defined by a normal and a point on that plane.
	//The outputs are a point on the line and a vector which indicates it's direction. If the planes are not parallel, 
	//the function outputs true, otherwise false.
	public static bool PlanePlaneIntersection(out Vector3 linePoint, out Vector3 lineVec, Vector3 plane1Normal, Vector3 plane1Position, Vector3 plane2Normal, Vector3 plane2Position){
		
		linePoint = Vector3.zero;
		lineVec = Vector3.zero;
		
		//We can get the direction of the line of intersection of the two planes by calculating the 
		//cross product of the normals of the two planes. Note that this is just a direction and the line
		//is not fixed in space yet. We need a point for that to go with the line vector.
		lineVec = Vector3.Cross(plane1Normal, plane2Normal);
		
		//Next is to calculate a point on the line to fix it's position in space. This is done by finding a vector from
		//the plane2 location, moving parallel to it's plane, and intersecting plane1. To prevent rounding
		//errors, this vector also has to be perpendicular to lineDirection. To get this vector, calculate
		//the cross product of the normal of plane2 and the lineDirection.		
		Vector3 ldir = Vector3.Cross(plane2Normal, lineVec);		
		
		float denominator = Vector3.Dot(plane1Normal, ldir);
		
		//Prevent divide by zero and rounding errors by requiring about 5 degrees angle between the planes.
		if(Mathf.Abs(denominator) > 0.006f){
			
			Vector3 plane1ToPlane2 = plane1Position - plane2Position;
			float t = Vector3.Dot(plane1Normal, plane1ToPlane2) / denominator;
			linePoint = plane2Position + t * ldir;
			
			return true;
		}
		
		//output not valid
		else{
			return false;
		}
	}
	
}
