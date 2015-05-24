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
		void Start () 
		{
			Debug.Log("Called once?");
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