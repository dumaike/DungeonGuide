using UnityEngine;
using System.Collections;
using System;

namespace DungeonGuide
{
	public enum SnapType
	{
		CENTER,
		EDGE,
		CORNER
	}
	
	public class GridUtility
	{	
		#region public methods
		public static void SnapToGrid(GameObject go, SnapType snapType)
		{
			if (snapType == SnapType.CENTER)
			{
				int snappedX = (int)go.transform.position.x;
				int snappedZ = (int)go.transform.position.z;
				if (snappedX != go.transform.position.x ||
				    snappedZ != go.transform.position.z ||
				    go.transform.position.y != 0)
				{
					Vector3 snappedPosition = go.transform.position;
					snappedPosition.x = (float)Math.Round (snappedPosition.x);
					snappedPosition.z = (float)Math.Round (snappedPosition.z);
					snappedPosition.y = 0;
					go.transform.position = snappedPosition;				
				}
			}
			if (snapType == SnapType.EDGE)
			{
				//Something is on the edge if one of it's coords is X.5 and the other is X.0.
				//If we double them both, one should be odd and the other even
				float originalX = go.transform.position.x;
				float originalZ = go.transform.position.z;
				float doubledX = originalX*2;
				float doubledZ = originalZ*2;
				float remainderX = doubledX%2;
				float remainderZ = doubledZ%2;
				if ((remainderX == 0 && remainderZ != 0) ||
				    (remainderX != 0 && remainderZ == 0 && ((int)doubledX/2.0f == originalX)))
				{
					return;
				}				
				
				float roundedX = (float)Math.Round(doubledX);
				float roundedZ = (float)Math.Round(doubledZ);
				float errorOnX = (float)Math.Abs(roundedX - doubledX);
				float errorOnZ = (float)Math.Abs(roundedZ - doubledZ);
				bool closerOnX = errorOnX < errorOnZ;
				
				float finalX;
				float finalZ;
				bool xIsEven = roundedX % 2 == 0;
				bool zIsEven = roundedZ % 2 == 0;
				if ((xIsEven && !zIsEven) || (!xIsEven && zIsEven))
				{
					finalX = roundedX / 2.0f;
					finalZ = roundedZ / 2.0f;
				}
				else if (closerOnX)
				{
					finalX = roundedX / 2.0f;
					
					//X is a whole number
					if (xIsEven)
					{
						//Get Z to the closest half number
						finalZ = (float)Math.Round(originalZ+0.5f)-0.5f;
					}
					// X is a half number
					else 
					{							
						//Get Z to the closest whole number
						finalZ = (float)Math.Round(originalZ);
					}
				}
				else
				{
					finalZ = roundedZ / 2.0f;
					
					//Z is a whole number
					if (zIsEven)
					{
						//Get X to the closest half number
						finalX = (float)Math.Round(originalX+0.5f)-0.5f;
					}
					//Z is a half number
					else 
					{							
						//Get X to the closest whole number
						finalX = (float)Math.Round(originalX);
					}
				}
				
				Log.Print("Starting with ( " + originalX + ", " + originalZ + ") -> (" + finalX + ", " + finalZ + ")");
				
				Vector3 snappedPosition = new Vector3(finalX, 0, finalZ);
				go.transform.position = snappedPosition;	
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
		public static void ReparentToPath(GameObject go, string path)
		{
			ReparentToPathInternal(go, path, "");
		}
		
		private static void ReparentToPathInternal(GameObject go, string childPath, string parentPath)
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
						return;
					}
					parent.transform.parent = grandparent.transform;
				}
				go.transform.parent = parent.transform;
			}
			else
			{
				string topLevelObjectName = childPath.Substring(0, indexOfSlash);
				string topLevelObjectPath = parentPath + "/" + topLevelObjectName;
				GameObject topLevelObject = GameObject.Find(topLevelObjectPath);
				
				//If the top level object doesn't exist, make it
				if (topLevelObject == null)
				{
					topLevelObject = new GameObject(topLevelObjectName);
					
					//If the top level object parent doesn't exist, there's a problem
					//(or he's just on the root)
					GameObject parentOfTopLevelObject = null;
					if (!string.IsNullOrEmpty(parentPath))
					{
						parentOfTopLevelObject = new GameObject(parentPath);
						if (parentOfTopLevelObject == null)
						{
							Log.Error("Some sort of logic error has happened in ReparentToPathInternal. We got to a child without creating it's parents", LogChannel.DEBUG);
							return;
						}
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
				ReparentToPathInternal(go, newChildPath, newParentPath);
			}
		}
		#endregion
	}

}

