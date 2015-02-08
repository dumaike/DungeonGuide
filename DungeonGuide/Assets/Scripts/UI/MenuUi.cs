using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class MenuUi : MonoBehaviour
	{	
		#region public methods
		public void CloseMenu()
		{
			SceneManager.userInputCtlr.MenuItemClicked();
			this.gameObject.SetActive(false);
		}
		
		public void OpenMenu()
		{
			SceneManager.userInputCtlr.MenuItemClicked();
			this.gameObject.SetActive(true);
		}
		#endregion

		#region private methods

		#endregion
	}

}

