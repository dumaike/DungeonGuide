using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DungeonGuide
{
	public class CreateCharacterButton : MonoBehaviour
	{
		private enum CharacterTemplate
		{
			One_by_One_Character_Template,
			One_by_One_Player_Template
		};		
	
		[SerializeField]
		private CharacterTemplate characterType = CharacterTemplate.One_by_One_Character_Template;
		
		[SerializeField]
		private Texture characterTexture;
		
		private CharacterCreationUi creationUi;
		
		private GameObject objectToCreate;
		
		#region initializers
		public void Awake()
		{
			this.creationUi = this.transform.GetComponentInParent<CharacterCreationUi>();
			
			switch (this.characterType)
			{
				case CharacterTemplate.One_by_One_Character_Template:
					this.objectToCreate = Resources.Load("1x1CharacterTemplate") as GameObject;
					break;
				case CharacterTemplate.One_by_One_Player_Template:
					this.objectToCreate = Resources.Load("1x1PlayerCharacterTemplate") as GameObject;
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
			GameObject createdCharacter = Instantiate(this.objectToCreate) as GameObject;
			createdCharacter.transform.position = this.creationUi.characterCreationPosition;
			
			//Register the character for vision if applicable
			MoveableRoot moveableRootOfCharacter = createdCharacter.GetComponentInChildren<MoveableRoot>();
			if (moveableRootOfCharacter is PlayerCharacterRoot)
			{
				SceneManager.chVisionCtrl.AddCharacterToVision(moveableRootOfCharacter as PlayerCharacterRoot);
			}
			
			//Set up the character texture
			MeshRenderer characterMesh = createdCharacter.GetComponentInChildren<MeshRenderer>();
			characterMesh.material.mainTexture = this.characterTexture;
			
			this.creationUi.CloseCharacterCreation();
		}
		#endregion

		#region private methods

		#endregion
	}

}

