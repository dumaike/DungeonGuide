using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DungeonGuide
{
	public class CreateObjectButton : MonoBehaviour
	{				
		[SerializeField]
		private GameObject objectToCreate;
		
		private ObjectCreationUi creationUi;
		
		private GameObject gameplayObjectRoot;
		
		#region initializers
		public void Awake()
		{
			this.creationUi = this.transform.GetComponentInParent<ObjectCreationUi>();
			
			this.gameplayObjectRoot = GameObject.Find(GridUtility.GAMEPLAY_OBJECT_ROOT_NAME);
		}
		
		#endregion

		#region public methods
		public void CreateObject()
		{
			GameObject createdObject = Instantiate(this.objectToCreate) as GameObject;
			createdObject.transform.position = this.creationUi.characterCreationPosition;

			//If the object is corner snapping, snap them to the nearest corner
			SnappableRoot snappableRoot = createdObject.GetComponent<SnappableRoot>();
			if (snappableRoot != null && snappableRoot.snapType == SnapType.CORNER)
			{
				createdObject.transform.position =
					createdObject.transform.position + new Vector3(0.5f, 0, 0.5f);
			}

			createdObject.transform.SetParent(this.gameplayObjectRoot.transform);
			
			MoveableEntity moveableRootOfObject = createdObject.GetComponentInChildren<MoveableEntity>();
			
			this.creationUi.CloseObjectCreation();			
			SceneManager.eventCtr.FireObjectCreatedEvent(moveableRootOfObject, moveableRootOfObject.transform.position);
		}
		#endregion

		#region private methods

		#endregion
	}

}

