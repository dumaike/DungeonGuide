using UnityEngine;
using UnityEngine.UI;

namespace DungeonGuide
{
	public class TravelDistanceDisplay : MonoBehaviour
	{
		[SerializeField]
		private Text totalDistanceText;

		[SerializeField]
		private Text straightLineDistanceText;

		private Image distanceBacking;

		/// used for straight line distance
		private Vector3 selectedObjectStartPosition;

		///used for total distance
		private float selectedObjectLastDistance;
	
		#region initializers
		private void Start()
		{
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

			this.selectedObjectLastDistance = 0;

			this.distanceBacking.gameObject.SetActive (selected);

			if (selected) 
			{
				this.straightLineDistanceText.text = "0";
				this.totalDistanceText.text = "0";
			}
		}

		/// <summary>
		/// Used to update straight line and total distance when object is moved.
		/// </summary>
		/// <param name="target">the object that moved.</param>
		/// <param name="oldPosition">where the object was last update.</param>
		/// <param name="newPosition">where the object is now.</param>
		private void HandleObjectMovedEvent(MoveableEntity target, Vector3 oldPosition, Vector3 newPosition)
		{
			CalculateStraightLineDistance (target);
			CalculateTotalDistance (target, oldPosition, newPosition);
		}

		private void CalculateStraightLineDistance(MoveableEntity target)
		{
			Vector3 movementAmount = this.selectedObjectStartPosition - target.transform.position;
			float travelDistance = movementAmount.magnitude;

			int roundedDistance = (int)Mathf.Ceil((int)travelDistance * SceneManager.SCENE_TO_WORLD_UNITS);

			this.straightLineDistanceText.text = roundedDistance.ToString("N0");
		}

		private void CalculateTotalDistance(MoveableEntity target, Vector3 oldPosition, Vector3 newPosition)
		{
			Vector3 movementThisStep = oldPosition - newPosition;
			float distanceThisStep = movementThisStep.magnitude;
			float distanceTotal = distanceThisStep + this.selectedObjectLastDistance;
			this.selectedObjectLastDistance = distanceTotal;

			int roundedDistance = (int)Mathf.Ceil ((int)distanceTotal * SceneManager.SCENE_TO_WORLD_UNITS);

			if (roundedDistance > 0) 
			{
				this.distanceBacking.gameObject.SetActive (true);
				this.totalDistanceText.text = roundedDistance.ToString ("N0");
			}
			else
			{
				this.distanceBacking.gameObject.SetActive (false);
			}
		}
		#endregion
	}

}

