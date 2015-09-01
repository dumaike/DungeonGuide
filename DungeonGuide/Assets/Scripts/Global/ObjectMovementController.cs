using System;
using UnityEngine;

namespace DungeonGuide
{
	//TODO Factor all character movement out of user input controller and into here
	public class ObjectMovementController
	{
		private Vector3 desiredWorldCharacterPosition;
		private Vector3 lastWorldMousePosition;

		private ContextMenu contextMenuObject;
		private MoveableEntity selectedObject;

		#region initializers
		public ObjectMovementController(ContextMenu contextMenu)
		{
			this.contextMenuObject = contextMenu;

			SceneManager.eventCtr.objectSelected += HandleObjectSelected;
		}

		~ObjectMovementController()
		{
			SceneManager.eventCtr.objectSelected -= HandleObjectSelected;
		}
		#endregion

		#region private methods

		public void Update()
		{
			if (!this.contextMenuObject.IsContextMenuActive() &&
				UserInputController.currentMode == InputMode.CHARACTERS)
			{
				UpdateObjectMovementAndSelection();
			}
		}

		private void HandleObjectSelected(MoveableEntity target, bool selected)
		{
			this.selectedObject = !selected ? null : target;
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
				}
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
					finalObjectPosition.x = (float)Math.Round(finalObjectPosition.x);
					finalObjectPosition.z = (float)Math.Round(finalObjectPosition.z);
					finalObjectPosition.y = this.selectedObject.transform.position.y;
				}
				else if (snappableRoot.snapType == SnapType.CORNER)
				{
					finalObjectPosition.x = (float)Math.Round(finalObjectPosition.x - 0.5f) + 0.5f;
					finalObjectPosition.z = (float)Math.Round(finalObjectPosition.z - 0.5f) + 0.5f;
					finalObjectPosition.y = this.selectedObject.transform.position.y;
				}
			}

			Vector3 currentCharacterPosition = this.selectedObject.transform.position;
			Vector3 movementDirection = finalObjectPosition - currentCharacterPosition;
			float distanceToMove = movementDirection.magnitude;
			if (distanceToMove > 0)
			{
				Ray raycastRay = new Ray(currentCharacterPosition, movementDirection);
				RaycastHit hitInfo = new RaycastHit();

				if (!Physics.Raycast(raycastRay, out hitInfo, distanceToMove, layerMask) || this.selectedObject.freeMovement)
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

