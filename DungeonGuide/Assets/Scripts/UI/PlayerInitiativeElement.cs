using UnityEngine;
using System.Collections;

public class PlayerInitiativeElement : MonoBehaviour 
{
	private PlayerInitiativeView view;

	public void InitializeElement(PlayerInitiativeView view)
	{
		this.view = view;
	}

	public void RemoveElement()
	{
		this.view.RemovePlayer(this);
	}
}
