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
		private MoveableEntity selectedCharacter = null;

		#region public methods
		public void DisplayContextMenu(Vector3 snappedActionLocation, Vector3 mouseLocationInWorld)
		{
			this.gameObject.SetActive(true);
			this.snappedActionLocation = snappedActionLocation;
			this.mouseWorldLocation = mouseLocationInWorld;
			
			//Turn off the delete button if we didn't hit a character
			bool isCharacterSelected = this.selectedCharacter != null;
			this.deleteButton.interactable = isCharacterSelected;
			this.freedomButton.interactable = isCharacterSelected;
			this.tintCharacterButton.interactable = isCharacterSelected;
			this.freedomText.text = "Freedom";
			if (isCharacterSelected && this.selectedCharacter.freeMovement)
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
			MoveableEntity cachedSelectedCharacter = this.selectedCharacter;

			SceneManager.eventCtr.FireObjectRemovedEvent(this.selectedCharacter, this.selectedCharacter.transform.position);
			SceneManager.eventCtr.FireObjectSelected(this.selectedCharacter, false);

			Destroy(cachedSelectedCharacter);

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
			this.selectedCharacter.freeMovement = !this.selectedCharacter.freeMovement;
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
			this.selectedCharacter.TintCharacter(colorToTint);
			HideAllContextMenues();
		}		
		
		#endregion

		#region private methods
		private void HideAllContextMenues()
		{
			this.tintCharacterUi.SetActive(false);
			this.extendedContextMenuUi.SetActive(false);
			this.gameObject.SetActive(false);
		}
		#endregion
	}

}

