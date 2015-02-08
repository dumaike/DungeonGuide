using UnityEngine;
using UnityEngine.UI;

namespace DungeonGuide
{
	public class TintCharacterButton : MonoBehaviour 
	{
		[SerializeField]
		private ContextMenu contextMenu;
	
		public void TintCharacter()
		{
			Color tintColor = this.GetComponent<Image>().color;
			this.contextMenu.TintCharacter(tintColor);
		}
	}
}
