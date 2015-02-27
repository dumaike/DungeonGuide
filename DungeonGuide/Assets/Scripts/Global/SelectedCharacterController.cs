using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class SelectedCharacterController
	{		
		private MoveableEntity selectedCharacter;
		
		#region initializers
		public SelectedCharacterController()
		{
		
		}
		#endregion

		#region public methods
		public void SelectCharacter(MoveableEntity character)
		{
			this.selectedCharacter = character;
			this.selectedCharacter.CharacterSelected(true);
		}
		
		public void TintSelectedCharacter(Color color)
		{
			this.selectedCharacter.TintCharacter(color);
		}		
		
		public MoveableEntity GetSelectedCharacter()
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
			SceneManager.eventCtr.FireObjectRemovedEvent(this.selectedCharacter, this.selectedCharacter.transform.position);
			SceneManager.Instance.DestroyGo(this.selectedCharacter.gameObject);
			DeselectCharacter();						
		}
		#endregion

		#region private methods

		#endregion
	}

}

