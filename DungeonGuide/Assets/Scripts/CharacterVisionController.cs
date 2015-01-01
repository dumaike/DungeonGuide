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
		
		public const float HALF_TILE_WIDTH = 0.3f;
		private Vector3[] visionPoints = {
			new Vector3(HALF_TILE_WIDTH, 0.1f, HALF_TILE_WIDTH), 
			new Vector3(-HALF_TILE_WIDTH, 0.1f, HALF_TILE_WIDTH), 
			new Vector3(HALF_TILE_WIDTH, 0.1f, -HALF_TILE_WIDTH), 
			new Vector3(-HALF_TILE_WIDTH, 0.1f, -HALF_TILE_WIDTH) 
		};      

		public const bool DRAW_SIGHT_LINES = false;

		public const string INVISIBLE_LAYER_NAME = "NonSightBlocking";

		private List<PlayerCharacterRoot> playerCharacters;

		#region initializers
		public CharacterVisionController()
		{						
			this.allTiles = GameObject.FindObjectsOfType<TileRoot>();
			this.playerCharacters = new List<PlayerCharacterRoot>(GameObject.FindObjectsOfType<PlayerCharacterRoot>());
			foreach (PlayerCharacterRoot player in this.playerCharacters) 
			{
				GameObjectUtility.SetLayerRecursive (LayerMask.NameToLayer (INVISIBLE_LAYER_NAME), player.transform); 
			}
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
							unseenLeftovers.Add (curTile);
						}
						
#if DRAW_SIGHT_LINES
						Debug.DrawLine (raycastRay.origin, hitInfo.point);
#endif
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

