using UnityEditor;
using System;

namespace DungeonGuide
{
	public class ConfigPreferencesWindow : EditorWindow
	{
		// Add menu named "My Window" to the Window menu
		[MenuItem("Tools/Configuration Preferences %4")]
		public static void Init()
		{
			//Make a window and focus on it
			ConfigPreferencesWindow window = (ConfigPreferencesWindow)EditorWindow.GetWindow(typeof(ConfigPreferencesWindow));
			window.Focus();
		}
		
		// Use this for initialization
		public void OnGUI()
		{
			EditorGUILayout.LabelField("Channels Being Logged");
			foreach (LogChannel channel in (LogChannel[])Enum.GetValues(typeof(LogChannel)))
			{
				bool toggledInClass = Log.IsChannelLogging(channel);
				bool toggledInWindow = toggledInClass;
				toggledInWindow = EditorGUILayout.Toggle(channel.ToString(), toggledInWindow);
				
				if (toggledInClass != toggledInWindow)
				{
					if (toggledInWindow)
					{
						Log.AddChannel(channel);
					}
					else
					{
						Log.RemoveChannel(channel);
					}
				}
			}	
		}
	}
	
	
	
	
}