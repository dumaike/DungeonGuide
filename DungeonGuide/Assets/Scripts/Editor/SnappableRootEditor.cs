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
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			
			if (this.snappableObject == null)
			{
				this.snappableObject = (SnappableRoot)target;
			}
			
			if (this.snappableObject.transform.parent == null && this.snappableObject.gameObject.activeInHierarchy)
			{
				GridUtility.ReparentToPath(this.snappableObject.gameObject, GridUtility.GAMEPLAY_OBJECT_ROOT_NAME + "/" + this.snappableObject.name);
			}
			
			GridUtility.SnapToGrid(this.snappableObject.gameObject, this.snappableObject.snapType);
		}

		#endregion
	}

}

