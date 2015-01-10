using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DungeonGuide
{
	public class InteractiveObjectController
	{
		private const float INTERACT_RADIUS = 1.25f;
		
		private List<InteractiveRoot> reavealableObjects = new List<InteractiveRoot>();
		private List<InteractiveRoot> toggleableObjects = new List<InteractiveRoot>();

		#region initializers
		public InteractiveObjectController()
		{						
			List<InteractiveRoot> allInteractive = new List<InteractiveRoot>(GameObject.FindObjectsOfType<InteractiveRoot>());
			foreach (InteractiveRoot interactiveObj in allInteractive)
			{
				if (interactiveObj.type == InteractiveRoot.InteractiveType.REVEALABLE)
				{
					this.reavealableObjects.Add(interactiveObj);
				}
				else if (interactiveObj.type == InteractiveRoot.InteractiveType.TOGGLEABLE)
				{
					this.toggleableObjects.Add(interactiveObj);
				}
				else
				{
					Log.Warning("There's a type of interactive object not being tracked by " + 
						" the InteractiveObjectController. It's type is " + interactiveObj.type);
				}
			}
		}
		#endregion

		#region public methods
		public void RevealAppropriateObjects(Vector3 revealLocation)
		{			
			List<InteractiveRoot> revealedList = 
				ToggleAppropriateObjects(revealLocation, this.reavealableObjects);
			
			//Removed revealed objects from the list for future checks
			foreach (InteractiveRoot revealed in revealedList)
			{
				this.reavealableObjects.Remove(revealed);
			}
		}
		
		public void ToggleAppropriateObjects(Vector3 toggleLocation)
		{
			ToggleAppropriateObjects(toggleLocation, this.toggleableObjects);
		}
		
		public bool IsTogglableObjectInRange(Vector3 toggleLocation)
		{			
			foreach (InteractiveRoot interactiveObj in this.toggleableObjects)
			{
				Vector3 distFromClick = interactiveObj.transform.position - toggleLocation;
				if (distFromClick.magnitude <= InteractiveObjectController.INTERACT_RADIUS)
				{
					return true;
				}
			}
			return false;
		}
		#endregion

		#region private methods
		
		public List<InteractiveRoot> ToggleAppropriateObjects(
			Vector3 interactLocation, List<InteractiveRoot> listToExamine)
		{
			//Find objects to reveal and reveal them
			List<InteractiveRoot> interactedWithList = new List<InteractiveRoot>();
			foreach (InteractiveRoot interactiveObj in listToExamine)
			{
				Vector3 distFromClick = interactiveObj.transform.position - interactLocation;
				if (distFromClick.magnitude <= InteractiveObjectController.INTERACT_RADIUS)
				{
					interactiveObj.TriggerInteraction();
					interactedWithList.Add(interactiveObj);
				}
			}
			
			return interactedWithList;
		}
		#endregion
	}

}

