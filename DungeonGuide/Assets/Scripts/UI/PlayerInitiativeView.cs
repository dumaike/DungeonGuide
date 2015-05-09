using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInitiativeView : MonoBehaviour 
{
	[SerializeField]
	private PlayerInitiativeElement playerInitiativeElementTemplate;
	
	[SerializeField]
	private int maxPlayers = 10;

	List<PlayerInitiativeElement> playerEntries = 
		new List<PlayerInitiativeElement>();
		
	private int activeElementIndex = 0;
	
	public void AddPlayer()
	{
		if (this.playerEntries.Count >= this.maxPlayers)
		{
			return;
		}
	
		PlayerInitiativeElement newEle =
			Instantiate(this.playerInitiativeElementTemplate);
		
		newEle.transform.SetParent(this.transform, false);
		newEle.InitializeElement(this);
		
		this.playerEntries.Add(newEle);
		
		if (this.playerEntries.Count == 1)
		{
			newEle.SetActing();
		}
	}
	
	public void RemovePlayer(PlayerInitiativeElement elementToRemove)
	{
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
		this.playerEntries[this.activeElementIndex].SetWaiting();
		
		this.activeElementIndex = (this.activeElementIndex + 1)%this.playerEntries.Count;
		
		this.playerEntries[this.activeElementIndex].SetActing();
	}
	
}
