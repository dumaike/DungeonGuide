using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class CreateCharacterButton : MonoBehaviour
	{
		[SerializeField]
		private CharacterRoot characterToCreate;
		
		private CharacterCreationUi creationUi;
		
		#region initializers
		public void Awake()
		{
			this.creationUi = this.transform.GetComponentInParent<CharacterCreationUi>();
		}
		
		#endregion

		#region public methods
		public void CreateCharacter()
		{
			CharacterRoot createdCharacter = Instantiate(this.characterToCreate) as CharacterRoot;
			createdCharacter.transform.position = this.creationUi.characterCreationPosition;
			if (createdCharacter is PlayerCharacterRoot)
			{
				SceneManager.ChVisionCtrl.AddCharacterToVision(createdCharacter as PlayerCharacterRoot);
			}
			this.creationUi.CloseCharacterCreation();
		}
		#endregion

		#region private methods

		#endregion
	}

}

