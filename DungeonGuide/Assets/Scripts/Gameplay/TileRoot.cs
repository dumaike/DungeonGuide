using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace DungeonGuide
{
	public class TileRoot : MonoBehaviour
	{
		public MeshRenderer[] meshRenderers {get; private set;}

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

