using UnityEngine;
using System.Collections;
using System;

namespace DungeonGuide
{
	public class MoveableRoot : MonoBehaviour
	{						
		protected MeshRenderer[] meshRenderers;

		private Color selectedColor = new Color(0, 1, 1, 1);
		private Color defaultColor = new Color(1, 1, 1, 1);
		
		public Vector3 characterDimensions {get; private set;}
		
		public bool freeMovement = false;

		#region initializers
		virtual protected void Awake()
		{		
			GameObjectUtility.SetLayerRecursive (
				LayerMask.NameToLayer (CharacterVisionController.INVISIBLE_LAYER_NAME), this.transform); 
		
			this.meshRenderers = this.GetComponentsInChildren<MeshRenderer> ();
			
			//HACK Assumes centered single mesh objects
			foreach (MeshRenderer renderer in this.meshRenderers)
			{
				float x = renderer.bounds.extents.x;
				float z = renderer.bounds.extents.z;
				float y = renderer.bounds.extents.y;
				this.characterDimensions = new Vector3((float)Math.Ceiling(x*2), (float)Math.Ceiling(y*2), (float)Math.Ceiling(z*2));
			}
		}

		private void Start()
		{

		}

		private void OnDestroy()
		{

		}
		#endregion

		#region public methods
		public void CharacterSelected(bool selected)
		{
			foreach (MeshRenderer curRenderer in this.meshRenderers)
			{
				curRenderer.material.color = selected ? this.selectedColor : this.defaultColor;
			}
		}
		#endregion

		#region private methods

		#endregion
	}

}

