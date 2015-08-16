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
		private MoveableEntity selectedObject;

		#region initializers
		public UserInputController(Text inputModeButtonText, ContextMenu contextMenu)
		{
			this.intputModeButtonText = inputModeButtonText;
			this.contextMenuObject = contextMenu;

			SceneManager.eventCtr.menuItemClicked += HandleMenuItemClicked;
			SceneManager.eventCtr.objectSelected += HandleObjectSelected;
		}

		~UserInputController()
		{						
			SceneManager.eventCtr.menuItemClicked -= HandleMenuItemClicked;
			SceneManager.eventCtr.objectSelected -= HandleObjectSelected;
		}
		#endregion

		#region public methods		
		
		public int SelectedCharacterMovementAmount()
		{
			if (this.selectedObject == null)
			{
				return 0;
			}		
			
			return (int)(selectedObject.transform.position - this.selectedCharacterStartPosition).magnitude;
		}
		
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
				if (!this.contextMenuObject.IsContextMenuActive())
				{
					if (this.currentMode == InputMode.CHARACTERS)
					{
						UpdateObjectMovementAndSelection ();
					}
					else if (this.currentMode == InputMode.CAMERA)
					{
						UpdateCameraMovement ();
					}
				}				
			}		
		} 
		#endregion

		#region private methods

		private void HandleMenuItemClicked()
		{
			this.mousePressedTime = float.MinValue;
		}

		private void HandleObjectSelected(MoveableEntity target, bool selected)
		{
			this.selectedObject = !selected ? null : target;
		}

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
				
	        Log.Print("Tracking context menue click.", LogChannel.INPUT);
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

		private void UpdateObjectMovementAndSelection()
		{
			//If a character was clicked
			if ((Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) && 
				this.selectedObject == null)
			{				
				SelectObjectUnderMouse();
			}

			//If a character was released
			if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) &&
				this.selectedObject != null)
			{
				this.selectedObject.CharacterSelected(false);
				SceneManager.eventCtr.FireObjectSelected(this.selectedObject, false);
			}
			
			if (this.selectedObject != null)
			{
				MoveSelectedObjectToMouse();
			}
		}
		
		private void SelectObjectUnderMouse()
		{
			Ray raycastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			this.lastWorldMousePosition = raycastRay.origin;
			
			//Only click default and non-blocking layers
			int layerMask = (1 << LayerAccessor.DEFAULT) + (1 << LayerAccessor.BLOCKS_NOTHING);
			RaycastHit hitInfo;
			if (Physics.Raycast(raycastRay, out hitInfo, 1000, layerMask))
			{
				MoveableEntity hitCharacter = hitInfo.transform.GetComponentInParent<MoveableEntity>();
				if (hitCharacter != null)
				{
					SceneManager.eventCtr.FireObjectSelected(hitCharacter, true);
					hitCharacter.CharacterSelected(true);
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
				
				SceneManager.eventCtr.FireCameraZoomedEvent();
			}
		}
		
		private void MoveSelectedObjectToMouse()
		{
			if (this.selectedObject == null)
			{
				Log.Error("You're trying to move a selected character, but there is none", 
					LogChannel.LOGIC);
				return;
			}

			int layerMask = (1 << LayerAccessor.DEFAULT) + 
				(1 << LayerAccessor.BLOCKS_MOVEMENT) + 
				(1 << LayerAccessor.BLOCKS_BOTH);
			
			Vector3 newMousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
			Vector3 mousePositionDelta = newMousePosition - this.lastWorldMousePosition;
			this.lastWorldMousePosition = newMousePosition;
			
			this.desiredWorldCharacterPosition = this.desiredWorldCharacterPosition + mousePositionDelta;
			Vector3 finalObjectPosition = this.desiredWorldCharacterPosition;

			SnappableRoot snappableRoot = this.selectedObject.GetComponent<SnappableRoot>();
			if (snappableRoot != null)
			{
				if (snappableRoot.snapType == SnapType.CENTER)
				{
					finalObjectPosition.x = (float) Math.Round(finalObjectPosition.x);
					finalObjectPosition.z = (float) Math.Round(finalObjectPosition.z);
					finalObjectPosition.y = this.selectedObject.transform.position.y;
				}
				else if (snappableRoot.snapType == SnapType.CORNER)
				{
					finalObjectPosition.x = (float) Math.Round(finalObjectPosition.x - 0.5f) + 0.5f;
					finalObjectPosition.z = (float) Math.Round(finalObjectPosition.z - 0.5f) + 0.5f;
					finalObjectPosition.y = this.selectedObject.transform.position.y;
				}
			}

			Vector3 currentCharacterPosition = this.selectedObject.transform.position;
			Vector3 movementDirection = finalObjectPosition - currentCharacterPosition;
			float distanceToMove = movementDirection.magnitude;
			if (distanceToMove > 0)
			{
				Ray raycastRay = new Ray (currentCharacterPosition, movementDirection);
				RaycastHit hitInfo = new RaycastHit ();

				if (!Physics.Raycast (raycastRay, out hitInfo, distanceToMove, layerMask) || this.selectedObject.freeMovement)
				{
					Log.Print("Moving character from " + this.selectedObject.transform.position + " to " + finalObjectPosition, 
					          LogChannel.CHARACTER_MOVEMENT);

					//Debug.DrawLine (raycastRay.origin, raycastRay.origin + raycastRay.direction*distanceToMove);
					Vector3 oldPosition = this.selectedObject.transform.position;
					this.selectedObject.transform.position = finalObjectPosition;
					
					//Fire an event to let everyone know we moved someone
					SceneManager.eventCtr.FireObjectMovedEvent(this.selectedObject, oldPosition, finalObjectPosition);
				}
				else
				{
					Log.Print("Can't move because we hit a " + hitInfo.transform.name + " when trying to move to " 
					          + finalObjectPosition + " from " + this.selectedObject.transform.position, 
					          LogChannel.CHARACTER_MOVEMENT, hitInfo.transform.gameObject);
				}
			}
		}		
		#endregion
	}

}

