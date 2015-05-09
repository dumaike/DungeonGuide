using UnityEngine;
using System.Collections;

public class PlayerInitiativeElement : MonoBehaviour 
{
	private PlayerInitiativeView view;
	
	[SerializeField]
	private GameObject actingObject;
	
	[SerializeField]
	private GameObject waitingObject;

	public void InitializeElement(PlayerInitiativeView view)
	{
		this.view = view;
		SetWaiting();
	}

	public void RemoveElement()
	{
		this.view.RemovePlayer(this);
	}
	
	public void SetActing()
	{
		this.actingObject.SetActive(true);
		this.waitingObject.SetActive(false);
	}
	
	public void SetWaiting()
	{		
		this.actingObject.SetActive(false);
		this.waitingObject.SetActive(true);
	}
}
