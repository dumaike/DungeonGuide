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
		
	private void Awake()
	{
		
	}
	
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
	}
	
	public void RemovePlayer(PlayerInitiativeElement elementToRemove)
	{
		this.playerEntries.Remove(elementToRemove);
		
		Destroy (elementToRemove.gameObject);
	}
	
}
