using UnityEngine;
using System.Collections;
// MyScriptEditor.cs
using UnityEditor;
using System;

namespace DungeonGuide
{
	[CustomEditor(typeof(TileRoot))] 
	public class TileRootEditor : Editor
	{
		private TileRoot tile;
		private Vector3 oldPosition;
		#region public methods

		#endregion

		#region private methods
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			if (this.tile == null)
			{
				this.tile = target as TileRoot;
			}

			if (this.tile.transform.parent == null && this.tile.gameObject.activeInHierarchy)
			{
				GameObject tileRoot = GameObject.Find("Tiles");
				if (tileRoot == null)
				{
					Debug.LogError("Could not find an object named \"TileRoot\" to place the tile under. Please Create one!");
				}
				else
				{
					this.tile.transform.parent = tileRoot.transform;
				}
			}

			Vector3 snappedPosition = this.tile.transform.position;
			snappedPosition.x = (float)Math.Round (snappedPosition.x);
			snappedPosition.z = (float)Math.Round (snappedPosition.z);
			snappedPosition.y = 0;
			this.tile.transform.position = snappedPosition;
		}
		#endregion
	}

}

