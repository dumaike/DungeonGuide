using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DungeonGuide
{
	public class ObjectRotationUi : MonoBehaviour
	{
		private MoveableEntity selectedObject;

		[SerializeField]
		private Slider rotationSlider = null;

		#region initializers
		private void Start()
		{
			SceneManager.eventCtr.objectSelected += HandleObjectSelected;
			this.gameObject.SetActive(false);
		}

		private void OnDestroy()
		{
			if (SceneManager.eventCtr != null)
			{
				SceneManager.eventCtr.objectSelected -= HandleObjectSelected;
			}
		}
		#endregion

		#region public methods
		public void SetSelectedObjectRotation(float value)
		{
			this.selectedObject.transform.eulerAngles = new Vector3(
					this.selectedObject.transform.eulerAngles.x, 
					this.rotationSlider.value,
					this.selectedObject.transform.eulerAngles.z);
		}
		#endregion

		#region private methods

		private void HandleObjectSelected(MoveableEntity target, bool selected)
		{
			this.selectedObject = !selected ? null : target;

			if (this.selectedObject != null)
			{
				this.rotationSlider.value = target.transform.eulerAngles.y;
			}
		}

		#endregion
	}

}

