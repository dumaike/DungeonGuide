using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DungeonGuide
{
	public class TravelDistanceDisplay : MonoBehaviour
	{
		private Text distanceText;
		private Image distanceBacking;
	
		#region initializers
		private void Awake()
		{
			this.distanceText = this.GetComponentInChildren<Text>();
			this.distanceBacking = this.GetComponentInChildren<Image>();
		}
		#endregion

		#region private methods
		private void Update()
		{
			float travelDistance = 
				SceneManager.userInputCtlr.SelectedCharacterMovementAmount();
			int roundedDistance = (int)Mathf.Ceil(travelDistance*SceneManager.SCENE_TO_WORLD_UNITS);
			
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

