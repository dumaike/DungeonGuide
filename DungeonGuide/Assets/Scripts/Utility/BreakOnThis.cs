using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class BreakOnThis : MonoBehaviour
	{
		#region initializers
		private void Awake()
		{

		}

		private void Start()
		{

		}

		private void OnDestroy()
		{

		}
		#endregion

		#region public methods
		public static bool HasBreakComponent(MonoBehaviour component)
		{
			return component.gameObject.GetComponent<BreakOnThis>() != null;
		}
		#endregion

		#region private methods

		#endregion
	}

}

