using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DungeonGuide
{
	public class TileRoot : MonoBehaviour
	{
		private MeshRenderer[] meshRenderers;

		#region initializers
		private void Awake()
		{
			this.meshRenderers = this.GetComponentsInChildren<MeshRenderer> ();
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
			foreach(MeshRenderer curRenderer in this.meshRenderers)
			{
				curRenderer.enabled = visible;
			}
		}
		#endregion

		#region private methods

		#endregion
	}

}

