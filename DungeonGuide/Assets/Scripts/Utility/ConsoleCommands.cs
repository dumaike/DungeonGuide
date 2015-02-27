using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class ConsoleCommands
	{
		public static bool fullVisionMode = false;
		[CUDLR.Command("fullvision", "Toggles full vision vs character based vision")]
		public static void FullVision() 
		{
			fullVisionMode = !fullVisionMode;
			//SceneManager.chVisionCtrl.ShowAllTiles();
		}
	}

}

