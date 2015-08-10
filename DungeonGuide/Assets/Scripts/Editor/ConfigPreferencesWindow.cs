using UnityEditor;
using System;
using System.Globalization;

namespace DungeonGuide
{
	public class ConfigPreferencesWindow : EditorWindow
	{
		// Add menu named "My Window" to the Window menu
		[MenuItem("Tools/Configuration Preferences %4")]
		public static void Init()
		{
			//Make a window and focus on it
			ConfigPreferencesWindow window = (ConfigPreferencesWindow)GetWindow(typeof(ConfigPreferencesWindow));
			window.Focus();
		}
		
		// Use this for initialization
		public void OnGUI()
		{
			EditorGUILayout.LabelField("Channels Being Logged");
			foreach (LogChannel channel in (LogChannel[])Enum.GetValues(typeof(LogChannel)))
			{				
				string prettyChannelName = channel.ToString().Replace("_", " ");
				TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
				prettyChannelName = textInfo.ToTitleCase(prettyChannelName.ToLower());	
				
				bool toggledInClass = Log.IsChannelLogging(channel);
				bool toggledInWindow = toggledInClass;
				toggledInWindow = EditorGUILayout.Toggle(prettyChannelName, toggledInWindow);
				
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