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
			
			Log.Print("Running OnInspectorGUI for tile: " + this.tile.name);
			
			int snappedX = (int)this.tile.transform.position.x;
			int snappedY = (int)this.tile.transform.position.y;
			if (snappedX != this.tile.transform.position.x ||
			    snappedY != this.tile.transform.position.y)
			{
				Log.Print("Snapping Tile: " + this.tile.name);

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

					Vector3 snappedPosition = this.tile.transform.position;
					snappedPosition.x = (float)Math.Round (snappedPosition.x);
					snappedPosition.z = (float)Math.Round (snappedPosition.z);
					snappedPosition.y = 0;
					this.tile.transform.position = snappedPosition;
			}
		}
		#endregion
	}

}

