using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DungeonGuide
{
	public class DockButton : MonoBehaviour
	{		
		/// <summary>
		/// The button itself, to be shifted when it's pressed
		/// </summary>
		[SerializeField]
		private GameObject dockButton = null;
		
		/// <summary>
		/// The character dock to be shifted when this button is pressed
		/// </summary>
		[SerializeField]
		private CharacterDock characterDock = null;
		
		/// <summary>
		/// The character dock UI to be shifted when this button is pressed
		/// </summary>
		[SerializeField]
		private GameObject dockUI  = null;
		
		[SerializeField]
		private Text dockButtonText = null;

		#region initializers
		#endregion

		#region public methods
		public void ToggleDockVisiblity()
		{
			this.characterDock.ToggleDockVisibility();
			
			this.dockButton.transform.position = this.dockButton.transform.position + this.characterDock.dockShift;
			this.dockUI.transform.position = this.dockUI.transform.position + this.characterDock.dockShift;
			
			if (this.characterDock.isDockVisible)
			{
				this.dockButtonText.text = "<<";
			}
			else
			{
				this.dockButtonText.text = ">>";
			}			
		}
		#endregion

		#region private methods

		#endregion
	}

}