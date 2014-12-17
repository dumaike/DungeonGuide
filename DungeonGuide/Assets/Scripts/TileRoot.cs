using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace DungeonGuide
{
	public class TileRoot : MonoBehaviour
	{
		private MeshRenderer[] meshRenderers;

		public int test;

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

		public IEnumerator SnapNextFrame()
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();

			Debug.Log ("Not called?");

			Vector3 snappedPosition = this.transform.position;
			snappedPosition.x = (float)Math.Round (snappedPosition.x);
			snappedPosition.z = (float)Math.Round (snappedPosition.z);
			snappedPosition.y = 0;
        	this.transform.position = snappedPosition;
		}
		#endregion

		#region private methods

		#endregion
	}

}

