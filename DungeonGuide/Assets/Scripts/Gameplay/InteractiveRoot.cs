using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DungeonGuide
{
	public class InteractiveRoot : MonoBehaviour
	{
		public enum InteractiveType
		{
			REVEALABLE,
			TOGGLEABLE
		}
	
		[SerializeField]
		private List<GameObject> objectToShow;
		
		[SerializeField]
		private List<GameObject> objectsToHide;
		
		[SerializeField]
		private InteractiveType typeSerialized = InteractiveType.TOGGLEABLE;
		public InteractiveType type {get {return this.typeSerialized;} private set {this.typeSerialized = value;}}
		
		private bool objectState = false;
	
		#region initializers
		#endregion

		#region public methods
		public void TriggerInteraction()
		{
			this.objectState = !this.objectState;
			
			foreach(GameObject cur in this.objectToShow)
			{
				cur.SetActive(this.objectState);
			}
			
			foreach(GameObject cur in this.objectsToHide)
			{
				cur.SetActive(!this.objectState);
			}
		}
		#endregion

		#region private methods

		#endregion
	}

}

