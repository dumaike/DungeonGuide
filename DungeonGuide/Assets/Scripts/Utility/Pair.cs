using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

		public override bool Equals(object compair)
		{
			Pair<E1, E2> pair = compair as Pair<E1, E2>;
			if (pair != null)
			{
				return
					EqualityComparer<E1>.Default.Equals(pair.element1, this.element1) &&
					EqualityComparer<E2>.Default.Equals(pair.element2, this.element2);
			}

			return false;
		}

		public override string ToString()
		{
			return "(" + element1.ToString() + ", " + element2.ToString() + ")";
		}

		#endregion
	}
}

