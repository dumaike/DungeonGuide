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

		private Vector3 desiredCharacterPosition;
        private Vector3 lastMousePosition;

		private InputMode currentMode = InputMode.CHARACTERS;

		[SerializeField]
		private Text intputModeButton;

		#region initializers
		private void Awake()
		{

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
			if (this.currentMode == InputMode.CHARACTERS)
			{
				UpdateCharacterMovement ();
			}
			else if (this.currentMode == InputMode.CAMERA)
			{
				UpdateCameraMovement ();
			}
        }

		private void UpdateCharacterMovement()
		{
			//If a character was clicked
			if (Input.GetMouseButtonDown(0))
			{
				ResetActionsInProgress();

				Ray raycastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				this.lastMousePosition = raycastRay.origin;
				RaycastHit hitInfo = new RaycastHit();
				if (Physics.Raycast(raycastRay, out hitInfo))
				{
					this.selectedCharacter = hitInfo.transform.GetComponentInParent<CharacterRoot>();  
					if (this.selectedCharacter != null)
					{
						this.selectedCharacter.CharacterSelected(true);
						this.desiredCharacterPosition = this.selectedCharacter.transform.position;
					}
				}
			}

			//If a character was released
			if (this.selectedCharacter != null && Input.GetMouseButtonUp(0))
			{		
				Vector3 snappedCharacterPosition = this.desiredCharacterPosition;
				snappedCharacterPosition.x = (float)Math.Round(snappedCharacterPosition.x);
				snappedCharacterPosition.z = (float)Math.Round(snappedCharacterPosition.z);
				snappedCharacterPosition.y = 0;				
				this.selectedCharacter.transform.position = snappedCharacterPosition;
				
				this.selectedCharacter.CharacterSelected(false);
				this.selectedCharacter = null;
			}

			//If we're holding a character
			if (this.selectedCharacter != null)
			{
				int layerMask = 1 << 0;
				
				Vector3 newMousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
				Vector3 mousePositionDelta = newMousePosition - this.lastMousePosition;
				this.lastMousePosition = newMousePosition;
				
				this.desiredCharacterPosition = this.desiredCharacterPosition + mousePositionDelta;
				Vector3 snappedCharacterPosition = this.desiredCharacterPosition;
				snappedCharacterPosition.x = (float)Math.Round(snappedCharacterPosition.x);
				snappedCharacterPosition.z = (float)Math.Round(snappedCharacterPosition.z);
				snappedCharacterPosition.y = 0;
				
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
		}
		
		private void UpdateCameraMovement()
		{
			if (Input.GetMouseButtonDown(0))
			{
				ResetActionsInProgress();
				
				this.lastMousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
			}
			
			if(Input.GetMouseButton(0))
			{
				Vector3 newMousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
				Vector3 mousePositionDelta = newMousePosition - this.lastMousePosition;
				//Include the delta in the new mouse position because we're moving the camera too
				this.lastMousePosition = newMousePosition - mousePositionDelta;
				
				//Transform screen coords into world coords
				Camera.main.transform.position -= mousePositionDelta;
			}
		}

		private void ResetActionsInProgress()
		{			
			this.selectedCharacter = null;
		}
		#endregion
	}

}

