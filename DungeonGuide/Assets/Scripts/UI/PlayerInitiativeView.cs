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
			
		private PlayerInitiativeElement actingElement;
		
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
			newEle.InitializeElement(this, this.newPlayerName.text, int.Parse(this.newPlayerInitiative.text));
			
			this.playerEntries.Add(newEle);
			
			if (this.playerEntries.Count == 1)
			{
				newEle.SetActing();
				this.actingElement = newEle;
			}
			
			this.playerEntries.Sort((x, y) => (y.initiative.CompareTo(x.initiative)));
			
			for (int iPlayerIndex = 0; iPlayerIndex < this.playerEntries.Count; ++iPlayerIndex)
			{
				PlayerInitiativeElement player = this.playerEntries[iPlayerIndex];
				player.transform.SetSiblingIndex(iPlayerIndex);
			}
			
			this.addPlayerDialog.SetActive(false);
		}
		
		public void RemovePlayer(PlayerInitiativeElement elementToRemove)
		{
			//Clear out double clicks
			SceneManager.eventCtr.FireMenuItemClickedEvent();
			
			if (elementToRemove == this.actingElement)
			{
				AdvanceInitiative();
			}
		
			this.playerEntries.Remove(elementToRemove);			
			
			Destroy (elementToRemove.gameObject);
			
			if (this.playerEntries.Count == 0)
			{
				this.actingElement = null;
			}
		}
		
		public void AdvanceInitiative()
		{
			//Clear out double clicks
			SceneManager.eventCtr.FireMenuItemClickedEvent();
		
			if (this.actingElement != null)
			{
				this.actingElement.SetWaiting();
				
				int activeElementIndex = this.playerEntries.IndexOf(this.actingElement);
				activeElementIndex = (activeElementIndex + 1)%this.playerEntries.Count;
				
				this.actingElement = this.playerEntries[activeElementIndex];
				
				this.actingElement.SetActing();
			}
		}
	}
}
