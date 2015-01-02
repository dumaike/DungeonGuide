using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class CloseAppScript : MonoBehaviour
	{
		#region public methods
		public void CloseApplication()
		{
			Application.Quit ();
		}
		#endregion
	}

}

