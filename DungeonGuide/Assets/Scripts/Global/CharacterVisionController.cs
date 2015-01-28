using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DungeonGuide
{
	public class CharacterVisionController
	{				
		private const float VISION_DISTANCE = 60.0f;
		private const int NUM_RAYS = 360;

		public const string INVISIBLE_LAYER_NAME = "NonSightBlocking";
		
		private Vector3 VISION_OFFSET = new Vector3(0, 0.1f, 0);

		private List<PlayerCharacterRoot> playerCharacters;
		private List<GameObject> characterVisionMeshes;
		
		private List<List<Vector3>> visionPerCharacter;
		
		private bool visionDirty = true;
		
		private MeshFilter visionOverlay;
		
		private Material depthMaskShader;

		#region initializers
		public CharacterVisionController(MeshFilter visionOverlay, Material depthMaskShader)
		{						
			this.depthMaskShader = depthMaskShader;
			this.visionOverlay = visionOverlay;
			UpdateVisionQuad();
		
			PlayerCharacterRoot[] characters = GameObject.FindObjectsOfType<PlayerCharacterRoot>();
			
			this.characterVisionMeshes = new List<GameObject>();
			this.playerCharacters = new List<PlayerCharacterRoot>();
			foreach (PlayerCharacterRoot character in characters)
			{
				AddCharacterToVision(character);
			}						
		}
		#endregion

		#region public methods
		public void SetVisionDirty()
		{
			Log.Print("Character vision set to dirty. Recalculating.", LogChannel.CHARACTER_VISION);
		
			this.visionDirty = true;
		}
		
		public void RemoveCharacterFromVision(PlayerCharacterRoot character)
		{
			SetVisionDirty();
			
			int indexOfCharacter = this.playerCharacters.IndexOf(character);
			this.playerCharacters.RemoveAt(indexOfCharacter);
			this.characterVisionMeshes.RemoveAt(indexOfCharacter);
		}
		
		public void AddCharacterToVision(PlayerCharacterRoot character)
		{
			SetVisionDirty();
			this.playerCharacters.Add (character);
			
			GameObject newMeshObject = new GameObject(character.name + "VisionMesh");
			MeshFilter meshFilter = newMeshObject.AddComponent<MeshFilter>();
			meshFilter.sharedMesh = new Mesh();
			newMeshObject.AddComponent<MeshRenderer>();
			this.characterVisionMeshes.Add(newMeshObject);			
		}
		
		public void Update()
		{		
			//If nothing has changed that requires a vision re-calc, don't
			//bother doing anything in the update.
			if (!this.visionDirty || ConsoleCommands.fullVisionMode)
			{
				return;
			}
		
			int layerMask = 1 << 0;
			
			Vector3 visionLocationOffset = SceneManager.visionCam.transform.position - SceneManager.gameplayCam.transform.position;
			
			//Create the vision points
			for (int iPlayerIndex = 0; iPlayerIndex < this.playerCharacters.Count; ++iPlayerIndex) 
			{
				PlayerCharacterRoot player = this.playerCharacters[iPlayerIndex];
			
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
			
			this.visionDirty = false;
		}
		
		public void ShowAllTiles()
		{
			this.visionOverlay.gameObject.SetActive(false);
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
		#endregion

		#region private methods
		#endregion
	}

}

