using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DungeonGuide
{
	public class LongPressMenu : MonoBehaviour
	{
		[SerializeField]
		private Button deleteButton;
	
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
			
			if (display)
			{
				this.deleteButton.interactable = SceneManager.SelectedChCtrl.IsCharacterSelected();
			}
		}
		
		public void DeleteSelectedCharacter()
		{
			SceneManager.SelectedChCtrl.DeleteSelectedCharacter();
			DisplayLongPressMenu(false);
		}
		#endregion

		#region private methods

		#endregion
	}

}

