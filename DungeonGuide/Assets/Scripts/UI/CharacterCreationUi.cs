using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class CharacterCreationUi : MonoBehaviour
	{
		public Vector3 characterCreationPosition {get; private set;}
	
		#region public methods
		public void CloseCharacterCreation()
		{
			this.gameObject.SetActive(false);
		}
		
		public void OpenCharacterCreation(Vector3 positionToCreateCharacter)
		{
			this.characterCreationPosition = positionToCreateCharacter;
			this.gameObject.SetActive(true);
		}
		#endregion

		#region private methods

		#endregion
	}

}

