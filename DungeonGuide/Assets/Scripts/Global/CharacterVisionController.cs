using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DungeonGuide
{
	public class CharacterVisionController
	{		
		private TileRoot[] allTiles;
		private List<TileRoot> unseenTiles;
		
		private const float VISION_DISTANCE = 15.0f;
		
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
		
		private bool visionDirty = true;

		#region initializers
		public CharacterVisionController()
		{						
			this.allTiles = GameObject.FindObjectsOfType<TileRoot>();
			Log.Print("There are " + this.allTiles.Length + " tiles in the scene.", LogChannel.EDITOR_SETUP);
			this.playerCharacters = new List<PlayerCharacterRoot>(GameObject.FindObjectsOfType<PlayerCharacterRoot>());
		}
		#endregion

		#region public methods
		public void SetVisionDirty()
		{
			Log.Print("Character vision set to dirty. Recalculating.", LogChannel.CHARACTER_VISION);
		
			this.visionDirty = true;
		}
		
		public void RemoveCharacterFromVision(PlayerCharacterRoot character)
		{
			SetVisionDirty();
			this.playerCharacters.Remove (character);
		}
		
		public void AddCharacterToVision(PlayerCharacterRoot character)
		{
			SetVisionDirty();
			this.playerCharacters.Add (character);
		}
		
		public void Update()
		{
			//If nothing has changed that requires a vision re-calc, don't
			//bother doing anything in the update.
			if (!this.visionDirty || ConsoleCommands.fullVisionMode)
			{
				return;
			}
		
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
						bool canSeeAllMeshesInTile = true;
						foreach (MeshRenderer meshRenderer in curTile.allMeshRenderers)
						{						
							float distanceToMesh = (visionPoint - meshRenderer.transform.position).magnitude;
							if (distanceToMesh > VISION_DISTANCE)
							{
								canSeeAllMeshesInTile = false;
								break;
							}
							
							//If the mesh doesn't even exist in the current game state
							//don't bother raycasting
							if (!curTile.DoesMeshExist(meshRenderer))
							{
								continue;
							}
						
							//Cast a ray toward the tile
							Ray raycastRay = new Ray (visionPoint, meshRenderer.transform.position - visionPoint);
							RaycastHit hitInfo = new RaycastHit ();
							Physics.Raycast (raycastRay, out hitInfo, VISION_DISTANCE, layerMask);
						
							//If it's close to the distance to the tile, we can see the tile
							bool canSeeMesh = hitInfo.transform == meshRenderer.transform || 
								distanceToMesh < hitInfo.distance || hitInfo.transform == null;
							if (!canSeeMesh)
							{
								canSeeAllMeshesInTile = false;
								break;
							}
#if DRAW_SIGHT_LINES
							Debug.DrawLine (raycastRay.origin, hitInfo.point);
#endif
						}
						
						if (canSeeAllMeshesInTile)
						{
							curTile.ShowTile (canSeeAllMeshesInTile);        
						} 
						else 
						{
							unseenLeftovers.Add (curTile);
						}
					}
					
					this.unseenTiles = new List<TileRoot> (unseenLeftovers);
				}
			}
			
			foreach (TileRoot unseenTile in this.unseenTiles) 
			{
				unseenTile.ShowTile (false);
			}
			
			this.visionDirty = false;
		}
		
		public void ShowAllTiles()
		{
			foreach (TileRoot curTile in this.allTiles) 
			{
				curTile.ShowTile(true);
			}
		}
		#endregion

		#region private methods
		#endregion
	}

}

