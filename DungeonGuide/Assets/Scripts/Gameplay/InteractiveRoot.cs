using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DungeonGuide
{
	public class InteractiveRoot : MonoBehaviour
	{
		public enum InteractiveType
		{
			REVEALABLE,
			TOGGLEABLE
		}
	
		[SerializeField]
		private List<GameObject> toggledGameObject;
		
		[SerializeField]
		private List<GameObject> defaultGameObjects;
		
		[SerializeField]
		private InteractiveType typeSerialized = InteractiveType.TOGGLEABLE;
		public InteractiveType type {get {return this.typeSerialized;} private set {this.typeSerialized = value;}}
		
		private bool inDefaultState = true;
	
		#region initializers
		#endregion

		#region public methods
		public void TriggerInteraction()
		{
			this.inDefaultState = !this.inDefaultState;
			
			foreach(GameObject cur in this.toggledGameObject)
			{
				cur.gameObject.SetActive(!this.inDefaultState);
			}
			
			foreach(GameObject cur in this.defaultGameObjects)
			{
				cur.gameObject.SetActive(this.inDefaultState);
			}
			
			SceneManager.eventCtr.FireInteractiveObjectToggledEvent();
		}
		
		public bool ContainsMesh(MeshRenderer mesh)
		{		
			foreach(GameObject cur in this.toggledGameObject)
			{
				if (cur == mesh)
				{
					return true;
				}
			}
			
			foreach(GameObject cur in this.defaultGameObjects)
			{
				if (cur == mesh)
				{
					return true;
				}
			}
			
			return false;
		}
		
		public void ShowCurrentlyActiveMeshes(bool visible)
		{						
			//TODO Make this more effecient by only checking when
			//we change visibily states
		
			List<GameObject> currentlyShownMeshes = this.toggledGameObject;
			if (this.inDefaultState)
			{
				currentlyShownMeshes = this.defaultGameObjects;
			}
			
			foreach(GameObject cur in currentlyShownMeshes)
			{
				cur.gameObject.SetActive(visible);
			}
		}
		
		public bool DoesMeshExist(GameObject mesh)
		{
			List<GameObject> currentlyHiddenMeshes = this.defaultGameObjects;
			if (this.inDefaultState)
			{
				currentlyHiddenMeshes = this.toggledGameObject;
			}
			
			return !currentlyHiddenMeshes.Contains(mesh);
		}
		#endregion

		#region private methods

		#endregion
	}

}

