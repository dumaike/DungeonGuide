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
		public delegate void ObjectMovedHandler(MoveableRoot target, Vector3 oldPosition, Vector3 newPosition);
		public event ObjectMovedHandler objectMovedEvent;
		
		/// <summary>
		/// Fires when an moveable object is created
		/// </summary>
		public delegate void ObjectCreatedHandler(MoveableRoot target, Vector3 position);
		public event ObjectCreatedHandler objectCreated;
		
		/// <summary>
		/// Fires when an moveable object is removed from the scene
		/// </summary>
		public delegate void ObjectRemovedHandler(MoveableRoot target, Vector3 position);
		public event ObjectRemovedHandler objectRemoved;
		
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
	
		public EventCenter()
		{
		
		}	
		
		public void FireObjectMovedEvent(MoveableRoot target, Vector3 oldPosition, Vector3 newPosition)
		{
			if (this.objectMovedEvent != null)
			{
				this.objectMovedEvent(target, oldPosition, newPosition);
			}
		}	
		
		public void FireObjectCreatedEvent(MoveableRoot target, Vector3 position)
		{
			if (this.objectCreated != null)
			{
				this.objectCreated(target, position);
			}
		}	
		
		public void FireObjectRemovedEvent(MoveableRoot target, Vector3 position)
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
	}
}