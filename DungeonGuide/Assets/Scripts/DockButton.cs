using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DungeonGuide
{
	public class DockButton : MonoBehaviour
	{
		/// <summary>
		/// The UI for the dock to be shifted when this button is pressed
		/// </summary>
		[SerializeField]
		private GameObject dockUI = null;
		
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
		
		[SerializeField]
		private Text dockButtonText = null;

		private bool isDockVisible = false;
		#region initializers
		#endregion

		#region public methods
		public void ToggleDockVisiblity()
		{
			float shiftAmount = this.characterDock.dockShift;
			if (this.isDockVisible)
			{
				shiftAmount = -shiftAmount;
			}
			Vector3 shiftVector = new Vector3(shiftAmount,0,0);
			
			this.dockUI.transform.position = this.dockUI.transform.position + shiftVector;
			this.dockButton.transform.position = this.dockButton.transform.position + shiftVector;
			this.characterDock.transform.position = this.characterDock.transform.position + shiftVector;
			
			this.isDockVisible = !this.isDockVisible;
			if (this.isDockVisible)
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