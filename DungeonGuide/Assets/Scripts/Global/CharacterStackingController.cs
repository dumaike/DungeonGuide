using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGuide
{
	public class CharacterStackingController
	{				
		private List<MoveableEntity> stackableEntities;
		
		private const float SCALE_PER_COUNT = 0.9f;
		private Vector3 SPACING_BETWEEN_ITEMS = new Vector3(-0.1f, -0.001f, 0.1f);
		
		#region initializers
		public CharacterStackingController()
		{								
			MoveableEntity[] characters = GameObject.FindObjectsOfType<MoveableEntity>();
			
			this.stackableEntities = new List<MoveableEntity>();
			foreach (MoveableEntity character in characters)
			{
				if (character.stackableParts != null)
				{
					AddStackableObject(character, character.transform.position);			
				}
			}	
			
			SceneManager.eventCtr.objectMovedEvent += HandleobjectMovedEvent;
			SceneManager.eventCtr.objectCreated += AddStackableObject;
			SceneManager.eventCtr.objectRemoved += RemoveStackableObject;
		}
		
		~CharacterStackingController()
		{						
			SceneManager.eventCtr.objectMovedEvent += HandleobjectMovedEvent;
			SceneManager.eventCtr.objectCreated -= AddStackableObject;
			SceneManager.eventCtr.objectRemoved -= RemoveStackableObject;
		}
		#endregion
		
		#region public methods
		
		
		#endregion
		
		#region private methods
		private void RemoveStackableObject(MoveableEntity entity, Vector3 position)
		{			
			int indexOfCharacter = this.stackableEntities.IndexOf(entity);
			this.stackableEntities.RemoveAt(indexOfCharacter);
		}
		
		private void AddStackableObject(MoveableEntity character, Vector3 position)
		{
			this.stackableEntities.Add (character);
		}
		
		void HandleobjectMovedEvent (MoveableEntity target, Vector3 oldPosition, Vector3 newPosition)
		{
			//If the moving object isn't stackable, don't worry about it
			if (target.stackableParts == null)
			{
				return;
			}
		
			//Check to see what other objects it stacks with
			List<MoveableEntity> destinationStack = new List<MoveableEntity>();
			destinationStack.Add(target);
			List<MoveableEntity> sourceStack = new List<MoveableEntity>();
			List<MoveableEntity> nonStacked = new List<MoveableEntity>();
			foreach (MoveableEntity otherObject in this.stackableEntities)
			{
				if (otherObject.transform.position.x == newPosition.x && 
				    otherObject.transform.position.z == newPosition.z && 
				    otherObject.characterSize == target.characterSize &&
				    otherObject != target &&
				    otherObject != null)
				{
					destinationStack.Add(otherObject);
				}
				else if (otherObject.transform.position.x == oldPosition.x && 
					otherObject.transform.position.z == oldPosition.z && 
					otherObject.characterSize == target.characterSize &&
					otherObject != target &&
					otherObject != null)
				{
					sourceStack.Add(otherObject);
				}
				else if (otherObject != target)
				{
					nonStacked.Add(otherObject);
				}
			}
			
			CreateStack(destinationStack);
			CreateStack(sourceStack);
			
			//Make sure we keep stack order between all stacks
			this.stackableEntities = new List<MoveableEntity>();
			this.stackableEntities.AddRange(destinationStack);
			this.stackableEntities.AddRange(sourceStack);
			this.stackableEntities.AddRange(nonStacked);
		}
		
		private void CreateStack(List<MoveableEntity> stack)
		{
			float entityScale = Mathf.Pow(SCALE_PER_COUNT, stack.Count - 1);
			Vector3 lowestItem = Vector3.zero;
			if (stack.Count > 1)
			{
				lowestItem = (SPACING_BETWEEN_ITEMS*((stack.Count-1)/2.0f));
			}
			
			if (stack.Count > 1)
			{
				Log.Print("Creating a stack of size " + stack.Count + ".", LogChannel.STACKING);
			}
			
			foreach(MoveableEntity entity in stack)
			{
				entity.stackableParts.transform.localScale = new Vector3(entityScale, entityScale, entityScale);
				entity.stackableParts.transform.localPosition = lowestItem;
				lowestItem -= SPACING_BETWEEN_ITEMS;
				
				//Zero out the y of all entities
				Vector3 zeroedEntityPosition = entity.transform.position;
				zeroedEntityPosition.y = 0;
				entity.transform.position = zeroedEntityPosition;
				
				entity.ShowText(false);
			}
			
			//Raise the last entity up a little so he's the first one clicked
			if (stack.Count > 0)
			{				
				MoveableEntity topEntity = stack[stack.Count - 1];
				if (stack.Count > 1)
				{
					Vector3 zeroedEntityPosition = topEntity.transform.position;
					zeroedEntityPosition.y = lowestItem.y + 1;
					topEntity.transform.position = zeroedEntityPosition;
				}
				topEntity.ShowText(true);
			}
		}
		
		#endregion
	}

}

