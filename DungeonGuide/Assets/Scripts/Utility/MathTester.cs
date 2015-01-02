using UnityEngine;
using System.Collections;
using System;

namespace DungeonGuide
{
	[ExecuteInEditMode]
	public class MathTester : MonoBehaviour
	{
		public float input;
		public float output;
		
		private void Update()
		{
			this.output = (float)Math.Round(input+0.5f)-0.5f;
		}
	}

}

