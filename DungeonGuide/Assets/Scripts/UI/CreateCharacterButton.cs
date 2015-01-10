using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class CreateCharacterButton : MonoBehaviour
	{
		[SerializeField]
		private MoveableRoot objectToCreate;
		
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
			MoveableRoot createdCharacter = Instantiate(this.objectToCreate) as MoveableRoot;
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

