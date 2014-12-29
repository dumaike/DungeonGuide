using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class CharacterRoot : MonoBehaviour
	{						
		protected MeshRenderer[] meshRenderers;

		private Color selectedColor = new Color(0, 1, 1, 1);
		private Color defaultColor = new Color(1, 1, 1, 1);

		[SerializeField]
		private bool inPlaySerialized = false;
		public bool inPlay { get{ return this.inPlaySerialized; } private set { this.inPlaySerialized = value; } }

		#region initializers
		virtual protected void Awake()
		{		
			this.meshRenderers = this.GetComponentsInChildren<MeshRenderer> ();
		}

		private void Start()
		{

		}

		private void OnDestroy()
		{

		}
		#endregion

		#region public methods
		public void CharacterSelected(bool selected)
		{
			foreach (MeshRenderer curRenderer in this.meshRenderers)
			{
				curRenderer.material.color = selected ? this.selectedColor : this.defaultColor;
			}
			
			//If we're deselecting, see if we should be docking or not
			if (!selected)
			{
				CharacterDock characterDoc = GameObject.FindObjectOfType<CharacterDock>();
				Vector3 mousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
				this.inPlay = !characterDoc.IsPointInDock(mousePosition);
				if (this.inPlay)
				{
					this.transform.parent = null;
					this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
				}
				else
				{
					this.transform.position = new Vector3(mousePosition.x, CharacterDock.UI_HEIGHT_OFFSET, mousePosition.z);
					this.transform.parent = characterDoc.transform;
				}
			}
		}
		#endregion

		#region private methods

		#endregion
	}

}

