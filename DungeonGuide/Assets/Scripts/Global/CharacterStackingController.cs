using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGuide
{
	public class CharacterStackingController
	{				
		private List<MoveableRoot> playerCharacters;
		
		#region initializers
		public CharacterStackingController()
		{								
			this.playerCharacters = GameObject.FindObjectsOfType<MoveableRoot>().ToList();
			
			SceneManager.eventCtr.objectMovedEvent += HandleobjectMovedEvent;
		}
		
		~CharacterStackingController()
		{						
			SceneManager.eventCtr.objectMovedEvent += HandleobjectMovedEvent;
		}
		#endregion
		
		#region public methods
		
		
		#endregion
		
		#region private methods
		
		void HandleobjectMovedEvent (MoveableRoot target, Vector3 oldPosition, Vector3 newPosition)
		{
			
		}
		
		#endregion
	}

}

