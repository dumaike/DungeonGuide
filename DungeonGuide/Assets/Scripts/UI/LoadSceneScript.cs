using UnityEngine;

namespace DungeonGuide
{
	public class LoadSceneScript : MonoBehaviour
	{
		[SerializeField]
		private string sceneName;
	
		#region public methods
		public void LoadScene()
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(this.sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
		}
		#endregion
	}

}

