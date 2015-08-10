using UnityEngine;
using System.Collections;
using System;

namespace DungeonGuide
{
	public class EventCenter
	{
		/// <summary>
		/// Fires when any object moves
		/// </summary>
		public delegate void ObjectMovedHandler(MoveableEntity target, Vector3 oldPosition, Vector3 newPosition);
		public event ObjectMovedHandler objectMovedEvent;
		
		/// <summary>
		/// Fires when an moveable object is created
		/// </summary>
		public delegate void ObjectCreatedHandler(MoveableEntity target, Vector3 position);
		public event ObjectCreatedHandler objectCreated;

		/// <summary>
		/// Fires when an moveable object is removed from the scene
		/// </summary>
		public delegate void ObjectRemovedHandler(MoveableEntity target, Vector3 position);
		public event ObjectRemovedHandler objectRemoved;

		/// <summary>
		/// Fires when a moveable object is selected
		/// </summary>
		public delegate void ObjectSelectedHandler(MoveableEntity target, bool selected);
		public event ObjectSelectedHandler objectSelected;

		/// <summary>
		/// Fires when an interactive object is toggled (door open, secret door opened, etc)
		/// </summary>
		public delegate void InteractiveObjectToggledHandler();
		public event InteractiveObjectToggledHandler interactiveObjectToggeled;
		
		/// <summary>
		/// Fires when an interactive object is toggled (door open, secret door opened, etc)
		/// </summary>
		public delegate void CameraZoomHandler();
		public event CameraZoomHandler cameraZoomed;
		
		/// <summary>
		/// Fires when a menu item is clicked
		/// </summary>
		public delegate void MenuItemClickedHandler();
		public event MenuItemClickedHandler menuItemClicked;
	
		public EventCenter()
		{
		
		}	
		
		public void FireObjectMovedEvent(MoveableEntity target, Vector3 oldPosition, Vector3 newPosition)
		{
			if (this.objectMovedEvent != null)
			{
				this.objectMovedEvent(target, oldPosition, newPosition);
			}
		}	
		
		public void FireObjectCreatedEvent(MoveableEntity target, Vector3 position)
		{
			if (this.objectCreated != null)
			{
				this.objectCreated(target, position);
			}
		}	
		
		public void FireObjectRemovedEvent(MoveableEntity target, Vector3 position)
		{
			if (this.objectRemoved != null)
			{
				this.objectRemoved(target, position);
			}
		}	
		
		public void FireInteractiveObjectToggledEvent()
		{
			if (this.interactiveObjectToggeled != null)
			{
				this.interactiveObjectToggeled();
			}
		}	
		
		public void FireCameraZoomedEvent()
		{
			if (this.cameraZoomed != null)
			{
				this.cameraZoomed();
			}
		}

		public void FireMenuItemClickedEvent()
		{
			if (this.menuItemClicked != null)
			{
				this.menuItemClicked();
			}
		}

		public void FireObjectSelected(MoveableEntity target, bool selected)
		{
			if (this.objectSelected != null)
			{
				this.objectSelected(target, selected);
			}
		}
	}
}