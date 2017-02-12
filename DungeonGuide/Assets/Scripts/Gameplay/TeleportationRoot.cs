using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace DungeonGuide
{
	public class TeleportationRoot : MonoBehaviour
	{			
		[SerializeField]		
		private GameObject destinationObject;
			
		#region initializers		
		protected void Start()
		{
			SceneManager.eventCtr.objectMovedEvent += HandleCharacterMovedEvent;
		}
		
		protected void OnDestroy()
		{			
			if (SceneManager.eventCtr != null)
			{
				SceneManager.eventCtr.objectMovedEvent -= HandleCharacterMovedEvent;
			}
		}
		#endregion

		#region public methods
		
		#endregion
		
		#region public methods
		
		#endregion
		
		#region private methods
		private void HandleCharacterMovedEvent (MoveableEntity target, Vector3 oldPosition, Vector3 newPosition)
		{			
			Vector3 oldPositionFlat = oldPosition;
			oldPositionFlat.y = 0;
			
			Vector3 newPositionFlat = newPosition;
			newPositionFlat.y = 0;
			
			Vector3 thisPositionFlat = this.transform.position;
			thisPositionFlat.y = 0;
						
			//If they moved into our spot and used to be in a spot next to us, teleport!
			if ((newPositionFlat - thisPositionFlat).magnitude < 1 && 
				(oldPositionFlat - thisPositionFlat).magnitude < 2)
			{
				target.transform.position = destinationObject.transform.position;
				
				//Fire another event to let everyone know we moved somewhere else
				SceneManager.eventCtr.FireObjectMovedEvent(target, newPosition, destinationObject.transform.position);

				SceneManager.eventCtr.FireObjectSelected(target, false);
			}
		}
		#endregion

		#region private methods

		#endregion
	}

}

