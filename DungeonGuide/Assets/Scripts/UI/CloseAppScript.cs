using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class CloseAppScript : MonoBehaviour
	{
		[SerializeField]
		GameObject closeDialog;
	
		#region public methods
		public void CloseApplication()
		{
			Application.Quit ();
		}
		
		public void ShowCloseDialog()
		{
			SceneManager.userInputCtlr.MenuItemClicked();
			this.closeDialog.gameObject.SetActive(true);
		}
		
		public void HideCloseDialog()
		{
			SceneManager.userInputCtlr.MenuItemClicked();
			this.closeDialog.gameObject.SetActive(false);
		}
		#endregion
	}

}