using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DungeonGuide
{
	public class CharacterVisionController
	{				
		private const float VISION_DISTANCE = 15.0f;
		private const int NUM_RAYS = 360;

		public const string INVISIBLE_LAYER_NAME = "NonSightBlocking";
		
		private Vector3 VISION_OFFSET = new Vector3(0, 0.1f, 0);

		private List<PlayerCharacterRoot> playerCharacters;
		
		private List<List<Vector3>> visionPerCharacter;
		
		private bool visionDirty = true;

		#region initializers
		public CharacterVisionController()
		{						
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
			
			//Create the vision points
			foreach (PlayerCharacterRoot player in this.playerCharacters) 
			{
				Vector3 characterVisionOrigin = player.transform.position + VISION_OFFSET;
				float raycastStep = 360/NUM_RAYS;
				for (int iRayIndex = 0; iRayIndex < NUM_RAYS; ++iRayIndex)
				{
					Vector3 directionVector = Quaternion.Euler(0, iRayIndex*raycastStep, 0)*Vector3.right;
					Ray raycastRay = new Ray(characterVisionOrigin, directionVector);
					
					RaycastHit hitInfo = new RaycastHit ();
					bool hitSomething = Physics.Raycast (raycastRay, out hitInfo, VISION_DISTANCE, layerMask);
					Vector3 hitPoint;
					if (hitSomething)
					{
						hitPoint = hitInfo.point;
					}
					else
					{
						hitPoint = characterVisionOrigin + directionVector*VISION_DISTANCE;
					}
					
					Debug.DrawRay(characterVisionOrigin, hitPoint - characterVisionOrigin);
				}	
			}
			
			//Draw the character's vision mesh
			
			
			//this.visionDirty = false;
		}
		
		public void ShowAllTiles()
		{
		
		}
		#endregion

		#region private methods
		#endregion
	}

}

