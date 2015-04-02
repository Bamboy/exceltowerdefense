using UnityEngine;
using System.Collections;

//Stephan Ennen - 4/2/2015

namespace Excelsion.UI
{
	//Handles the selection of any gameobject you can select.
	public class SelectionController : MonoBehaviour 
	{
		public delegate void OnSelectionChanged( Component newSelection );
		public OnSelectionChanged onSelectionChanged;

		#region Access Instance Anywhere
		private static SelectionController selectC;
		public static SelectionController Get()
		{
			if( selectC != null )
				return selectC;
			else
			{
				GameObject prefab = Resources.LoadAssetAtPath<GameObject>("GUI/Selection");
				GameObject obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.LookRotation(Vector3.up)) as GameObject;
				selectC = obj.GetComponent< SelectionController >();
				return selectC;
			}
		}
		void Awake()
		{
			if( selectC == null )
				selectC = this;
			else
				GameObject.DestroyObject( this.gameObject );
		}
		#endregion


		private static ISelectable sel;
		public static ISelectable Selected
		{
			get{ return sel; }
			set{ sel = value; }
		}
		public static bool HasSelection
		{
			get{ return Selected != null; }
		}

		private Vector3 scaleMultiplier = new Vector3(0.875f, 1f, 1f);
		private MeshRenderer renderComp;
		void Start () 
		{
			renderComp = GetComponent< MeshRenderer >();
			renderComp.enabled = false;
		}
		

		void Update () 
		{
			if( Input.GetMouseButtonDown(0) )
				OnUserClick();

			if( HasSelection == true )
			{
				transform.Rotate(0,0,90 * Time.deltaTime);
				renderComp.enabled = true;
			}
			else
				renderComp.enabled = false;
		}

		void OnUserClick()
		{
			RaycastHit data;
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			if( Physics.Raycast( ray, out data ) )
			{
				Component s = data.transform.GetComponent( typeof(ISelectable) ) as Component;
				if( s == null )
				{
					Deselect();
				}
				else
				{
					Select( s );
				}
			}
			else
				Deselect();
		}
		public void Deselect()
		{
			Selected = null;
			if( onSelectionChanged != null )
				onSelectionChanged( null );
		}
		public void Select( Component obj )
		{
			if( obj is ISelectable )
			{
				Selected = obj as ISelectable;

				if( Selected.SelectionTransform != null )
				{
					transform.position = Selected.SelectionTransform.position;
					transform.localScale = Vector3.Scale(Selected.SelectionTransform.lossyScale, scaleMultiplier);
				}
				else
				{
					Debug.LogWarning("You probably want to specify a SelectionTransform for "+ obj.name +". Using defaults.", obj);
					transform.position = obj.transform.position;
					transform.localScale = scaleMultiplier;
				}
				if( onSelectionChanged != null )
					onSelectionChanged( obj );
			}
			else
				Debug.LogError("Something tried to set a bad selection! ( "+ obj.name +" )", obj);
		}















	}
}