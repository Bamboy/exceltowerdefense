using UnityEngine;
using System.Collections;

//Stephan Ennen - 4/2/2015

namespace Excelsion.UI
{
	//Represents something that can be selected and information displayed about it.
	public interface ISelectable
	{
		void OnDrawSelectedUI();
		//Add a bool for if UI should be done in world UI or screen UI system?

		//Uses position and scale to place our selection indicator. This should be an empty transform.
		Transform SelectionTransform{ get; }
	}
}