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
			GameObject createdCharacter = Instantiate(this.objectToCreate) as GameObject;
			createdCharacter.transform.position = this.creationUi.characterCreationPosition;
			createdCharacter.transform.SetParent(this.gameplayObjectRoot.transform);
			
			MoveableEntity moveableRootOfCharacter = createdCharacter.GetComponentInChildren<MoveableEntity>();
			
			this.creationUi.CloseObjectCreation();			
			SceneManager.eventCtr.FireObjectCreatedEvent(moveableRootOfCharacter, moveableRootOfCharacter.transform.position);
		}
		#endregion

		#region private methods

		#endregion
	}

}

