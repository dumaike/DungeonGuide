using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TabButtonScript : MonoBehaviour 
{
	[SerializeField]
	private List<Button> otherTabButtons;
	
	[SerializeField]
	private List<GameObject> otherTabsContent;
	
	[SerializeField]
	private GameObject thisTabContent;
	
	public void ThisTabClicked()
	{	
		//Enable other tabs when this one is clicked
		foreach (Button button in this.otherTabButtons)
		{
			button.interactable = true;
		}
		
		//Disable other content when this tab is clicked
		foreach (GameObject obj in this.otherTabsContent)
		{
			obj.SetActive(false);
		}
		
		//Disable this tab and enable this content
		this.GetComponent<Button>().interactable = false;
		this.thisTabContent.SetActive(true);
	}
}
