using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class ObjectCreationUi : MonoBehaviour
	{
		public Vector3 characterCreationPosition {get; private set;}
	
		#region public methods
		public void CloseObjectCreation()
		{
			this.gameObject.SetActive(false);
		}
		
		public void OpenObjectCreation(Vector3 positionToCreateObject)
		{
			this.characterCreationPosition = positionToCreateObject;
			this.gameObject.SetActive(true);
		}
		#endregion

		#region private methods

		#endregion
	}

}

