using UnityEngine;
using System.Collections;
using UnityEditor;

namespace DungeonGuide
{
	public class MenuItems
	{
		#region public methods
		[MenuItem("Tools/Align Tiles")]
		public static void AlignTiles()
		{
			SnappableRoot[] allSnappable = GameObject.FindObjectsOfType<SnappableRoot>();
			foreach(SnappableRoot root in allSnappable)
			{
				GridUtility.SnapToGrid(root.gameObject, root.snapType);
			}
			
			TileRoot[] allTiles = GameObject.FindObjectsOfType<TileRoot>();
			foreach(TileRoot root in allTiles)
			{
				GridUtility.SnapToGrid(root.gameObject, SnapType.CENTER);
			}
		}
		#endregion

		#region private methods

		#endregion
	}

}

