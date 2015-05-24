using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DungeonGuide
{
	public class PlayerInitiativeView : MonoBehaviour 
	{
		[SerializeField]
		private PlayerInitiativeElement playerInitiativeElementTemplate;
		
		[SerializeField]
		private GameObject addPlayerDialog;
		
		[SerializeField]
		private InputField newPlayerName;
		
		[SerializeField]
		private InputField newPlayerInitiative;
		
		[SerializeField]
		private int maxPlayers = 10;
	
		List<PlayerInitiativeElement> playerEntries = 
			new List<PlayerInitiativeElement>();
			
		private int activeElementIndex = 0;
		
		public void AddPlayer()
		{
			//Clear out double clicks
			SceneManager.eventCtr.FireMenuItemClickedEvent();			
			
			if (this.playerEntries.Count >= this.maxPlayers)
			{
				return;
			}
			
			this.newPlayerInitiative.text = "";
			this.newPlayerName.text = "";
			this.addPlayerDialog.SetActive(true);
			
			//Set the player name as the next input
			EventSystem system = EventSystem.current;// EventSystemManager.currentSystem;
			Selectable next = 
				system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
			
			system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
		}
		
		public void ConfirmAddPlayer()
		{
			PlayerInitiativeElement newEle =
				Instantiate(this.playerInitiativeElementTemplate);
			
			newEle.transform.SetParent(this.transform, false);
			newEle.InitializeElement(this, this.newPlayerName.text);
			
			this.playerEntries.Add(newEle);
			
			if (this.playerEntries.Count == 1)
			{
				newEle.SetActing();
			}
			
			this.addPlayerDialog.SetActive(false);
		}
		
		public void RemovePlayer(PlayerInitiativeElement elementToRemove)
		{
			//Clear out double clicks
			SceneManager.eventCtr.FireMenuItemClickedEvent();
			
			int indexOfPlayer = this.playerEntries.IndexOf(elementToRemove);
			if (indexOfPlayer == this.activeElementIndex)
			{
				AdvanceInitiative();
			}
		
			this.playerEntries.Remove(elementToRemove);
			
			Destroy (elementToRemove.gameObject);
		}
		
		public void AdvanceInitiative()
		{
			//Clear out double clicks
			SceneManager.eventCtr.FireMenuItemClickedEvent();
		
			if (this.playerEntries.Count > 0)
			{
				this.playerEntries[this.activeElementIndex].SetWaiting();
				
				this.activeElementIndex = (this.activeElementIndex + 1)%this.playerEntries.Count;
				
				this.playerEntries[this.activeElementIndex].SetActing();
			}
		}
	}
}
