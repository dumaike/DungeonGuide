using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class LoadSceneScript : MonoBehaviour
	{
		[SerializeField]
		private string sceneName;
	
		#region public methods
		public void LoadScene()
		{
			Application.LoadLevel(this.sceneName);
		}
		#endregion
	}

}

