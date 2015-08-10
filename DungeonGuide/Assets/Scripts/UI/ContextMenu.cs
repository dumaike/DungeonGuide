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
		private Button rotationButton;

		[SerializeField]
		private Text freedomText;
		
		[SerializeField]
		private Button toggleButton;
		
		[SerializeField]
		private Button tintCharacterButton;

		//TODO Make all menus into a dictionary of context menus
		
		[SerializeField]
		private ObjectCreationUi characterCreationUi;
		
		[SerializeField]
		private GameObject extendedContextMenuUi;

		[SerializeField]
		private GameObject tintCharacterUi;

		[SerializeField]
		private ObjectRotationUi rotateObjectUi;

		private Vector3 snappedActionLocation;
		private Vector3 mouseWorldLocation;
		private MoveableEntity selectedObject = null;

		#region initializers
		private void Start()
		{
			SceneManager.eventCtr.objectSelected += HandleObjectSelected;
			this.gameObject.SetActive(false);
		}

		private void OnDestroy()
		{
			if (SceneManager.eventCtr != null)
			{
				SceneManager.eventCtr.objectSelected -= HandleObjectSelected;
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
			bool isCharacterSelected = this.selectedObject != null;
			this.deleteButton.interactable = isCharacterSelected;
			this.rotationButton.interactable = isCharacterSelected;
			this.tintCharacterButton.interactable = isCharacterSelected;
			
			this.freedomText.text = "Freedom";
			this.freedomButton.interactable = isCharacterSelected;
			if (isCharacterSelected && this.selectedObject.freeMovement)
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
			MoveableEntity cachedSelectedCharacter = this.selectedObject;
			SceneManager.eventCtr.FireObjectSelected(cachedSelectedCharacter, false);

			SceneManager.eventCtr.FireObjectRemovedEvent(
				cachedSelectedCharacter, cachedSelectedCharacter.transform.position);

			Destroy(cachedSelectedCharacter.gameObject);

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
			this.selectedObject.freeMovement = !this.selectedObject.freeMovement;
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

		public void DisplayRotateObjectMenu()
		{
			HideExtendedContextMenu();
			this.rotateObjectUi.gameObject.SetActive(true);
		}

		public void HideRotateObjectMenu()
		{
			HideAllContextMenues();
		}

		public bool IsContextMenuActive()
		{
			return this.tintCharacterUi.activeSelf ||
				this.extendedContextMenuUi.activeSelf ||
				this.gameObject.activeSelf ||
				this.characterCreationUi.gameObject.activeSelf ||
				this.rotateObjectUi.gameObject.activeSelf;
		}
		
		public void TintCharacter(Color colorToTint)
		{
			this.selectedObject.TintCharacter(colorToTint);
			HideAllContextMenues();
		}		
		
		#endregion

		#region private methods
		private void HideAllContextMenues()
		{
			this.tintCharacterUi.SetActive(false);
			this.extendedContextMenuUi.SetActive(false);
			this.gameObject.SetActive(false);
			this.characterCreationUi.gameObject.SetActive(false);
			this.rotateObjectUi.gameObject.SetActive(false);
		}

		private void HandleObjectSelected(MoveableEntity target, bool selected)
		{
			this.selectedObject = !selected ? null : target;
		}
		#endregion
	}

}

