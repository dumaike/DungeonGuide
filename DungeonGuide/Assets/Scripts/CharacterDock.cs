﻿using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	[ExecuteInEditMode]
	public class CharacterDock : MonoBehaviour
	{
		[SerializeField]
		private GameObject rightEdge = null;
		
		[SerializeField]
		private GameObject leftEdge = null;
		
		public float dockShift {get; private set;}

		#region initializers
		private void Awake()
		{
			MoveDockToEdge ();
		}
		#endregion

		#region public methods
		
		#endregion

		#region private methods
		private void Update()
		{
			if (!Application.isPlaying)
			{
				//MoveDockToEdge();
			}
		}

		private void MoveDockToEdge()
		{
			//Move the dock so that it's centered independent of screen space
			Vector3 leftCenterEdgeOfCamera = Camera.main.ScreenToWorldPoint (new Vector3 (0, Screen.height / 2.0f, 0));
			Vector3 dockEdgeDifference = leftCenterEdgeOfCamera - this.rightEdge.transform.position;
			Vector3 newPosition = this.transform.position + dockEdgeDifference;
			newPosition.y = this.transform.position.y;
			this.transform.position = newPosition;
			
			this.dockShift = this.rightEdge.transform.position.x - this.leftEdge.transform.position.x;		
		}
		#endregion
	}

}
