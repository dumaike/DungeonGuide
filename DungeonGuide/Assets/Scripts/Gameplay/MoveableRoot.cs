using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace DungeonGuide
{
	[RequireComponent(typeof(Animator))]
	public class MoveableRoot : MonoBehaviour
	{						
		protected MeshRenderer[] meshRenderers;

		private Color selectedColor = new Color(0, 1, 1, 1);
		private Color defaultColor = new Color(1, 1, 1, 1);
		
		[SerializeField]
		[Range(1,2)]
		private int characterSizeEditor = 1;
		public int characterSize {get {return this.characterSizeEditor;} private set{this.characterSizeEditor = value;}}
		
		[SerializeField]
		private bool hasVisionEditor = true;
		public bool hasVision {get {return this.hasVisionEditor;} private set{this.hasVisionEditor = value;}}
		
		public bool freeMovement = false;

		#region initializers
		virtual protected void Start()
		{		
			GameObjectUtility.SetLayerRecursive (LayerAccessor.BLOCKS_NOTHING, this.transform); 
		
			this.meshRenderers = this.GetComponentsInChildren<MeshRenderer> ();
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
		
		public void TintCharacter(Color color)
		{
			this.defaultColor = color;
			foreach (MeshRenderer curRenderer in this.meshRenderers)
			{
				curRenderer.material.color = this.defaultColor;
			}
		}
		#endregion

		#region private methods

		#endregion
	}

}

