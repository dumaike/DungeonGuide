using UnityEngine;
using System;

namespace DungeonGuide
{
	public enum SnapType
	{
		CENTER,
		CORNER,
		NONE
	}
	
	public class GridUtility
	{	
		public const string GAMEPLAY_OBJECT_ROOT_NAME = "GameplayObjects";
	
		#region public methods
		public static void SnapToGrid(GameObject go, SnapType snapType)
		{
			if (snapType == SnapType.CENTER)
			{
				int snappedX = (int)go.transform.position.x;
				int snappedZ = (int)go.transform.position.z;
				if (snappedX != go.transform.position.x
				    || snappedZ != go.transform.position.z
				    //|| go.transform.localPosition.y != 0
					)
				{
					Vector3 snappedGlobalPosition = go.transform.position;
					snappedGlobalPosition.x = (float)Math.Round (snappedGlobalPosition.x);
					snappedGlobalPosition.z = (float)Math.Round (snappedGlobalPosition.z);
					go.transform.position = snappedGlobalPosition;				
					
					Vector3 snappedLocalPosition = go.transform.localPosition;
					//snappedLocalPosition.y = 0;
					go.transform.localPosition = snappedLocalPosition;				
				}
			}
		}
		#endregion
		
		#region private methods		
		/// <summary>
		/// Reparents a game object to a specific game object path, if the path
		/// doesn't exist, it creates the path
		/// </summary>
		/// <param name="go">The object to reparent</param>
		/// <param name="path">The string path representing the where we should reparent. For example: "GameplayObjects/Tiles"</param>
		public static bool ReparentToPath(GameObject go, string path)
		{
			return ReparentToPathInternal(go, path, "");
		}
		
		private static bool ReparentToPathInternal(GameObject go, string childPath, string parentPath)
		{
			int indexOfSlash = childPath.IndexOf("/");			
			if (indexOfSlash == -1)
			{
				GameObject parent = GameObject.Find(parentPath + "/" + childPath);
				if (parent == null)
				{
					parent = new GameObject(childPath);
					GameObject grandparent = GameObject.Find(parentPath);
					if (grandparent == null)
					{
						Log.Error("Some sort of logic error has happened in ReparentToPathInternal. We got to a child without creating it's parents", LogChannel.DEBUG);
						return false;
					}
					parent.transform.parent = grandparent.transform;
				}

				//If we're already at that parent
				if (go.transform.parent == parent.transform)
				{
					return false;
				}

				go.transform.parent = parent.transform;
				return true;
			}

			string topLevelObjectName = childPath.Substring(0, indexOfSlash);
			string topLevelObjectPath = parentPath + "/" + topLevelObjectName;
			GameObject topLevelObject = GameObject.Find(topLevelObjectPath);
				
			//If the top level object doesn't exist, make it
			if (topLevelObject == null)
			{
				topLevelObject = new GameObject(topLevelObjectName);
					
				//If the top level object parent doesn't exist, there's a problem
				//(or he's just on the root)
				if (!string.IsNullOrEmpty(parentPath))
				{
					GameObject parentOfTopLevelObject = new GameObject(parentPath);

					topLevelObject.transform.parent = parentOfTopLevelObject.transform;
				}
			}
				
			string newChildPath = childPath.Substring(indexOfSlash + 1);
			string newParentPath = parentPath;
			if (!string.IsNullOrEmpty(newParentPath))
			{
				newParentPath += "/";
			}
			newParentPath += topLevelObjectName;
			return ReparentToPathInternal(go, newChildPath, newParentPath);
		}
		#endregion
	}

}

