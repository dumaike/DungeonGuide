using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class LongPressMenu : MonoBehaviour
	{
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
		}
		
		public void DeleteSelectedCharacter()
		{
			SceneManager.SelectedChCtrl.DeleteSelectedCharacter();
		}
		#endregion

		#region private methods

		#endregion
	}

}

