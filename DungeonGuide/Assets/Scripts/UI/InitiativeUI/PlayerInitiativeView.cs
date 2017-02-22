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
		private Text newPlayerName;
		
		[SerializeField]
		private Text newPlayerInitiative;

        [SerializeField]
        private Dropdown playerNameDropdown;
		
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
            this.playerNameDropdown.value = 0;			
		}
		
		public void ConfirmAddPlayer()
		{
			PlayerInitiativeElement newEle =
				Instantiate(this.playerInitiativeElementTemplate);

            int numberOfDuplicatesFound = 1;

            for (int iNameIndex = 0; iNameIndex < this.playerEntries.Count; ++iNameIndex)
            {
                PlayerInitiativeElement player = this.playerEntries[iNameIndex];
                int parenthesisPosition = player.playerName.text.IndexOf("(");
                if (parenthesisPosition == -1 && this.newPlayerName.text.Equals(player.playerName.text))
                {
                    player.playerName.text = player.playerName.text + "(" + numberOfDuplicatesFound.ToString("N0") + ")";
                    ++numberOfDuplicatesFound;
                }
                else if (this.newPlayerName.text.Equals(player.playerName.text.Substring(0,parenthesisPosition)))
                {
                   ++numberOfDuplicatesFound;
                }
            }
            if (numberOfDuplicatesFound != 1)
            {
                this.newPlayerName.text = this.newPlayerName.text + "(" + numberOfDuplicatesFound.ToString("N0") + ")";
            }

            newEle.transform.SetParent(this.transform, false);
			
			int playerInitiative = 0;
			int.TryParse(this.newPlayerInitiative.text, out playerInitiative);
			newEle.InitializeElement(this, this.newPlayerName.text, playerInitiative);
			
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
