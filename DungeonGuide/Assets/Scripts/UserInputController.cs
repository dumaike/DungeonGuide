using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class UserInputController : MonoBehaviour
	{
        private PlayerCharacterRoot selectedCharacter;
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
                    this.selectedCharacter = hitInfo.transform.GetComponentInParent<PlayerCharacterRoot>();  
                    if (this.selectedCharacter != null)
                    {
                        this.selectedCharacter.CharacterSelected(true);
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
                Vector3 newMousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
                Vector3 mousePositionDelta = newMousePosition - this.lastMousePosition;
                this.lastMousePosition = newMousePosition;

                this.selectedCharacter.transform.position = this.selectedCharacter.transform.position + mousePositionDelta;
            }
        }
		#endregion
	}

}

