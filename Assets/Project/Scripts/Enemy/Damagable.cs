using UnityEngine;
using System.Collections;

namespace Excelsion.Attributes
{
	//Represents health
	public class Damagable : MonoBehaviour 
	{
		private int hpcurrent;
		private int hpdefault;
		public int Health{ get{ return hpcurrent; } }
		public int HealthMax{ get{ return hpdefault; } }
		
		void Start () 
		{
			hpdefault = hpcurrent;
		}

		public void Damage( )
		{

		}

		void Update () 
		{
		
		}
	}
}