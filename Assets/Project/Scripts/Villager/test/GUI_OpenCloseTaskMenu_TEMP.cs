using UnityEngine;
using System.Collections;

// TODO something like MenuController should do it
public class GUI_OpenCloseTaskMenu_TEMP : MonoBehaviour
{
	private bool isShown = false;

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			if (isShown)
			{
				Destroy(TaskMenu.Get().gameObject);
				isShown = !isShown;
			}
			else
			{
				TaskMenu.Get();
				isShown = !isShown;
			}
		}
	}
}
