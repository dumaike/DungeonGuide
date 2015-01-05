using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	[ExecuteInEditMode]
	public class CreateTriangleScript : MonoBehaviour
	{
		#region initializers
		private void Awake()
		{
			MeshCollider meshCollider = this.gameObject.GetComponent<MeshCollider>();			
			MeshFilter meshFilter = this.gameObject.GetComponent<MeshFilter>();	
			MeshRenderer meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
			if (meshCollider == null || meshFilter == null || meshRenderer == null)
			{
				Log.Error("Could not create a triangle because mesh filter and collider don't exist");
				return;
			}
					
        	Mesh mesh = meshFilter.mesh;
			mesh.Clear();
			float sideLen = 0.5f;
			mesh.vertices = new Vector3[] {new Vector3(-sideLen, -sideLen, 0), new Vector3(sideLen, -sideLen, 0), new Vector3(sideLen, sideLen, 0)};
			mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
			mesh.triangles = new int[] {0, 1, 2};			
		}
		#endregion

		#region public methods

		#endregion

		#region private methods

		#endregion
	}

}

