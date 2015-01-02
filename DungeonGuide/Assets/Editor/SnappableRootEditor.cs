using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace DungeonGuide
{
	[CustomEditor(typeof(SnappableRoot))] 
	public class SnappableRootEditor : Editor
	{
		private SnappableRoot snappableObject;
		#region public methods

		#endregion

		#region private methods
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (this.snappableObject == null)
			{
				this.snappableObject = target as SnappableRoot;
			}
			
			if (this.snappableObject.transform.parent == null && this.snappableObject.gameObject.activeInHierarchy)
			{
				GameObject snapRoot = GameObject.Find("Tiles");
				if (snapRoot == null)
				{
					Log.Error("Could not find an object named \"TileRoot\" to place the tile under. Please Create one!");
				}
				else
				{
					this.snappableObject.transform.parent = snapRoot.transform;
				}
			}
			
			GridUtility.SnapToGrid(this.snappableObject.gameObject, this.snappableObject.snapType);
		}
		#endregion
	}

}

