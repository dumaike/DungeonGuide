using UnityEngine;
using System.Collections;
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
	
		private enum InputMode
		{
			CAMERA,
			CHARACTERS,
		};

		private Vector3 selectedCharacterStartPosition;

		private Vector3 desiredWorldCharacterPosition;
        private Vector3 lastWorldMousePosition;
        
        private bool longPressActive = false;
        private Vector3 pressedMousePositionScreen;
		private float mousePressedTime;
		private bool mousePressed = false;
		private float longPressMinimumMovement;

		private InputMode currentMode = InputMode.CHARACTERS;
		
		private const float LONG_PRESS_DURATION = 0.5f;
		private const float ZOOM_INCREMENT = 0.5f;
		
		/// <summary>
		/// The amount we'll allow the user to move in pixels and still count
		/// that click as a long press, expressed in a percent of the screen
		/// width.
		/// </summary>
		private const float LONG_PRESS_MOVEMENT_TOLERANCE = 0.01f;
		
		private Text intputModeButtonText;	
		private LongPressMenu longPressMenuObject;			

		#region initializers
		public UserInputController(Text inputModeButtonText, LongPressMenu longPressMenuObject)
		{
			this.intputModeButtonText = inputModeButtonText;
			this.longPressMenuObject = longPressMenuObject;
			
			this.longPressMinimumMovement = Screen.width * LONG_PRESS_MOVEMENT_TOLERANCE;
			Log.Print("The long press minimum movement is " + this.longPressMinimumMovement + " pixels.", LogChannel.INPUT);
		}
		#endregion

		#region public methods
		public void ReceiveInputEvent(InputEvent inputEvent)
		{
			switch (inputEvent)
			{
				case InputEvent.TOGGLE_INPUT_MODE:
					ToggleInputMode();
					break;
				case InputEvent.ZOOM_IN:
					CameraZoom(-ZOOM_INCREMENT);
					break;
				case InputEvent.ZOOM_OUT:
					CameraZoom(ZOOM_INCREMENT);
					break;
				default:
					Log.Error("No action mapped for input event: " + inputEvent);
					break;
			}
		}
		
		public void Update()
		{
			if (!this.longPressActive)
			{
				if (this.currentMode == InputMode.CHARACTERS)
				{
					UpdateCharacterMovement ();
				}
				else if (this.currentMode == InputMode.CAMERA)
				{
					UpdateCameraMovement ();
				}
				
				UpdateLongPressCheck();
			}		
			else if (this.longPressActive && this.longPressMenuObject.gameObject.activeSelf == false)
			{
				this.longPressActive = false;
			}	
		} 
		#endregion

		#region private methods
		private void ToggleInputMode()
		{
			if (this.currentMode == InputMode.CAMERA)
			{
				this.currentMode = InputMode.CHARACTERS;
				this.intputModeButtonText.text = "Character Mode";
			}
			else
			{
				this.currentMode = InputMode.CAMERA;
				this.intputModeButtonText.text = "Camera Mode";
			}
		}
		
		private void CameraZoom(float amount)
		{
			float newZoom = Camera.main.orthographicSize + amount;
			if (newZoom <= 0) 
			{
				newZoom = Camera.main.orthographicSize;
			}
			
			//Transform screen coords into world coords
			Camera.main.orthographicSize = newZoom;
		}
		
        private void UpdateLongPressCheck()
        {
			//If the mouse was pressed
			if (Input.GetMouseButtonDown(0))
			{
				Log.Print("Tracking long press.", LogChannel.INPUT);
				this.pressedMousePositionScreen = Input.mousePosition;
				this.mousePressedTime = Time.time;
				this.mousePressed = true;
			}
			
			//If a mouse was released
			if (this.mousePressed)
			{	
				//If we move the mouse too much, just stop tracking for long press
				Vector3 mousePositionDelta = Input.mousePosition - this.pressedMousePositionScreen;
				if (mousePositionDelta.magnitude > this.longPressMinimumMovement)
				{
					Log.Print("Stopped tracking long press. Mouse moved too far.", LogChannel.INPUT);
					this.mousePressed = false;
				}				
				//If the amount of time has passed, trigger the long press UI
				else if (this.mousePressed && Time.time - this.mousePressedTime > UserInputController.LONG_PRESS_DURATION)
				{
					Log.Print("Log press activated", LogChannel.INPUT);
					this.longPressActive = true;
					
					Vector3 desiredLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					desiredLocation.x = (float)Math.Round(desiredLocation.x);
					desiredLocation.z = (float)Math.Round(desiredLocation.z);
					desiredLocation.y = UserInputController.UI_HEIGHT;
					this.longPressMenuObject.transform.position = desiredLocation;
										
					desiredLocation.y = 0;
					this.longPressMenuObject.DisplayLongPressMenu(desiredLocation);
				}
				else if (Input.GetMouseButtonUp(0))
				{
					Log.Print("Stopped tracking long press. Mouse released", LogChannel.INPUT);
					this.mousePressed = false;
				}
			}
        }

		private void UpdateCharacterMovement()
		{
			//If a character was clicked
			if (Input.GetMouseButtonDown(0) && !SceneManager.SelectedChCtrl.IsCharacterSelected())
			{				
				Ray raycastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				this.lastWorldMousePosition = raycastRay.origin;
				this.pressedMousePositionScreen = this.lastWorldMousePosition;
				
				RaycastHit hitInfo = new RaycastHit();
				if (Physics.Raycast(raycastRay, out hitInfo))
				{
					CharacterRoot hitCharacter = hitInfo.transform.GetComponentInParent<CharacterRoot>();
					if (hitCharacter != null)
					{
						SceneManager.SelectedChCtrl.SelectCharacter(hitCharacter);  
						this.desiredWorldCharacterPosition = hitCharacter.transform.position;
					}
				}
			}

			//If a character was released
			if (SceneManager.SelectedChCtrl.IsCharacterSelected() && Input.GetMouseButtonUp(0))
			{		
				SceneManager.SelectedChCtrl.DeselectCharacter();
			}
			
			if (SceneManager.SelectedChCtrl.IsCharacterSelected())
			{
				MoveSelectedCharacterToMouse();
			}
		}
		
		private void UpdateCameraMovement()
		{
			if (Input.GetMouseButtonDown(0))
			{				
				this.lastWorldMousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
			}
			
			if(Input.GetMouseButton(0))
			{
				Vector3 newMousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
				Vector3 mousePositionDelta = newMousePosition - this.lastWorldMousePosition;
				//Include the delta in the new mouse position because we're moving the camera too
				this.lastWorldMousePosition = newMousePosition - mousePositionDelta;
				
				//Transform screen coords into world coords
				Camera.main.transform.position -= mousePositionDelta;
			}
		}
		
		private void MoveSelectedCharacterToMouse()
		{
			CharacterRoot selectedCharacter = SceneManager.SelectedChCtrl.GetSelectedCharacter();
		
			int layerMask = 1 << 0;
			
			Vector3 newMousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
			Vector3 mousePositionDelta = newMousePosition - this.lastWorldMousePosition;
			this.lastWorldMousePosition = newMousePosition;
			
			this.desiredWorldCharacterPosition = this.desiredWorldCharacterPosition + mousePositionDelta;
			Vector3 snappedCharacterPosition = this.desiredWorldCharacterPosition;
			snappedCharacterPosition.x = (float)Math.Round(snappedCharacterPosition.x);
			snappedCharacterPosition.z = (float)Math.Round(snappedCharacterPosition.z);
			snappedCharacterPosition.y = selectedCharacter.transform.position.y;
			
			Vector3 currentCharacterPosition = selectedCharacter.transform.position;
			Vector3 movementDirection = snappedCharacterPosition - currentCharacterPosition;
			float distanceToMove = movementDirection.magnitude;
			if (distanceToMove > 0)
			{
				Ray raycastRay = new Ray (currentCharacterPosition + movementDirection.normalized*CharacterVisionController.HALF_TILE_WIDTH, movementDirection);
				RaycastHit hitInfo = new RaycastHit ();
				
				if (!Physics.Raycast (raycastRay, out hitInfo, distanceToMove, layerMask))
				{
					Log.Print("Moving character from " + selectedCharacter.transform.position + " to " + snappedCharacterPosition, 
					          LogChannel.CHARACTER_MOVEMENT);
					
					selectedCharacter.transform.position = snappedCharacterPosition;
				}
				else
				{
					Log.Print("Can't move because we hit a " + hitInfo.transform.name + " when trying to move to " 
					          + snappedCharacterPosition + " from " + selectedCharacter.transform.position, 
					          LogChannel.CHARACTER_MOVEMENT, hitInfo.transform.gameObject);
				}
			}
		}		
		#endregion
	}

}

