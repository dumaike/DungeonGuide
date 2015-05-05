using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace DungeonGuide
{
	public class MoveableEntity : MonoBehaviour
	{						
		protected MeshRenderer[] meshRenderers;

		private Color selectedColor = new Color(0, 1, 1, 1);
		private Color defaultColor = new Color(1, 1, 1, 1);
		
		private float startingAlpha;
		
		[SerializeField]
		[Range(1,2)]
		private int characterSizeEditor = 1;
		public int characterSize {get {return this.characterSizeEditor;} private set{this.characterSizeEditor = value;}}
		
		[SerializeField]
		private bool hasVisionEditor = false;
		public bool hasVision {get {return this.hasVisionEditor;} private set{this.hasVisionEditor = value;}}
		
		[SerializeField]
		private bool snapsEditor = true;
		public bool snaps {get {return this.snapsEditor;} private set{this.snapsEditor = value;}}
		
		[SerializeField]
		private GameObject stackablePartsEditor = null;
		public GameObject stackableParts {get {return this.stackablePartsEditor;}}
		
		
		public bool freeMovement = false;

		#region initializers
		virtual protected void Start()
		{		
			GameObjectUtility.SetLayerRecursive (LayerAccessor.BLOCKS_NOTHING, this.transform); 
		
			this.meshRenderers = this.GetComponentsInChildren<MeshRenderer> ();
			
			foreach (MeshRenderer curRenderer in this.meshRenderers)
			{
				this.startingAlpha = curRenderer.material.color.a;
			}
			
			this.selectedColor.a = this.startingAlpha;
			this.defaultColor.a = this.startingAlpha;
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
			this.defaultColor.a = this.startingAlpha;
			foreach (MeshRenderer curRenderer in this.meshRenderers)
			{
				curRenderer.material.color = this.defaultColor;
			}
		}
		
		public void SetCharacterText(string value)
		{			
			Text counterText = this.GetComponentInChildren<Text>();
			if (counterText != null)
			{
				counterText.text = value;
			}
		}
		
		public void ShowText(bool show)
		{			
			Text counterText = this.GetComponentInChildren<Text>();
			if (counterText != null)
			{
				counterText.enabled = show;
			}
		}
		#endregion

		#region private methods

		#endregion
	}

}

