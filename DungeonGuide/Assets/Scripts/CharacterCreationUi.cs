using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class CharacterCreationUi : MonoBehaviour
	{
		public Vector3 characterCreationPosition {get; private set;}
	
		#region initializers
		private void Awake()
		{
			if (Time.time < 0.1f)
			{
				Log.Warning("You left the CharacterCreationUi active. Deactivating at startup, but you should really do this in the editor");
				this.gameObject.SetActive(false);
			}
		}

		private void Start()
		{

		}

		private void OnDestroy()
		{

		}
		#endregion

		#region public methods
		public void CloseCharacterCreation()
		{
			this.gameObject.SetActive(false);
		}
		
		public void OpenCharacterCreation(Vector3 positionToCreateCharacter)
		{
			this.characterCreationPosition = positionToCreateCharacter;
			this.gameObject.SetActive(true);
		}
		#endregion

		#region private methods

		#endregion
	}

}

