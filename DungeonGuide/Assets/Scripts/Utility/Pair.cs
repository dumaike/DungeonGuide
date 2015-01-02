using UnityEngine;
using System.Collections;

namespace DungeonGuide
{
	public class Pair<E1, E2>
	{
		public E1 element1;
		
		public E2 element2;
		
		#region initializers
		public Pair()
		{
		
		}
		#endregion

		#region public methods
		public Pair(E1 ele1, E2 ele2)
		{
			this.element1 = ele1;
			this.element2 = ele2;
		}
		#endregion
	}

}

