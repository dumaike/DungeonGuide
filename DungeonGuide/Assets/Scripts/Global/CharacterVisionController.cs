using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DungeonGuide
{
	public class CharacterVisionController
	{				
		private const float VISION_DISTANCE = 60.0f;
		private const int NUM_RAYS = 360;
		
		private Vector3 VISION_OFFSET = new Vector3(0, 0.1f, 0);

		private List<MoveableRoot> playerCharacters;
		private List<GameObject> characterVisionMeshes;
		
		private List<List<Vector3>> visionPerCharacter;
		
		private MeshFilter visionOverlay;
		
		private Material depthMaskShader;

		#region initializers
		public CharacterVisionController(MeshFilter visionOverlay, Material depthMaskShader)
		{						
			this.depthMaskShader = depthMaskShader;
			this.visionOverlay = visionOverlay;
			UpdateVisionQuad();
		
			MoveableRoot[] characters = GameObject.FindObjectsOfType<MoveableRoot>();
			
			this.characterVisionMeshes = new List<GameObject>();
			this.playerCharacters = new List<MoveableRoot>();
			foreach (MoveableRoot character in characters)
			{
				if (character.hasVision)
				{
					AddCharacterToVision(character);			
				}
			}						
			
			SceneManager.eventCtr.objectMovedEvent += HandleobjectMovedEvent;
			SceneManager.eventCtr.interactiveObjectToggeled += HandleinteractiveObjectToggeled;
			SceneManager.eventCtr.objectCreated += HandleobjectCreated;
			SceneManager.eventCtr.objectRemoved += HandleobjectRemoved;
			SceneManager.eventCtr.cameraZoomed += HandlecameraZoomed;
			
			UpdateVision();
		}
		
		~CharacterVisionController()
		{			
			SceneManager.eventCtr.objectMovedEvent -= HandleobjectMovedEvent;
			SceneManager.eventCtr.interactiveObjectToggeled -= HandleinteractiveObjectToggeled;
			SceneManager.eventCtr.objectCreated -= HandleobjectCreated;
			SceneManager.eventCtr.objectRemoved -= HandleobjectRemoved;
			SceneManager.eventCtr.cameraZoomed -= HandlecameraZoomed;
		}
		#endregion
		
		#region public methods
		
		public void ShowAllTiles()
		{
			this.visionOverlay.gameObject.SetActive(false);
		}
		
		#endregion
		
		#region private methods
		
		private void RemoveCharacterFromVision(MoveableRoot character)
		{			
			int indexOfCharacter = this.playerCharacters.IndexOf(character);
			this.playerCharacters.RemoveAt(indexOfCharacter);
			
			SceneManager.Destroy(this.characterVisionMeshes[indexOfCharacter]);
			this.characterVisionMeshes.RemoveAt(indexOfCharacter);
		}
		
		private void AddCharacterToVision(MoveableRoot character)
		{
			this.playerCharacters.Add (character);
			
			GameObject newMeshObject = new GameObject(character.name + "VisionMesh");
			MeshFilter meshFilter = newMeshObject.AddComponent<MeshFilter>();
			meshFilter.sharedMesh = new Mesh();
			newMeshObject.AddComponent<MeshRenderer>();
			this.characterVisionMeshes.Add(newMeshObject);			
		}
		
		public void UpdateVisionQuad()
		{
			Vector3[] cameraCorners = new Vector3[]
			{
				SceneManager.gameplayCam.ViewportToWorldPoint(new Vector3(0,0,0)),
				SceneManager.gameplayCam.ViewportToWorldPoint(new Vector3(0,1,0)),
				SceneManager.gameplayCam.ViewportToWorldPoint(new Vector3(1,1,0)),
				SceneManager.gameplayCam.ViewportToWorldPoint(new Vector3(1,0,0))
			};
			
			this.visionOverlay.sharedMesh = new Mesh();
			Mesh mesh = this.visionOverlay.sharedMesh;
			mesh.Clear();
			mesh.vertices = cameraCorners;
			mesh.triangles = new int[]
			{
				0,1,2,
				2,3,0
			};
			
			Vector2[] uvs = new Vector2[]
			{
				new Vector2(0,0), //bottom-left
				new Vector2(0,1), //top-left
				new Vector2(1,1), //top-right
				new Vector2(1,0) //bottom-right
			};
			mesh.uv = uvs;
			
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			mesh.Optimize();			
		}
		
		private void HandlecameraZoomed ()
		{
			UpdateVisionQuad();
		}

		private void HandleobjectMovedEvent (MoveableRoot target, Vector3 oldPosition, Vector3 newPosition)
		{
			if (target.hasVision)
			{
				UpdateVision();
			}
		}
		
		private void HandleinteractiveObjectToggeled ()
		{
			UpdateVision();
		}	
		
		private void HandleobjectRemoved (MoveableRoot target, Vector3 position)
		{			
			if (target.hasVision)
			{
				RemoveCharacterFromVision(target);
			}
			UpdateVision();
		}
		
		private void HandleobjectCreated (MoveableRoot target, Vector3 position)
		{			
			if (target.hasVision)
			{
				AddCharacterToVision(target);
			}
			UpdateVision();
		}
		
		private void UpdateVision()
		{			
			int layerMask = (1 << LayerAccessor.DEFAULT) + (1 << LayerAccessor.BLOCKS_SIGHT);
			
			Vector3 visionLocationOffset = SceneManager.visionCam.transform.position - SceneManager.gameplayCam.transform.position;
			
			//Create the vision points
			for (int iPlayerIndex = 0; iPlayerIndex < this.playerCharacters.Count; ++iPlayerIndex) 
			{
				MoveableRoot player = this.playerCharacters[iPlayerIndex];
				
				Vector3 characterVisionOrigin = player.transform.position + VISION_OFFSET;
				float raycastStep = 360.0f/NUM_RAYS;
				Vector3[] visionPoints = new Vector3[NUM_RAYS + 1];
				visionPoints[0] = characterVisionOrigin + visionLocationOffset;
				
				for (int iRayIndex = 0; iRayIndex < NUM_RAYS; ++iRayIndex)
				{
					Vector3 directionVector = Quaternion.Euler(0, iRayIndex*raycastStep, 0)*Vector3.right;
					Ray raycastRay = new Ray(characterVisionOrigin, directionVector);
					
					RaycastHit hitInfo = new RaycastHit ();
					bool hitSomething = Physics.Raycast (raycastRay, out hitInfo, VISION_DISTANCE, layerMask);
					Vector3 hitPoint;
					if (hitSomething)
					{
						hitPoint = hitInfo.point;
					}
					else
					{
						hitPoint = characterVisionOrigin + directionVector*VISION_DISTANCE;
					}
					
					visionPoints[iRayIndex + 1] = hitPoint + visionLocationOffset;
					//Debug.DrawRay(characterVisionOrigin, hitPoint - characterVisionOrigin);
				}
				
				//Draw the character's vision mesh
				GameObject visionMesh = this.characterVisionMeshes[iPlayerIndex];
				MeshFilter meshFilter = visionMesh.GetComponent<MeshFilter>();
				meshFilter.renderer.material = this.depthMaskShader;
				Mesh mesh = meshFilter.sharedMesh;
				mesh.Clear();
				mesh.vertices = visionPoints;
				int[] meshTris = new int[NUM_RAYS*3];
				for (int iTriangleIndex = 0; iTriangleIndex < NUM_RAYS; iTriangleIndex++)
				{
					meshTris[iTriangleIndex*3] = 0;
					meshTris[iTriangleIndex*3 + 1] = iTriangleIndex + 1;
					meshTris[iTriangleIndex*3 + 2] = iTriangleIndex + 2;
				}
				meshTris[meshTris.Length - 1] = 1;
				
				mesh.triangles = meshTris;
				
				mesh.RecalculateNormals();
				mesh.RecalculateBounds();
				mesh.Optimize();
			}
		}
		
		#endregion
	}

}

