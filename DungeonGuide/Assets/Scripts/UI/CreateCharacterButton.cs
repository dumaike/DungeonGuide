using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DungeonGuide
{
	public class CreateCharacterButton : MonoBehaviour
	{
		//The number of characters of this type created so far
		private int creationCounter = 0;
	
		private enum CharacterTemplate
		{
			One_by_One_Character_Template,
			One_by_One_Player_Template,
			Two_by_Two_Character_Template
		};		
	
		[SerializeField]
		private CharacterTemplate characterType = CharacterTemplate.One_by_One_Character_Template;
		
		[SerializeField]
		private Texture characterTexture;
		
		private CharacterCreationUi creationUi;
		
		private GameObject objectToCreate;
		
		private GameObject gameplayObjectRoot;
		
		#region initializers
		public void Awake()
		{
			this.creationUi = this.transform.GetComponentInParent<CharacterCreationUi>();
			
			this.gameplayObjectRoot = GameObject.Find(GridUtility.GAMEPLAY_OBJECT_ROOT_NAME);
			
			switch (this.characterType)
			{
				case CharacterTemplate.One_by_One_Character_Template:
					this.objectToCreate = Resources.Load("1x1CharacterTemplate") as GameObject;
				break;
				case CharacterTemplate.One_by_One_Player_Template:
					this.objectToCreate = Resources.Load("1x1PlayerCharacterTemplate") as GameObject;
					break;
				case CharacterTemplate.Two_by_Two_Character_Template:
					this.objectToCreate = Resources.Load("2x2CharacterTemplate") as GameObject;
					break;
				default:
					Log.Error("There's a template type that doesn't map to a prefab.", LogChannel.LOGIC, this);
					break;
			}
		}
		
		#endregion

		#region public methods
		public void CreateCharacter()
		{
			this.creationCounter++;
		
			GameObject createdCharacter = Instantiate(this.objectToCreate) as GameObject;
			createdCharacter.transform.position = this.creationUi.characterCreationPosition;
			createdCharacter.transform.parent = this.gameplayObjectRoot.transform;
			
			//Set up the character texture
			MeshRenderer characterMesh = createdCharacter.GetComponentInChildren<MeshRenderer>();
			characterMesh.material.mainTexture = this.characterTexture;
			
			MoveableEntity moveableRootOfCharacter = createdCharacter.GetComponentInChildren<MoveableEntity>();
			moveableRootOfCharacter.SetCharacterText(this.creationCounter.ToString());
			
			this.creationUi.CloseCharacterCreation();			
			SceneManager.eventCtr.FireObjectCreatedEvent(moveableRootOfCharacter, moveableRootOfCharacter.transform.position);
		}
		#endregion

		#region private methods

		#endregion
	}

}

