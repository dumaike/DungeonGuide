using UnityEngine;
using System.Collections;
using System;

namespace DungeonGuide
{
	public class UserInputController : MonoBehaviour
	{
		private CharacterRoot selectedCharacter;

		private Vector3 desiredCharacterPosition;
        private Vector3 lastMousePosition;

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

		#endregion

		#region private methods
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
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

            if (this.selectedCharacter != null && Input.GetMouseButtonUp(0))
            {
                this.selectedCharacter.CharacterSelected(false);
                this.selectedCharacter = null;
            }

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
					Ray raycastRay = new Ray (currentCharacterPosition, movementDirection);
					RaycastHit hitInfo = new RaycastHit ();

					if (!Physics.Raycast (raycastRay, out hitInfo, distanceToMove, layerMask))
					{						
						Debug.Log("Moving character from " + this.selectedCharacter.transform.position + " to " + snappedCharacterPosition);
						this.selectedCharacter.transform.position = snappedCharacterPosition;
					}
					else
					{
						Debug.Log("Can't move because we hit a " + hitInfo.transform.name + " when trying to move to " 
						          + snappedCharacterPosition + " from " + this.selectedCharacter.transform.position, 
						          hitInfo.transform.gameObject);
					}
				}
            }
        }
		#endregion
	}

}

