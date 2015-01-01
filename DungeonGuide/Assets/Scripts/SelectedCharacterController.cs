using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class SelectedCharacterController
	{		
		private CharacterRoot selectedCharacter;
		
		#region initializers
		public SelectedCharacterController()
		{
		
		}
		#endregion

		#region public methods
		public void SelectCharacter(CharacterRoot character)
		{
			this.selectedCharacter = character;
			this.selectedCharacter.CharacterSelected(true);
		}
		
		public CharacterRoot GetSelectedCharacter()
		{
			return this.selectedCharacter;
		}
		
		public void DeselectCharacter()
		{	
			this.selectedCharacter.CharacterSelected(false);
			this.selectedCharacter = null;
		}
		
		public bool IsCharacterSelected()
		{
			return this.selectedCharacter != null;
		}
		
		public void DeleteSelectedCharacter()
		{
			if (this.selectedCharacter is PlayerCharacterRoot)
			{
				SceneManager.ChVisionCtrl.RemoveCharacterFromVision(this.selectedCharacter as PlayerCharacterRoot);
			}
			
			SceneManager.Instance.DestroyGo(this.selectedCharacter.gameObject);
			DeselectCharacter();			
		}
		#endregion

		#region private methods

		#endregion
	}

}

