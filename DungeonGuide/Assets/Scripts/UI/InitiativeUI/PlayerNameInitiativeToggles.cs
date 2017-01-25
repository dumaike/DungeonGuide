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
		private Text nameField;

        [SerializeField]
        private GameObject placeholder;
		
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
                        this.placeholder.SetActive(false);
					}
				});
			}
			
		}
		
		private void OnEnable()
		{
			Toggle[] childToggles = GetComponentsInChildren<Toggle>(true);
			foreach (Toggle toggle in childToggles)
			{
				toggle.isOn = false;
			}
		}
	}
}