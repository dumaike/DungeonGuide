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
		private List<MeshRenderer> toggledStateMeshes;
		
		[SerializeField]
		private List<MeshRenderer> defaultStateMeshes;
		
		[SerializeField]
		private InteractiveType typeSerialized = InteractiveType.TOGGLEABLE;
		public InteractiveType type {get {return this.typeSerialized;} private set {this.typeSerialized = value;}}
		
		private bool inDefaultState = true;
	
		#region initializers
		#endregion

		#region public methods
		public void TriggerInteraction()
		{
			SceneManager.chVisionCtrl.SetVisionDirty();
			this.inDefaultState = !this.inDefaultState;
			
			foreach(MeshRenderer cur in this.toggledStateMeshes)
			{
				cur.gameObject.SetActive(!this.inDefaultState);
			}
			
			foreach(MeshRenderer cur in this.defaultStateMeshes)
			{
				cur.gameObject.SetActive(this.inDefaultState);
			}
		}
		
		public bool ContainsMesh(MeshRenderer mesh)
		{		
			foreach(MeshRenderer cur in this.toggledStateMeshes)
			{
				if (cur == mesh)
				{
					return true;
				}
			}
			
			foreach(MeshRenderer cur in this.defaultStateMeshes)
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
		
			List<MeshRenderer> currentlyShownMeshes = this.toggledStateMeshes;
			if (this.inDefaultState)
			{
				currentlyShownMeshes = this.defaultStateMeshes;
			}
			
			foreach(MeshRenderer cur in currentlyShownMeshes)
			{
				cur.gameObject.SetActive(visible);
			}
		}
		
		public bool DoesMeshExist(MeshRenderer mesh)
		{
			List<MeshRenderer> currentlyHiddenMeshes = this.defaultStateMeshes;
			if (this.inDefaultState)
			{
				currentlyHiddenMeshes = this.toggledStateMeshes;
			}
			
			return !currentlyHiddenMeshes.Contains(mesh);
		}
		#endregion

		#region private methods

		#endregion
	}

}

