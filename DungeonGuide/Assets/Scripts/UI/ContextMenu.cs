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
		private Button tintCharacterButton;
		
		[SerializeField]
		private ObjectCreationUi characterCreationUi;
		
		[SerializeField]
		private GameObject extendedContextMenuUi;
		
		[SerializeField]
		private GameObject tintCharacterUi;
		
		private Vector3 snappedActionLocation;
		private Vector3 mouseWorldLocation;
	
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
			this.tintCharacterButton.interactable = isCharacterSelected;
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
		}
		
		public void DeleteSelectedCharacter()
		{
			SceneManager.selectedChCtrl.DeleteSelectedCharacter();
			HideAllContextMenues();
		}
		
		public void CreateCharacter()
		{
			this.characterCreationUi.OpenObjectCreation(this.snappedActionLocation);
			HideContextMenu();
		}
		
		public void RevealObjects()
		{
			SceneManager.interactiveObjCtrl.RevealAppropriateObjects(this.mouseWorldLocation);
			HideAllContextMenues();
		}
		
		public void ToggleInteraction()
		{
			SceneManager.interactiveObjCtrl.ToggleAppropriateObjects(this.mouseWorldLocation);
			HideAllContextMenues();
		}
		
		public void ToggleMovementFreedom()
		{
			SceneManager.selectedChCtrl.GetSelectedCharacter().freeMovement = 
				!SceneManager.selectedChCtrl.GetSelectedCharacter().freeMovement;
			HideAllContextMenues();
		}
		
		public void DisplayExtendedContextMenu()
		{
			HideContextMenu();
			this.extendedContextMenuUi.SetActive(true);
		}
		
		public void HideExtendedContextMenu()
		{			
			HideAllContextMenues();
		}
		
		public void DisplayTintCharacterMenu()
		{
			HideContextMenu();
			this.tintCharacterUi.SetActive(true);
		}
		
		public void HideTintCharacterMenu()
		{			
			HideAllContextMenues();			
		}
		
		public bool IsContextMenuActive()
		{
			return this.tintCharacterUi.activeSelf ||
				this.extendedContextMenuUi.activeSelf ||
				this.gameObject.activeSelf ||
				this.characterCreationUi.gameObject.activeSelf;
		}
		
		public void TintCharacter(Color colorToTint)
		{			
			SceneManager.selectedChCtrl.TintSelectedCharacter(colorToTint);
			HideAllContextMenues();
		}		
		
		#endregion

		#region private methods
		private void HideAllContextMenues()
		{
			this.tintCharacterUi.SetActive(false);
			this.extendedContextMenuUi.SetActive(false);
			this.gameObject.SetActive(false);
			
			if (SceneManager.selectedChCtrl.IsCharacterSelected())
			{
				SceneManager.selectedChCtrl.DeselectCharacter();
			}
		}
		#endregion
	}

}

