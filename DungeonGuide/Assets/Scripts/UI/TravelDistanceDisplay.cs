using UnityEngine;
using UnityEngine.UI;

namespace DungeonGuide
{
	public class TravelDistanceDisplay : MonoBehaviour
	{
		private Text distanceText;
		private Image distanceBacking;

		private Vector3 selectedObjectStartPosition;
	
		#region initializers
		private void Start()
		{
			this.distanceText = this.GetComponentInChildren<Text>();
			this.distanceBacking = this.GetComponentInChildren<Image>();

			SceneManager.eventCtr.objectMovedEvent += HandleObjectMovedEvent;
			SceneManager.eventCtr.objectSelected += HandleObjectSelectedEvent;

			this.distanceBacking.gameObject.SetActive(false);
		}

		private void OnDestroy()
		{
			if (SceneManager.eventCtr != null)
			{
				SceneManager.eventCtr.objectMovedEvent -= HandleObjectMovedEvent;
				SceneManager.eventCtr.objectSelected -= HandleObjectSelectedEvent;
			}
		}
		#endregion

		#region private methods
		private void HandleObjectSelectedEvent(MoveableEntity target, bool selected)
		{
			this.selectedObjectStartPosition = target.transform.position;
		}

		private void HandleObjectMovedEvent(MoveableEntity target, Vector3 oldPosition, Vector3 newPosition)
		{
			Vector3 movementAmount = this.selectedObjectStartPosition - target.transform.position;
			float travelDistance = movementAmount.magnitude;

			int roundedDistance = (int)Mathf.Ceil(travelDistance * SceneManager.SCENE_TO_WORLD_UNITS);

			if (roundedDistance > 0)
			{
				this.distanceBacking.gameObject.SetActive(true);
				this.distanceText.text = roundedDistance.ToString("N0");
			}
			else
			{
				this.distanceBacking.gameObject.SetActive(false);
			}
		}
		#endregion
	}

}

