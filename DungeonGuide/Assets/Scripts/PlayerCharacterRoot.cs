using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DungeonGuide
{
	public class PlayerCharacterRoot : CharacterRoot
    {
        private TileRoot[] allTiles;
		private List<TileRoot> unseenTiles;

        private MeshRenderer[] meshRenderers;
        
        private const float VISION_DISTANCE = 20.0f;

		private const float HALF_TILE_WIDTH = 0.3f;
		private Vector3[] visionPoints = {
			new Vector3(HALF_TILE_WIDTH, 0.1f, HALF_TILE_WIDTH), 
			new Vector3(-HALF_TILE_WIDTH, 0.1f, HALF_TILE_WIDTH), 
			new Vector3(HALF_TILE_WIDTH, 0.1f, -HALF_TILE_WIDTH), 
			new Vector3(-HALF_TILE_WIDTH, 0.1f, -HALF_TILE_WIDTH) 
		};
        
        private Color selectedColor = new Color(0, 1, 1, 1);
        private Color defaultColor = new Color(1, 1, 1, 1);

		public const string INVISIBLE_LAYER_NAME = "NonSightBlocking";

		#region initializers
		private void Awake()
		{
            this.allTiles = GameObject.FindObjectsOfType<TileRoot>();
            this.meshRenderers = this.GetComponentsInChildren<MeshRenderer> ();

			GameObjectUtility.SetLayerRecursive (LayerMask.NameToLayer (INVISIBLE_LAYER_NAME), this.transform); 
		}

		private void Start()
		{

		}

		private void OnDestroy()
		{

		}
		#endregion

		#region public methods

		#endregion

		#region private methods
        private void Update()
		{
			int layerMask = 1 << 0;

            this.unseenTiles = new List<TileRoot>(this.allTiles);
			foreach (Vector3 localVisionPoint in this.visionPoints) 
			{
				List<TileRoot> unseenLeftovers = new List<TileRoot> ();
				Vector3 visionPoint = this.transform.position + localVisionPoint;

				foreach (TileRoot curTile in this.unseenTiles) 
				{
					Ray raycastRay = new Ray (visionPoint, curTile.transform.position - visionPoint);
					RaycastHit hitInfo = new RaycastHit ();
					Physics.Raycast (raycastRay, out hitInfo, VISION_DISTANCE, layerMask);
					float distanceFromHitToTile = Vector3.Distance (hitInfo.point, curTile.transform.position);
					bool showTile = distanceFromHitToTile < 0.45;
					if (showTile)
					{
						curTile.ShowTile (showTile);        
					}
					else
					{
						unseenLeftovers.Add(curTile);
					}

					Debug.DrawLine (raycastRay.origin, hitInfo.point);
				}

				this.unseenTiles = new List<TileRoot>(unseenLeftovers);
			}

			foreach (TileRoot unseenTile in this.unseenTiles) 
			{
				unseenTile.ShowTile(false);
			}
        }

        public void CharacterSelected(bool selected)
        {
            foreach (MeshRenderer curRenderer in this.meshRenderers)
            {
                curRenderer.material.color = selected ? this.selectedColor : this.defaultColor;
            }
        }
		#endregion
	}

}

