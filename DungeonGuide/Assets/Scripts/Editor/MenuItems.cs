using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGuide
{
	public class MenuItems
	{
		#region public methods
		[MenuItem("Tools/Align Tiles %3")]
		public static void AlignTiles()
		{
			SnappableRoot[] allSnappable = GameObject.FindObjectsOfType<SnappableRoot>();
			Log.Print("Aligning " + allSnappable.Length + " snappable tiles.", LogChannel.DEBUG);
			foreach(SnappableRoot root in allSnappable)
			{
				GridUtility.SnapToGrid(root.gameObject, root.snapType);
			}
		}
		
		[MenuItem("Tools/Delete Duplicates")]
		public static void DeleteDuplicates()
		{
			List<SnappableRoot> allSnappable = GameObject.FindObjectsOfType<SnappableRoot>().ToList();
			List<SnappableRoot> dupsToDelete = new List<SnappableRoot>();
			List<SnappableRoot> uniquesToKeep = new List<SnappableRoot>();
			
			foreach(SnappableRoot root in allSnappable)
			{
				SnappableRoot foundInUniques = uniquesToKeep.Find(found => 
				                                                  found.name == root.name && 
				                                                  found.transform.position == root.transform.position);
				
				if (foundInUniques == null)
				{
					uniquesToKeep.Add(root);
				}
				else
				{
					dupsToDelete.Add(root);
				}
			}
			
			Log.Print("Found " + allSnappable.Count + " tiles. Deleted " + dupsToDelete.Count + " duplicates.", LogChannel.DEBUG);
			
			foreach(SnappableRoot root in dupsToDelete)
			{
				GameObject.DestroyImmediate(root.gameObject);
			}
		}
		
		[MenuItem("Tools/Organize Tiles")]
		public static void OrgTiles()
		{
			SnappableRoot[] allSnappable = GameObject.FindObjectsOfType<SnappableRoot>();
			foreach(SnappableRoot root in allSnappable)
			{
				GridUtility.ReparentToPath(root.gameObject, GridUtility.GAMEPLAY_OBJECT_ROOT_NAME + "/" + root.name);
			}
		}
		#endregion

		#region private methods
		#endregion
	}

}