using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DungeonGuide
{
	public class CharacterVisionController
	{		
		private TileRoot[] allTiles;
		private List<TileRoot> unseenTiles;
		
		private const float VISION_DISTANCE = 20.0f;
		
		public const float HALF_TILE_WIDTH = 0.40f;
		private Vector3[] visionPoints = {
			new Vector3(HALF_TILE_WIDTH, 0.1f, HALF_TILE_WIDTH), 
			new Vector3(-HALF_TILE_WIDTH, 0.1f, HALF_TILE_WIDTH), 
			new Vector3(HALF_TILE_WIDTH, 0.1f, -HALF_TILE_WIDTH), 
			new Vector3(-HALF_TILE_WIDTH, 0.1f, -HALF_TILE_WIDTH) 
		};      

		public const bool DRAW_SIGHT_LINES = true;

		public const string INVISIBLE_LAYER_NAME = "NonSightBlocking";

		private List<PlayerCharacterRoot> playerCharacters;

		#region initializers
		public CharacterVisionController()
		{						
			this.allTiles = GameObject.FindObjectsOfType<TileRoot>();
			this.playerCharacters = new List<PlayerCharacterRoot>(GameObject.FindObjectsOfType<PlayerCharacterRoot>());
		}
		#endregion

		#region public methods
		public void RemoveCharacterFromVision(PlayerCharacterRoot character)
		{
			this.playerCharacters.Remove (character);
		}
		
		public void AddCharacterToVision(PlayerCharacterRoot character)
		{
			this.playerCharacters.Add (character);
		}
		
		public void Update()
		{
			int layerMask = 1 << 0;
			
			this.unseenTiles = new List<TileRoot> (this.allTiles);
			
			foreach (PlayerCharacterRoot player in this.playerCharacters) 
			{
				foreach (Vector3 localVisionPoint in this.visionPoints) 
				{
					List<TileRoot> unseenLeftovers = new List<TileRoot> ();
					Vector3 visionPoint = player.transform.position + localVisionPoint;
					
					foreach (TileRoot curTile in this.unseenTiles) 
					{
						foreach (MeshRenderer meshRenderer in curTile.meshRenderers)
						{
							//Cast a ray toward the tile
							Ray raycastRay = new Ray (visionPoint, meshRenderer.transform.position - visionPoint);
							RaycastHit hitInfo = new RaycastHit ();
							Physics.Raycast (raycastRay, out hitInfo, VISION_DISTANCE, layerMask);
						
							//If it's close to the distance to the tile, we can see the tile
							bool showTile = hitInfo.transform == meshRenderer.transform;
							if (showTile) 
							{
								curTile.ShowTile (showTile);        
							} 
							else 
							{
								unseenLeftovers.Add (curTile);
							}
							
#if DRAW_SIGHT_LINES
							Debug.DrawLine (raycastRay.origin, hitInfo.point);
#endif
						}
					}
					
					this.unseenTiles = new List<TileRoot> (unseenLeftovers);
				}
			}
			
			foreach (TileRoot unseenTile in this.unseenTiles) 
			{
				unseenTile.ShowTile (false);
			}
		}
		#endregion

		#region private methods
		
		#endregion
	}

}

