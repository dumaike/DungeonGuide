using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace DungeonGuide
{
	public class SceneManager : MonoBehaviour
	{		
		public const float SCENE_TO_WORLD_UNITS = 5.0f;
	
		public static SceneManager Instance {get; private set;}
		
		#pragma warning disable 414
		private UserInputController userInputController;
		private CharacterVisionController characterVisionController;
		private SelectedCharacterController selectedCharacterController;
		private InteractiveObjectController interactiveObjectController;
		private CharacterStackingController characterStackingController;
		private CharacterMovementController characterMovementController;
		private EventCenter eventCenter;
		#pragma warning restore 414
		
		public static UserInputController userInputCtlr {get{return SceneManager.Instance.userInputController;}}		
		public static SelectedCharacterController selectedChCtrl {get{return SceneManager.Instance.selectedCharacterController;}}		
		public static InteractiveObjectController interactiveObjCtrl {get{return SceneManager.Instance.interactiveObjectController;}}	
		public static CharacterMovementController crMvmtCtrl {get{return SceneManager.Instance.characterMovementController;}}	
		public static EventCenter eventCtr {get{return SceneManager.Instance.eventCenter;}}
		
		public static Camera gameplayCam {get{return SceneManager.Instance.gameplayCamera;}}		
		public static Camera visionCam {get{return SceneManager.Instance.visionCamera;}}
				
		[SerializeField]
		private Text intputModeButton;	
		
		[SerializeField]
		private ContextMenu contextMenu;	
		
		[SerializeField]
		private Camera gameplayCamera;
		
		[SerializeField]
		private Camera visionCamera;
		
		[SerializeField]
		private MeshFilter visionOverlayQuad;
		
		[SerializeField]
		private Material depthMaskShader;
	
		#region initializers
		private void Awake()
		{
			if (SceneManager.Instance != null)
			{
				Log.Error("There's a second SceneManager being created. THERE CAN BE ONLY ONE!", LogChannel.EDITOR_SETUP);
				return;
			}
			SceneManager.Instance = this;
			
			LayerAccessor.InitLayerInformation();
			
			GameObject gameplayObjectRoot = GameObject.Find(GridUtility.GAMEPLAY_OBJECT_ROOT_NAME);
			Canvas worldCanvas = gameplayObjectRoot.GetComponent<Canvas>();
			if (worldCanvas == null)
			{
				Log.Warning("There isn't a canvas on your gameplay root. " + 
					"Fixing at runtime, but you should fix it in the editor.", LogChannel.EDITOR_SETUP, gameplayObjectRoot);
				 worldCanvas = gameplayObjectRoot.AddComponent<Canvas>();
			}
			
			if (worldCanvas.renderMode != RenderMode.WorldSpace)
			{
				Log.Warning("The canvas on your gameplay root isn't rendering in world mode. " + 
				            "Fixing at runtime, but you should fix it in the editor.", LogChannel.EDITOR_SETUP, gameplayObjectRoot);
				worldCanvas.renderMode = RenderMode.WorldSpace;
			}
		
			this.eventCenter = new EventCenter();
			
			this.userInputController = new UserInputController(this.intputModeButton, this.contextMenu);
			this.characterVisionController = new CharacterVisionController(this.visionOverlayQuad, depthMaskShader);
			this.characterStackingController = new CharacterStackingController();
			this.selectedCharacterController = new SelectedCharacterController();
			this.interactiveObjectController = new InteractiveObjectController();
			this.characterMovementController = new CharacterMovementController();
		}

		private void OnDestroy()
		{
			this.eventCenter = null;
			
			this.userInputController = null;
			this.characterVisionController = null;
			this.selectedCharacterController = null;
			this.interactiveObjectController = null;
			this.characterStackingController = null;
			this.characterMovementController = null;
			this.eventCenter = null;
		}
		
		#endregion
		public void DestroyGo(GameObject objectToDestroy)
		{
			Destroy(objectToDestroy);
		}
		
		public void ShowAllTiles()
		{
			this.characterVisionController.ShowAllTiles();
		}
		
		#region public methods
		public void FireInputEvent(string inputEvent)
		{
			if(!Enum.IsDefined(typeof(UserInputController.InputEvent), inputEvent))
			{
				Log.Error("The input event " + inputEvent + " is not a valid input event type.", LogChannel.EDITOR_SETUP);
				return;
			}
		
			UserInputController.InputEvent castEvent = 
				(UserInputController.InputEvent)Enum.Parse(typeof(UserInputController.InputEvent), inputEvent);
			this.userInputController.ReceiveInputEvent(castEvent);
		}
		#endregion

		#region private methods		
		private void LateUpdate()
		{
			this.userInputController.Update();
		}		
		#endregion
	}

}

