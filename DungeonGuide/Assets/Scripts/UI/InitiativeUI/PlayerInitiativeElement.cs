using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace DungeonGuide
{
	public class PlayerInitiativeElement : MonoBehaviour 
	{
		private PlayerInitiativeView view;
		
		[SerializeField]
		private GameObject actingObject;
		
		[SerializeField]
		private GameObject waitingObject;
		
		[SerializeField]
		private Text playerName;
		
		public int initiative {get; private set;}
	
		public void InitializeElement(PlayerInitiativeView view, string playerName, int playerInitiative)
		{
			this.view = view;
			this.playerName.text = playerName;
			this.initiative = playerInitiative;
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
}