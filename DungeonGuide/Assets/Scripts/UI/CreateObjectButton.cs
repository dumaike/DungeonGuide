using UnityEngine;

namespace DungeonGuide
{
	public class CreateObjectButton : MonoBehaviour
	{				
		[SerializeField]
		protected GameObject objectToCreate;
		
		protected ObjectCreationUi creationUi;
		
		protected GameObject gameplayObjectRoot;
		
		#region initializers
		public virtual void Awake()
		{
			this.creationUi = this.transform.GetComponentInParent<ObjectCreationUi>();
			
			this.gameplayObjectRoot = GameObject.Find(GridUtility.GAMEPLAY_OBJECT_ROOT_NAME);
		}

		#endregion

		#region public methods
		/// <summary>
		/// The function the unity editon will call to create this object
		/// </summary>
		public void EditorCreateObject()
		{
			CreateObject();
		}
		#endregion

		#region protected methods
		public virtual GameObject CreateObject()
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

			return createdObject;
		}
		#endregion
	}

}

