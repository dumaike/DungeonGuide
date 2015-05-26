using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace DungeonGuide
{
	public class PlayerNameInitiativeToggles : MonoBehaviour 
	{
		[SerializeField]
		private ToggleGroup groupToDriveName;
		
		[SerializeField]
		private InputField nameField;
		
		private List<string> childTogglesNames = new List<string>();
			
		// Use this for initialization
		private void Start () 
		{
			//When a check box is ticked, set the name to that
			Toggle[] childToggles = GetComponentsInChildren<Toggle>(true);
			foreach (Toggle toggle in childToggles)
			{
				string toggleName = toggle.GetComponentInChildren<Text>().text;
				this.childTogglesNames.Add(toggleName);
				toggle.onValueChanged.AddListener(toggled => 
				{
					SceneManager.eventCtr.FireMenuItemClickedEvent();	
					
					if (toggled)
					{
						this.nameField.text = toggleName;
					}
				});
			}
			
			this.nameField.onValueChange.AddListener(OnLabelValueChanged);
		}
		
		private void OnEnable()
		{
			Toggle[] childToggles = GetComponentsInChildren<Toggle>(true);
			foreach (Toggle toggle in childToggles)
			{
				toggle.isOn = false;
			}
		}
		
		/// <summary>
		/// A callback for if a user starts typing in the text field, we
		/// want to make sure the premade check boxes are off.
		/// </summary>
		/// <param name="newValue">New value.</param>
		private void OnLabelValueChanged(string newValue)
		{
			if (this.childTogglesNames.Contains(newValue))
			{
				return;
			}
			
			this.groupToDriveName.SetAllTogglesOff();
		}
	}
}