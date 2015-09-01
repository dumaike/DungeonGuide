using UnityEngine;
using System;
using UnityEngine.UI;

namespace DungeonGuide
{
	public enum InputMode
	{
		CAMERA,
		CHARACTERS,
	};

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

		public static InputMode currentMode = InputMode.CHARACTERS;
		
		private const float ZOOM_INCREMENT = 0.5f;
		
		private readonly Text intputModeButtonText;

		private readonly ContextMenu contextMenuObject;

		private float mousePressedTime;

		private Vector3 lastWorldMousePosition;

		#region initializers
		public UserInputController(Text inputModeButtonText, ContextMenu contextMenu)
		{
			this.intputModeButtonText = inputModeButtonText;
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
				case InputEvent.TOGGLE_INPUT_MODE:
					ToggleInputMode();
					break;
				case InputEvent.ZOOM_IN:
					CameraZoom(-ZOOM_INCREMENT, SceneManager.gameplayCam);
					CameraZoom(-ZOOM_INCREMENT, SceneManager.visionCam);
					break;
				case InputEvent.ZOOM_OUT:
					CameraZoom(ZOOM_INCREMENT, SceneManager.gameplayCam);
					CameraZoom(ZOOM_INCREMENT, SceneManager.visionCam);
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
				if (!this.contextMenuObject.IsContextMenuActive() &&
					currentMode == InputMode.CAMERA)
				{
					UpdateCameraMovement ();
				}
			}		
		}
		#endregion

		#region private methods

		private void HandleMenuItemClicked()
		{
			this.mousePressedTime = float.MinValue;
		}

		private void ToggleInputMode()
		{		
			if (currentMode == InputMode.CAMERA)
			{
				currentMode = InputMode.CHARACTERS;
				this.intputModeButtonText.text = "Character Mode";
			}
			else
			{
				currentMode = InputMode.CAMERA;
				this.intputModeButtonText.text = "Camera Mode";
			}
		}
		
		private void CameraZoom(float amount, Camera camera)
		{
			float newZoom = camera.orthographicSize + amount;
			if (newZoom <= 0) 
			{
				newZoom = camera.orthographicSize;
			}
			
			//Transform screen coords into world coords
			camera.orthographicSize = newZoom;
			SceneManager.eventCtr.FireCameraZoomedEvent();
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
			if (Input.GetMouseButtonDown(0))
			{				
				this.lastWorldMousePosition = SceneManager.gameplayCam.ScreenPointToRay(Input.mousePosition).origin;
			}
			
			if(Input.GetMouseButton(0))
			{
				Vector3 newMousePosition = SceneManager.gameplayCam.ScreenPointToRay(Input.mousePosition).origin;
				Vector3 mousePositionDelta = newMousePosition - this.lastWorldMousePosition;
				//Include the delta in the new mouse position because we're moving the camera too
				this.lastWorldMousePosition = newMousePosition - mousePositionDelta;
				
				//Transform screen coords into world coords
				SceneManager.gameplayCam.transform.position -= mousePositionDelta;
				SceneManager.visionCam.transform.position -= mousePositionDelta;
				
				SceneManager.eventCtr.FireCameraZoomedEvent();
			}
		}
		#endregion
	}

}

