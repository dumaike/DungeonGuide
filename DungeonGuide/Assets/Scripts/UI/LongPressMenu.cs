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

		private void Start()
		{

		}

		private void OnDestroy()
		{

		}
		#endregion

		#region public methods
		public void DisplayLongPressMenu(Vector3 snappedActionLocation, Vector3 mouseLocationInWorld)
		{
			this.gameObject.SetActive(true);
			this.snappedActionLocation = snappedActionLocation;
			this.mouseWorldLocation = mouseLocationInWorld;
			
			//Turn off the delete button if we didn't hit a character
			this.deleteButton.interactable = SceneManager.SelectedChCtrl.IsCharacterSelected();
			
			//Turn off the toggle button if we didn't hit a toggleable
			this.toggleButton.interactable = 
				SceneManager.InteractiveObjCtrl.IsTogglableObjectInRange(mouseWorldLocation);
		}
		
		public void HideLongPressMenu()
		{
			this.gameObject.SetActive(false);
			
			if (SceneManager.SelectedChCtrl.IsCharacterSelected())
			{
				SceneManager.SelectedChCtrl.DeselectCharacter();
			}
		}
		
		public void DeleteSelectedCharacter()
		{
			SceneManager.SelectedChCtrl.DeleteSelectedCharacter();
			HideLongPressMenu();
		}
		
		public void CreateCharacter()
		{
			this.characterCreationUi.OpenCharacterCreation(this.snappedActionLocation);
			HideLongPressMenu();
		}
		
		public void RevealObjects()
		{
			SceneManager.InteractiveObjCtrl.RevealAppropriateObjects(this.mouseWorldLocation);
			HideLongPressMenu();
		}
		
		public void ToggleInteraction()
		{
			SceneManager.InteractiveObjCtrl.ToggleAppropriateObjects(this.mouseWorldLocation);
			HideLongPressMenu();
		}
		#endregion

		#region private methods

		#endregion
	}

}

