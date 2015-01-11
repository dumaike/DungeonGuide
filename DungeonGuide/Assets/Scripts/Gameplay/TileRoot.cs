using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace DungeonGuide
{
	public class TileRoot : MonoBehaviour
	{
		public MeshRenderer[] allMeshRenderers {get; private set;}
	
		private List<MeshRenderer> looseMeshRenderers;
		
		private InteractiveRoot[] interactiveComponents;
		
		private bool tileShown = true;

		#region initializers
		private void Awake()
		{
			this.allMeshRenderers = this.GetComponentsInChildren<MeshRenderer> (true);
			this.interactiveComponents = this.GetComponentsInChildren<InteractiveRoot> ();
			
			//If a mesh is handlered by an interactive toggle, don't include it in the
			//loose mesh renderers
			this.looseMeshRenderers = new List<MeshRenderer>();
			foreach (MeshRenderer mesh in allMeshRenderers)
			{
				bool foundMeshInInteractive = false;
				foreach (InteractiveRoot interactiveObject in this.interactiveComponents)
				{
					if (interactiveObject.ContainsMesh(mesh))
					{
						foundMeshInInteractive = true;
						break;
					}
				}
				
				if (!foundMeshInInteractive)
				{
					this.looseMeshRenderers.Add(mesh);
				}
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
		public void ShowTile(bool visible)
		{
			//Don't bother doing anything if we're already showing/hiding
			if (visible == this.tileShown)
			{
				return;
			}
			
			foreach(MeshRenderer curRenderer in this.looseMeshRenderers)
			{
				curRenderer.enabled = visible;
			}
			
			foreach(InteractiveRoot curInteractive in this.interactiveComponents)
			{
				curInteractive.ShowCurrentlyActiveMeshes(visible);
			}
			
			this.tileShown = visible;
		}
		
		/// <summary>
		/// Used to see if a mesh should could toward being able to see or not see a tileroot.
		/// For example, an open two might consist of two meshes, one for when it's opened, and
		/// one for when it's closed. We don't want to hide the whole door because we can't see
		/// closed door because it's disabled (and doesn't exist in the game at the moment)
		/// </summary>
		/// <returns><c>true</c>, if mesh exist was doesed, <c>false</c> otherwise.</returns>
		/// <param name="mesh">Mesh.</param>
		public bool DoesMeshExist(MeshRenderer mesh)
		{			
			if (this.interactiveComponents.Length == 0)
			{
				return true;
			}
		
			foreach(InteractiveRoot curInteractive in this.interactiveComponents)
			{
				if (curInteractive.DoesMeshExist(mesh))
				{
					return true;
				}
			}
			
			return false;
		}
		#endregion

		#region private methods

		#endregion
	}

}

