using UnityEngine;
using System;
using UnityEngine.UI;

namespace DungeonGuide
{
	public class UserInputController
	{			
		private const float UI_HEIGHT = 5.0f;
	
		public enum InputEvent
		{
			ZOOM_IN = 0,
			ZOOM_OUT = 1,
			TOGGLE_INPUT_MODE = 2
		}

		private const float DOUBLE_CLICK_DURATION = 0.2f;
		
		private const float ZOOM_INCREMENT = 0.5f;

		/// <summary>
		/// The smaller the number, the more parallel we require two touch movement
		/// to be to count as a camera movement. 0 is perfectly parallel, 2 is ever
		/// opposite direction touches count.
		/// </summary>
		private const float PARALLEL_TOUCH_THRESHOLD = 0.4f;

		private readonly ContextMenu contextMenuObject;

		private float mousePressedTime;

		private Vector3 lastTouch1Position;
		private Vector3 lastTouch2Position;
		private bool processingMultitouch = false;

		#region initializers
		public UserInputController(ContextMenu contextMenu)
		{
			this.contextMenuObject = contextMenu;

			SceneManager.eventCtr.menuItemClicked += HandleMenuItemClicked;
		}

		~UserInputController()
		{
			SceneManager.eventCtr.menuItemClicked -= HandleMenuItemClicked;
		}
		#endregion

		#region public methods		

		public void ReceiveInputEvent(InputEvent inputEvent)
		{
			HandleMenuItemClicked();

			switch (inputEvent)
			{
				case InputEvent.ZOOM_IN:
					UpdateCameraZoom(-ZOOM_INCREMENT, SceneManager.gameplayCam);
					UpdateCameraZoom(-ZOOM_INCREMENT, SceneManager.visionCam);
					break;
				case InputEvent.ZOOM_OUT:
					UpdateCameraZoom(ZOOM_INCREMENT, SceneManager.gameplayCam);
					UpdateCameraZoom(ZOOM_INCREMENT, SceneManager.visionCam);
					break;
				default:
					Log.Error("No action mapped for input event: " + inputEvent);
					break;
			}
		}

		public void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				Log.Print("Mouse 0 Down", LogChannel.INPUT);
			}
			if (Input.GetMouseButtonUp(0))
			{
				Log.Print("Mouse 0 Up", LogChannel.INPUT);
			}
			if (Input.GetMouseButtonDown(1))
			{
				Log.Print("Mouse 1 Down", LogChannel.INPUT);
			}
			if (Input.GetMouseButtonUp(1))
			{
				Log.Print("Mouse 1 Up", LogChannel.INPUT);
			}
		
			if (!this.contextMenuObject.IsContextMenuActive())
			{
				//If the context menu isn't active, check to see if it should be
				UpdateContextMenuCheck();
				
				//If it's still not active, preform selection or movement
				if (!this.contextMenuObject.IsContextMenuActive())
				{
					UpdateCameraMovement ();
					//UpdateCameraZoom(0, SceneManager.gameplayCam);
					//UpdateCameraZoom(0, SceneManager.visionCam);
				}
			}		
		}
		#endregion

		#region private methods

		private void HandleMenuItemClicked()
		{
			this.mousePressedTime = float.MinValue;
		}

		private void UpdateCameraZoom(float amount, Camera camera)
		{
			float zoomAmount = amount;
			if (zoomAmount == 0)
			{
				zoomAmount = GetCameraZoomDelta();
			}

			float newZoom = camera.orthographicSize + zoomAmount;
			if (newZoom <= 0) 
			{
				newZoom = camera.orthographicSize;
			}
			
			//Transform screen coords into world coords
			camera.orthographicSize = newZoom;
			SceneManager.eventCtr.FireCameraUpdateEvent();
		}

		/// <summary>
		/// The distance the camera should zoom in/out this frame
		/// </summary>
		/// <returns></returns>
		private float GetCameraZoomDelta()
		{
			//If there are exactly two touches, start camera movement.
			/*if (Input.touchCount == 2)
			{
				Log.Print("Two touches!");

				Touch touch1 = Input.GetTouch(0);
				Touch touch2 = Input.GetTouch(1);

				if (!this.processingMultitouch)
				{
					this.processingMultitouch = true;
					this.lastTouch1Position =
						SceneManager.gameplayCam.ScreenPointToRay(touch1.position).origin;
					this.lastTouch2Position =
						SceneManager.gameplayCam.ScreenPointToRay(touch2.position).origin;
				}

				Vector3 newTouch1 =
						SceneManager.gameplayCam.ScreenPointToRay(touch1.position).origin;
				Vector3 newTouch2 =
						SceneManager.gameplayCam.ScreenPointToRay(touch2.position).origin;

				float touchDistanceDelta = 
					((newTouch1 - newTouch2).magnitude - (this.lastTouch1Position - this.lastTouch2Position).magnitude)*2000;

				this.lastTouch1Position = newTouch1;
				this.lastTouch2Position = newTouch2;

				Log.Print("Touch dist: " + touchDistanceDelta);

				return touchDistanceDelta;
			}
			else
			{
				this.processingMultitouch = false;
			}*/
			
			float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
			return mouseScrollWheel;
		}

		private void UpdateContextMenuCheck()
        {
			//If the mouse was pressed
	        if (!Input.GetMouseButtonUp(0))
	        {
		        return;
	        }

	        //See if a valid double click time has passed (not two slow and not twice this frame)
	        float timeSinceLastClick = Time.time - this.mousePressedTime;
	        if (timeSinceLastClick < UserInputController.DOUBLE_CLICK_DURATION &&
	            timeSinceLastClick > 0)
	        {
		        DisplayContextMenu();
	        }
				
	        Log.Print("Tracking context menu click.", LogChannel.INPUT);
	        this.mousePressedTime = Time.time;
        }
        
        private void DisplayContextMenu()
        {			
			Log.Print("Log press activated", LogChannel.INPUT);
			
			Vector3 mouseLocationInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 snappedLocation = mouseLocationInWorld;
			snappedLocation.x = (float)Math.Round(snappedLocation.x);
			snappedLocation.z = (float)Math.Round(snappedLocation.z);
			snappedLocation.y = UserInputController.UI_HEIGHT;
			this.contextMenuObject.transform.position = snappedLocation;
			
			snappedLocation.y = 0;
			mouseLocationInWorld.y = 0;
			this.contextMenuObject.DisplayContextMenu(snappedLocation, mouseLocationInWorld);
        }
		
		private void UpdateCameraMovement()
		{
			Vector3 cameraMovement = GetCameraMovementDelta();

			if (cameraMovement != Vector3.zero)
			{
				//Transform screen coords into world coords
				SceneManager.gameplayCam.transform.position -= cameraMovement;
				SceneManager.visionCam.transform.position -= cameraMovement;
				
				SceneManager.eventCtr.FireCameraUpdateEvent();
			}
		}

		/// <summary>
		/// The distance the camera should pan move this frame
		/// </summary>
		/// <returns></returns>
		private Vector3 GetCameraMovementDelta()
		{
			//If there are exactly two touches, start camera movement.
			if (Input.touchCount == 2)
			{
				Touch touch1 = Input.GetTouch(0);
				Touch touch2 = Input.GetTouch(1);

				if (!this.processingMultitouch)
				{
					this.processingMultitouch = true;
					this.lastTouch1Position =
						SceneManager.gameplayCam.ScreenPointToRay(touch1.position).origin;
					this.lastTouch2Position =
						SceneManager.gameplayCam.ScreenPointToRay(touch2.position).origin;					
				}
				
				Vector3 newTouch1 =
						SceneManager.gameplayCam.ScreenPointToRay(touch1.position).origin;
				Vector3 newTouch2 = 
						SceneManager.gameplayCam.ScreenPointToRay(touch2.position).origin;

				Vector3 touch1Delta = newTouch1 - this.lastTouch1Position;
				Vector3 touch2Delta = newTouch2 - this.lastTouch2Position;

				Vector3 averageTouchDelta = (touch1Delta + touch2Delta) / 2;

				this.lastTouch1Position = newTouch1 - averageTouchDelta;
				this.lastTouch2Position = newTouch2 - averageTouchDelta;
				
				return averageTouchDelta;
			}
			else
			{
				this.processingMultitouch = false;
			}

			//We just grabbed right click, so get the "starting" position of the mouse as a touch 1
			if (Input.GetMouseButtonDown(1))
			{
				this.lastTouch1Position =
					SceneManager.gameplayCam.ScreenPointToRay(Input.mousePosition).origin;
			}

			//The right mouse is down, so grab the delta between this and the previous position
			if (Input.GetMouseButton(1))
			{
				Vector3 newTouchPosition =
					SceneManager.gameplayCam.ScreenPointToRay(Input.mousePosition).origin;
				Vector3 returnVector = newTouchPosition - this.lastTouch1Position;

				//Update the last pos plus the expected camera movement
				this.lastTouch1Position = newTouchPosition - returnVector;
				
				return returnVector;
			}
			
			return Vector3.zero;
		}
		#endregion
	}

}

