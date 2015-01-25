using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DungeonGuide
{
	public class LongPressMenu : MonoBehaviour
	{
		[SerializeField]
		private Button deleteButton;
		
		[SerializeField]
		private Button freedomButton;
		
		[SerializeField]
		private Text freedomText;
		
		[SerializeField]
		private Button toggleButton;
		
		[SerializeField]
		private CharacterCreationUi characterCreationUi;
		
		private Vector3 snappedActionLocation;
		private Vector3 mouseWorldLocation;
	
		#region initializers
		private void Awake()
		{			
			if (Time.time < 0.1f)
			{
				Log.Warning("You left the LongPressMenu active. Deactivating at startup, but you should really do this in the editor");
				this.gameObject.SetActive(false);
			}
		}
		#endregion

		#region public methods
		public void DisplayLongPressMenu(Vector3 snappedActionLocation, Vector3 mouseLocationInWorld)
		{
			this.gameObject.SetActive(true);
			this.snappedActionLocation = snappedActionLocation;
			this.mouseWorldLocation = mouseLocationInWorld;
			
			//Turn off the delete button if we didn't hit a character
			bool isCharacterSelected = SceneManager.selectedChCtrl.IsCharacterSelected();
			this.deleteButton.interactable = isCharacterSelected;
			this.freedomButton.interactable = isCharacterSelected;
			this.freedomText.text = "Freedom";
			if (isCharacterSelected && SceneManager.selectedChCtrl.GetSelectedCharacter().freeMovement)
			{
				this.freedomText.text = "Constrain";
			}
			
			//Turn off the toggle button if we didn't hit a toggleable
			this.toggleButton.interactable = 
				SceneManager.interactiveObjCtrl.IsTogglableObjectInRange(mouseWorldLocation);
		}
		
		public void HideLongPressMenu()
		{
			this.gameObject.SetActive(false);
			
			if (SceneManager.selectedChCtrl.IsCharacterSelected())
			{
				SceneManager.selectedChCtrl.DeselectCharacter();
			}
		}
		
		public void DeleteSelectedCharacter()
		{
			SceneManager.selectedChCtrl.DeleteSelectedCharacter();
			HideLongPressMenu();
		}
		
		public void CreateCharacter()
		{
			this.characterCreationUi.OpenCharacterCreation(this.snappedActionLocation);
			HideLongPressMenu();
		}
		
		public void RevealObjects()
		{
			SceneManager.interactiveObjCtrl.RevealAppropriateObjects(this.mouseWorldLocation);
			HideLongPressMenu();
		}
		
		public void ToggleInteraction()
		{
			SceneManager.interactiveObjCtrl.ToggleAppropriateObjects(this.mouseWorldLocation);
			HideLongPressMenu();
		}
		
		public void ToggleMovementFreedom()
		{
			SceneManager.selectedChCtrl.GetSelectedCharacter().freeMovement = 
				!SceneManager.selectedChCtrl.GetSelectedCharacter().freeMovement;
			HideLongPressMenu();
		}
		#endregion

		#region private methods

		#endregion
	}

}

