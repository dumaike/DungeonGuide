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
		
		private UserInputController userInputController;
		private CharacterVisionController characterVisionController;
		private SelectedCharacterController selectedCharacterController;
		private InteractiveObjectController interactiveObjectController;
		
		public static UserInputController userInputCtlr {get{return SceneManager.Instance.userInputController;}}		
		public static CharacterVisionController chVisionCtrl {get{return SceneManager.Instance.characterVisionController;}}		
		public static SelectedCharacterController selectedChCtrl {get{return SceneManager.Instance.selectedCharacterController;}}		
		public static InteractiveObjectController interactiveObjCtrl {get{return SceneManager.Instance.interactiveObjectController;}}
		
		public static Camera gameplayCam {get{return SceneManager.Instance.gameplayCamera;}}		
		public static Camera visionCam {get{return SceneManager.Instance.visionCamera;}}
		
		[SerializeField]
		private Text intputModeButton;	
		
		[SerializeField]
		private LongPressMenu longPressMenu;	
		
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
		
			this.userInputController = new UserInputController(this.intputModeButton, this.longPressMenu);
			this.characterVisionController = new CharacterVisionController(this.visionOverlayQuad, depthMaskShader);
			this.selectedCharacterController = new SelectedCharacterController();
			this.interactiveObjectController = new InteractiveObjectController();
		}

		private void OnDestroy()
		{
			this.userInputController = null;
			this.characterVisionController = null;
			this.selectedCharacterController = null;
			this.interactiveObjectController = null;
		}
		
		#endregion
		public void DestroyGo(GameObject objectToDestroy)
		{
			Destroy(objectToDestroy);
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
		private void Update()
		{		
			this.userInputController.Update();
			this.characterVisionController.Update();
		}
		#endregion
	}

}

