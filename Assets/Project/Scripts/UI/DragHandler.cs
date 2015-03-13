using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DragHandler : MonoBehaviour {

	public Image DragIcon;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void test()
	{
		Debug.Log ("X: " + Input.mousePosition.x + " Y: " + Input.mousePosition.y);
		DragIcon.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
	}
}
