using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class LongPressMenu : MonoBehaviour
	{
		#region initializers
		private void Awake()
		{

		}

		private void Start()
		{

		}

		private void OnDestroy()
		{

		}
		#endregion

		#region public methods
		public void DisplayLongPressMenu(bool display)
		{
			this.gameObject.SetActive(display);
		}
		
		public void DeleteSelectedCharacter()
		{
			CharacterRoot selectedCharacter = UserInputController.Instance.selectedCharacter;
			Destroy(selectedCharacter.gameObject);
		
			UserInputController.Instance.ResetActionsInProgress();
			if (selectedCharacter is PlayerCharacterRoot)
			{
				CharacterVisionController.Instance.RemoveCharacterFromVision(selectedCharacter as PlayerCharacterRoot);
			}
		}
		#endregion

		#region private methods

		#endregion
	}

}

