using UnityEngine;

namespace DungeonGuide
{
	public class CreateCharacterButton : CreateObjectButton
	{
		//The number of characters of this type created so far
		private int creationCounter = 0;
	
		private enum CharacterTemplate
		{
			One_by_One_Character_Template,
			One_by_One_Player_Template,
			Two_by_Two_Character_Template,
			Two_by_Two_Player_Template,
			Four_by_Four_Character_Template
		};		
	
		[SerializeField]
		private CharacterTemplate characterType = CharacterTemplate.One_by_One_Character_Template;
		
		[SerializeField]
		private Texture characterTexture = null;
		
		#region initializers
		public override void Awake()
		{
			base.Awake();
			
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
				case CharacterTemplate.Two_by_Two_Player_Template:
					this.objectToCreate = Resources.Load("2x2PlayerCharacterTemplate") as GameObject;
					break;
				case CharacterTemplate.Four_by_Four_Character_Template:
					this.objectToCreate = Resources.Load("4x4CharacterTemplate") as GameObject;
					break;
				default:
					Log.Error("There's a template type that doesn't map to a prefab.", LogChannel.LOGIC, this);
					break;
			}
		}
		
		#endregion

		#region protected methods
		public override GameObject CreateObject()
		{
			this.creationCounter++;
		
			GameObject createdCharacter = base.CreateObject();

			//Set up the character texture
			MeshRenderer characterMesh = createdCharacter.GetComponentInChildren<MeshRenderer>();
			characterMesh.material.mainTexture = this.characterTexture;
			
			MoveableEntity moveableRootOfCharacter = createdCharacter.GetComponentInChildren<MoveableEntity>();
			moveableRootOfCharacter.SetCharacterText(this.creationCounter.ToString());

			return createdCharacter;
		}
		#endregion

		#region private methods

		#endregion
	}

}

