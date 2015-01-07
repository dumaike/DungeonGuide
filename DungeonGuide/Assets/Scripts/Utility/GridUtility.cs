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
				    snappedZ != go.transform.position.z)
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
		public static void ReparentToPath(GameObject go, string path)
		{
			//TODO reparent an object to a path with a specific name convention
		}
		#endregion
	}

}

