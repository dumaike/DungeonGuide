using UnityEngine;
using System.Collections;

public class GameObjectUtility
{
	#region public methods
	public static void SetLayerRecursive(int layerIndex, Transform root)
	{
		root.gameObject.layer = layerIndex;
		for (int iChildIndex = 0; iChildIndex < root.transform.childCount; ++iChildIndex)
		{
			SetLayerRecursive(layerIndex, root.transform.GetChild(iChildIndex));
		}
	}
	#endregion

	#region private methods

	#endregion
}

