using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DungeonGuide
{
	//[ExecuteInEditMode]
	public class CharacterDock : MonoBehaviour
	{
		[SerializeField]
		private GameObject rightEdge = null;
		
		[SerializeField]
		private GameObject leftEdge = null;
		
		public Vector3 dockShift {get; private set;}
		
		public bool isDockVisible {get; private set;}
		
		public const float UI_HEIGHT_OFFSET = 2.0f;
		
		private CharacterRoster roster = null;

		#region initializers
		private void Awake()
		{
			this.isDockVisible = false;
			MoveDockToEdge ();
			
			this.roster = GameObject.FindObjectOfType<CharacterRoster>();
			if (this.roster == null || this.roster.availableCharacterPrefabs.Count == 0)
			{
				Log.Error("Could not find a character roster (or the roster is empty). Add one or there won't be any characters for the scene!", LogChannel.EDITOR_SETUP);
			}
			else
			{
				//HACK Only supports 1x1 characters
			
				//Set the character placement at the top left of the dock minus half the height of the first character
				Vector3 topLeftEdgeOfCamera = Camera.main.ScreenToWorldPoint (new Vector3 (0, Screen.height, UI_HEIGHT_OFFSET));			
				topLeftEdgeOfCamera.x = this.leftEdge.transform.position.x;			
				float startingX = topLeftEdgeOfCamera.x;	
				
				CharacterRoot previousCharacter = null;
			
				List<CharacterRoot> characters = this.roster.availableCharacterPrefabs;
				foreach (CharacterRoot character in characters)
				{
					CharacterRoot createdCharacter = Instantiate(character) as CharacterRoot;
					if (previousCharacter == null)
					{						
						topLeftEdgeOfCamera.z -= createdCharacter.characterDimensions.z / 2.0f;
						topLeftEdgeOfCamera.x += createdCharacter.characterDimensions.x / 2.0f;
					}
					else
					{
						topLeftEdgeOfCamera.x += createdCharacter.characterDimensions.x;
						if (topLeftEdgeOfCamera.x + (createdCharacter.characterDimensions.x / 2.0f) > this.rightEdge.transform.position.x)
						{
							topLeftEdgeOfCamera.x = startingX + createdCharacter.characterDimensions.x / 2.0f;
							topLeftEdgeOfCamera.z -= createdCharacter.characterDimensions.z;
						}
					}
					
					createdCharacter.transform.position = topLeftEdgeOfCamera;
					createdCharacter.transform.parent = this.transform;
					previousCharacter = character;
				}				
			}
		}
		#endregion

		#region public methods
		public bool IsPointInDock(Vector3 point)
		{
			return point.x < rightEdge.transform.position.x && this.isDockVisible;
		}		
		
		public void ToggleDockVisibility()
		{			
			this.isDockVisible = !this.isDockVisible;
			
			MoveDockToEdge();			
		}		
		#endregion

		#region private methods
		private void Update()
		{
			MoveDockToEdge();
		}

		private void MoveDockToEdge()
		{
			//The location in our UI that we want to be at the edge of the camera (different if we're docked or not)
			Vector3 referenceLocation = this.isDockVisible ? this.leftEdge.transform.position : this.rightEdge.transform.position;
			
			//Move the dock so that it's centered independent of screen space
			Vector3 leftCenterEdgeOfCamera = Camera.main.ScreenToWorldPoint (new Vector3 (0, Screen.height / 2.0f, 0));
			Vector3 dockEdgeDifference = leftCenterEdgeOfCamera - referenceLocation;
			Vector3 newPosition = this.transform.position + dockEdgeDifference;
			newPosition.y = this.transform.position.y;
			this.transform.position = newPosition;
			
			this.dockShift = new Vector3(this.rightEdge.transform.position.x - this.leftEdge.transform.position.x,0,0);	
			if (!this.isDockVisible)
			{
				this.dockShift = -this.dockShift;
			}
		}
		#endregion
	}

}

