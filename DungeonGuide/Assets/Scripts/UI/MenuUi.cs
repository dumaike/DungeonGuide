using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class MenuUi : MonoBehaviour
	{	
		#region public methods
		public void CloseMenu()
		{
			SceneManager.eventCtr.FireMenuItemClickedEvent();
			this.gameObject.SetActive(false);
		}
		
		public void OpenMenu()
		{
			SceneManager.eventCtr.FireMenuItemClickedEvent();
			this.gameObject.SetActive(true);
		}
		#endregion

		#region private methods

		#endregion
	}

}

