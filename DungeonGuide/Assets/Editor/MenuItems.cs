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
		}
		#endregion

		#region private methods

		#endregion
	}

}

