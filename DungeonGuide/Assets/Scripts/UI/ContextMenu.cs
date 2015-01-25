using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DungeonGuide
{
	public class ContextMenu : MonoBehaviour
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
				Log.Warning("You left the Context menu active. Deactivating at startup, but you should really do this in the editor");
				this.gameObject.SetActive(false);
			}
		}
		#endregion

		#region public methods
		public void DisplayContextMenu(Vector3 snappedActionLocation, Vector3 mouseLocationInWorld)
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
		
		public void HideContextMenu()
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
			HideContextMenu();
		}
		
		public void CreateCharacter()
		{
			this.characterCreationUi.OpenCharacterCreation(this.snappedActionLocation);
			HideContextMenu();
		}
		
		public void RevealObjects()
		{
			SceneManager.interactiveObjCtrl.RevealAppropriateObjects(this.mouseWorldLocation);
			HideContextMenu();
		}
		
		public void ToggleInteraction()
		{
			SceneManager.interactiveObjCtrl.ToggleAppropriateObjects(this.mouseWorldLocation);
			HideContextMenu();
		}
		
		public void ToggleMovementFreedom()
		{
			SceneManager.selectedChCtrl.GetSelectedCharacter().freeMovement = 
				!SceneManager.selectedChCtrl.GetSelectedCharacter().freeMovement;
			HideContextMenu();
		}
		#endregion

		#region private methods

		#endregion
	}

}

