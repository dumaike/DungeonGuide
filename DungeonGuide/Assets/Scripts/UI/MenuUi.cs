using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class MenuUi : MonoBehaviour
	{	
		#region initializers
		private void Awake()
		{
			if (Time.time < 0.1f)
			{
				Log.Warning("You left the MenuUi active. Deactivating at startup, but you should really do this in the editor");
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
		public void CloseMenu()
		{
			this.gameObject.SetActive(false);
		}
		
		public void OpenMenu()
		{
			this.gameObject.SetActive(true);
		}
		#endregion

		#region private methods

		#endregion
	}

}

