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
	
		public void InitializeElement(PlayerInitiativeView view, string playerName)
		{
			this.view = view;
			this.playerName.text = playerName;
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