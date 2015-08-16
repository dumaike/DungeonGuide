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

		/// <summary>
		/// Align all the tiles with the grid that should be aligned with the grid
		/// </summary>
		[MenuItem("Tools/Align Tiles %3")]
		public static void AlignTiles()
		{
			SnappableRoot[] allSnappable = Object.FindObjectsOfType<SnappableRoot>();
			Log.Print("Aligning " + allSnappable.Length + " snappable tiles.", LogChannel.EDITOR_SETUP);
			foreach(SnappableRoot root in allSnappable)
			{
				GridUtility.SnapToGrid(root.gameObject, root.snapType);
			}
		}
		
		/// <summary>
		/// Delete any duplicated tiles in the same position with the same name
		/// </summary>
		[MenuItem("Tools/Delete Duplicates")]
		public static void DeleteDuplicates()
		{
			List<SnappableRoot> allSnappable = Object.FindObjectsOfType<SnappableRoot>().ToList();
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
				Object.DestroyImmediate(root.gameObject);
			}
		}

		/// <summary>
		/// Reparent any tiles into the heiarcy location we'd expect to find them
		/// </summary>
		[MenuItem("Tools/Organize Tiles")]
		public static void OrgTiles()
		{
			SnappableRoot[] allSnappable = Object.FindObjectsOfType<SnappableRoot>();
			int numReparented = 0;

			foreach (SnappableRoot root in allSnappable)
			{
				bool didReparent = 
					GridUtility.ReparentToPath(root.gameObject, GridUtility.GAMEPLAY_OBJECT_ROOT_NAME + "/" + root.name);
				if (didReparent)
				{
					numReparented++;
					Log.Print("Reparenting \"" + root.name + "\"", LogChannel.EDITOR_SETUP, root);
				}
			}

			Log.Print("Found " + numReparented + " object(s) to reparent.");
		}

		/// <summary>
		/// Make sure all objects have their original prefab names
		/// </summary>
		[MenuItem("Tools/Fix Names")]
		public static void FixNames()
		{
			GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
			int renamesFound = 0;

			foreach (GameObject obj in allObjects)
			{
				PrefabType prefabType = PrefabUtility.GetPrefabType(obj);
				if (prefabType != PrefabType.None)
				{
					GameObject root = PrefabUtility.FindPrefabRoot(obj);
					if (root == obj)
					{
						Object prefab = PrefabUtility.GetPrefabParent(obj);
						if (prefab.name != obj.name)
						{
							Log.Print("Changing \"" + obj.name + "\" to \"" + 
								prefab.name + "\".", LogChannel.EDITOR_SETUP, obj);
							obj.name = prefab.name;
							renamesFound++;
						}
					}
				}
			}

			Log.Print("Found " + renamesFound + " object(s) to rename.");
		}
		#endregion

		#region private methods
		#endregion
	}

}