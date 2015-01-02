using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace DungeonGuide
{
	[CustomEditor(typeof(TileRoot))] 
	public class TileRootEditor : Editor
	{
		private TileRoot tile;
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
					Log.Error("Could not find an object named \"TileRoot\" to place the tile under. Please Create one!");
				}
				else
				{
					this.tile.transform.parent = tileRoot.transform;
				}
			}
			
			GridUtility.SnapToGrid(this.tile.gameObject, SnapType.CENTER);
		}
		#endregion
	}

}

