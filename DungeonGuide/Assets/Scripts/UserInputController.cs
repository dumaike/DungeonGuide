using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace DungeonGuide
{
	public class UserInputController : MonoBehaviour
	{
		private enum InputMode
		{
			CAMERA,
			CHARACTERS,
		};

		private CharacterRoot selectedCharacter;
		private Vector3 selectedCharacterStartPosition;

		private Vector3 desiredWorldCharacterPosition;
        private Vector3 lastWorldMousePosition;
        
        private bool longPressActive = false;
        private Vector3 pressedMousePositionScreen;
		private float mousePressedTime;
		private bool mousePressed = false;

		private InputMode currentMode = InputMode.CHARACTERS;
		
		private float longPressDuration = 1.0f;
		private float longPressMinimumMovement;

		[SerializeField]
		private Text intputModeButton;	
		
		[SerializeField]
		private LongPressMenu longPressMenu;	

		#region initializers
		private void Awake()
		{
			this.longPressMinimumMovement = Screen.width / 100.0f;
			Log.Print("The long press minimum movement is " + this.longPressMinimumMovement + " pixels.", LogChannel.INPUT, this);
		}

		private void Start()
		{

		}

		private void OnDestroy()
		{

		}
		#endregion

		#region public methods
		public void ToggleInputMode()
		{
			if (this.currentMode == InputMode.CAMERA)
			{
				this.currentMode = InputMode.CHARACTERS;
				this.intputModeButton.text = "Character Mode";
			}
			else
			{
				this.currentMode = InputMode.CAMERA;
				this.intputModeButton.text = "Camera Mode";
			}
		}

		public void CameraZoom(float amount)
		{
			float newZoom = Camera.main.orthographicSize + amount;
			if (newZoom <= 0) 
			{
				newZoom = Camera.main.orthographicSize;
			}

			//Transform screen coords into world coords
			Camera.main.orthographicSize = newZoom;
		}
		#endregion

		#region private methods
        private void Update()
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
			else if (this.longPressActive && this.longPressMenu.gameObject.activeSelf == false)
			{
				this.longPressActive = false;
				ResetActionsInProgress();
			}	
        } 
        
        private void UpdateLongPressCheck()
        {
			//If the mouse was pressed
			if (Input.GetMouseButtonDown(0))
			{
				Log.Print("Tracking long press.", LogChannel.INPUT, this);
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
					Log.Print("Stopped tracking long press. Mouse moved too far.", LogChannel.INPUT, this);
					this.mousePressed = false;
				}				
				//If the amount of time has passed, trigger the long press UI
				else if (this.mousePressed && Time.time - this.mousePressedTime > this.longPressDuration)
				{
					Log.Print("Log press activated", LogChannel.INPUT, this);
					this.longPressActive = true;
					this.longPressMenu.DisplayLongPressMenu(true);
				}
				else if (Input.GetMouseButtonUp(0))
				{
					Log.Print("Stopped tracking long press. Mouse released", LogChannel.INPUT, this);
					this.mousePressed = false;
				}
			}
        }

		private void UpdateCharacterMovement()
		{
			//If a character was clicked
			if (Input.GetMouseButtonDown(0) && selectedCharacter == null)
			{
				ResetActionsInProgress();

				Ray raycastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				this.lastWorldMousePosition = raycastRay.origin;
				this.pressedMousePositionScreen = this.lastWorldMousePosition;
				
				RaycastHit hitInfo = new RaycastHit();
				if (Physics.Raycast(raycastRay, out hitInfo))
				{
					this.selectedCharacter = hitInfo.transform.GetComponentInParent<CharacterRoot>();  
					if (this.selectedCharacter != null)
					{
						this.selectedCharacter.CharacterSelected(true);
						this.desiredWorldCharacterPosition = this.selectedCharacter.transform.position;
					}
				}
			}

			//If a character was released
			if (this.selectedCharacter != null && Input.GetMouseButtonUp(0))
			{		
				ResetActionsInProgress();
			}
			
			if (this.selectedCharacter != null)
			{
				MoveSelectedCharacterToMouse();
			}
		}
		
		private void UpdateCameraMovement()
		{
			if (Input.GetMouseButtonDown(0))
			{
				ResetActionsInProgress();
				
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
			int layerMask = 1 << 0;
			
			Vector3 newMousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
			Vector3 mousePositionDelta = newMousePosition - this.lastWorldMousePosition;
			this.lastWorldMousePosition = newMousePosition;
			
			this.desiredWorldCharacterPosition = this.desiredWorldCharacterPosition + mousePositionDelta;
			Vector3 snappedCharacterPosition = this.desiredWorldCharacterPosition;
			snappedCharacterPosition.x = (float)Math.Round(snappedCharacterPosition.x);
			snappedCharacterPosition.z = (float)Math.Round(snappedCharacterPosition.z);
			snappedCharacterPosition.y = this.selectedCharacter.transform.position.y;
			
			Vector3 currentCharacterPosition = this.selectedCharacter.transform.position;
			Vector3 movementDirection = snappedCharacterPosition - currentCharacterPosition;
			float distanceToMove = movementDirection.magnitude;
			if (distanceToMove > 0)
			{
				Ray raycastRay = new Ray (currentCharacterPosition + movementDirection.normalized*CharacterVisionController.HALF_TILE_WIDTH, movementDirection);
				RaycastHit hitInfo = new RaycastHit ();
				
				if (!Physics.Raycast (raycastRay, out hitInfo, distanceToMove, layerMask))
				{
					Log.Print("Moving character from " + this.selectedCharacter.transform.position + " to " + snappedCharacterPosition, 
					          LogChannel.CHARACTER_MOVEMENT);
					
					this.selectedCharacter.transform.position = snappedCharacterPosition;
				}
				else
				{
					Log.Print("Can't move because we hit a " + hitInfo.transform.name + " when trying to move to " 
					          + snappedCharacterPosition + " from " + this.selectedCharacter.transform.position, 
					          LogChannel.CHARACTER_MOVEMENT, hitInfo.transform.gameObject);
				}
			}
		}		

		private void ResetActionsInProgress()
		{			
			if (this.selectedCharacter != null)
			{
				this.selectedCharacter.CharacterSelected(false);
				this.selectedCharacter = null;
			}
		}
		#endregion
	}

}

