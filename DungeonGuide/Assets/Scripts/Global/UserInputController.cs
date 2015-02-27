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
        
		private float mousePressedTime;

		private InputMode currentMode = InputMode.CHARACTERS;
		
		private const float DOUBLE_CLICK_DURATION = 0.2f;
		private const float ZOOM_INCREMENT = 0.5f;
		
		/// <summary>
		/// The amount we'll allow the user to move in pixels and still count
		/// that click as a long press, expressed in a percent of the screen
		/// width.
		/// </summary>
		private const float LONG_PRESS_MOVEMENT_TOLERANCE = 0.01f;
		
		private Text intputModeButtonText;	
		private ContextMenu contextMenuObject;		

		#region initializers
		public UserInputController(Text inputModeButtonText, ContextMenu contextMenu)
		{
			this.intputModeButtonText = inputModeButtonText;
			this.contextMenuObject = contextMenu;
		}
		#endregion

		#region public methods
		
		public void MenuItemClicked()
		{
			this.mousePressedTime = float.MinValue;
		}		
		
		public int SelectedCharacterMovementAmount()
		{
			if (!SceneManager.selectedChCtrl.IsCharacterSelected())
			{
				return 0;
			}
		
			MoveableRoot selectedCharacter = SceneManager.selectedChCtrl.GetSelectedCharacter();
			
			return (int)(selectedCharacter.transform.position - this.selectedCharacterStartPosition).magnitude;
		}
		
		public void ReceiveInputEvent(InputEvent inputEvent)
		{
			MenuItemClicked();
		
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
				if (this.currentMode == InputMode.CHARACTERS)
				{
					UpdateCharacterMovement ();
				}
				else if (this.currentMode == InputMode.CAMERA)
				{
					UpdateCameraMovement ();
				}
				
				UpdateContextMenuCheck();
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
		
		private void CameraZoom(float amount, Camera camera)
		{
			float newZoom = camera.orthographicSize + amount;
			if (newZoom <= 0) 
			{
				newZoom = camera.orthographicSize;
			}
			
			//Transform screen coords into world coords
			camera.orthographicSize = newZoom;
			SceneManager.chVisionCtrl.UpdateVisionQuad();
		}
		
        private void UpdateContextMenuCheck()
        {
			//If the mouse was pressed
			if (Input.GetMouseButtonDown(0))
			{
				float timeSinceLastClick = Time.time - this.mousePressedTime;
				if (timeSinceLastClick < UserInputController.DOUBLE_CLICK_DURATION &&
					timeSinceLastClick != 0)
				{
					DisplayContextMenu();
				}
				
				Log.Print("Tracking context menue click.", LogChannel.INPUT);
				this.mousePressedTime = Time.time;
			}
			
			/*if (Input.GetMouseButtonDown(1))
			{
				SelectCharacterUnderMouse();
				DisplayContextMenu();
			}*/
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

		private void UpdateCharacterMovement()
		{
			//If a character was clicked
			if ((Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) && !SceneManager.selectedChCtrl.IsCharacterSelected())
			{				
				SelectCharacterUnderMouse();
			}

			//If a character was released
			if (SceneManager.selectedChCtrl.IsCharacterSelected() && (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)))
			{		
				SceneManager.selectedChCtrl.DeselectCharacter();
			}
			
			if (SceneManager.selectedChCtrl.IsCharacterSelected())
			{
				MoveSelectedCharacterToMouse();
			}
		}
		
		private void SelectCharacterUnderMouse()
		{
			Ray raycastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			this.lastWorldMousePosition = raycastRay.origin;
			
			RaycastHit hitInfo = new RaycastHit();
			if (Physics.Raycast(raycastRay, out hitInfo))
			{
				MoveableRoot hitCharacter = hitInfo.transform.GetComponentInParent<MoveableRoot>();
				if (hitCharacter != null)
				{
					SceneManager.selectedChCtrl.SelectCharacter(hitCharacter);  
					this.desiredWorldCharacterPosition = hitCharacter.transform.position;
					this.selectedCharacterStartPosition = this.desiredWorldCharacterPosition;
				}
			}
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
								
				SceneManager.chVisionCtrl.UpdateVisionQuad();
			}
		}
		
		private void MoveSelectedCharacterToMouse()
		{
			MoveableRoot selectedCharacter = SceneManager.selectedChCtrl.GetSelectedCharacter();
			
			int layerMask = (1 << LayerAccessor.DEFAULT) + (1 << LayerAccessor.BLOCKS_MOVEMENT);
			
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
				Ray raycastRay = new Ray (currentCharacterPosition, movementDirection);
				RaycastHit hitInfo = new RaycastHit ();

				if (!Physics.Raycast (raycastRay, out hitInfo, distanceToMove, layerMask) || selectedCharacter.freeMovement)
				{
					Log.Print("Moving character from " + selectedCharacter.transform.position + " to " + snappedCharacterPosition, 
					          LogChannel.CHARACTER_MOVEMENT);

					//Debug.DrawLine (raycastRay.origin, raycastRay.origin + raycastRay.direction*distanceToMove);
					Vector3 oldPosition = selectedCharacter.transform.position;
					selectedCharacter.transform.position = snappedCharacterPosition;
					
					//Fire an event to let everyone know we moved someone
					SceneManager.eventCtr.FireObjectMovedEvent(selectedCharacter, oldPosition, snappedCharacterPosition);
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

