using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum LogChannel
{
	DEBUG, CHARACTER_MOVEMENT, EDITOR_SETUP, INPUT, CHARACTER_VISION, LOGIC
};


public class Log
{
	public const string LOG_CHANNEL_PREFS_KEY = "LOG_CHANNELS";
	
	private static List<LogChannel> channelsToLog = null;
	
	public static bool IsChannelLogging(LogChannel channel)
	{		
		if (channelsToLog == null)
		{
			channelsToLog = new List<LogChannel>();
			string logChannelsString = PlayerPrefs.GetString(LOG_CHANNEL_PREFS_KEY);
			PlayerPrefs.SetString(LOG_CHANNEL_PREFS_KEY, "");
			
			logChannelsString = logChannelsString.Trim();
			
			string[] logChannelsTokenized = logChannelsString.Split(' ');
			for (int iChannelIndex = 0; iChannelIndex < logChannelsTokenized.Length; ++iChannelIndex)
			{
				string channelAsString = logChannelsTokenized[iChannelIndex];
				if (channelAsString != null && channelAsString.Length > 1)
				{
					LogChannel channelToAdd = (LogChannel)System.Enum.Parse(typeof(LogChannel), channelAsString, true);
					AddChannel(channelToAdd);
				}
			}
			
		}
		
		if (Log.channelsToLog.Contains(channel))
		{
			return true;
		}
		
		return false;
	}
	
	public static void Print(string message, LogChannel channel = LogChannel.DEBUG, Object context = null)
	{
		if (IsChannelLogging(channel))
		{
			Debug.Log(channel + ": " + message, context);
		}
	}
	
	public static void Warning(string message, LogChannel channel = LogChannel.DEBUG, Object context = null)
	{
		Debug.LogWarning(channel + ": " +message, context);
	}
	
	public static void Error(string message, LogChannel channel = LogChannel.DEBUG, Object context = null)
	{
		Debug.LogError(channel + ": " +message, context);
	}	
	
	public static void AddChannel(LogChannel channel)
	{
		if (IsChannelLogging(channel))
			Debug.LogWarning("A channel that's already being logged is trying to start logging (" + channel + ")");
		else
		{			
			string logChannelsString = PlayerPrefs.GetString(LOG_CHANNEL_PREFS_KEY);
			logChannelsString += channel.ToString() + " ";
			PlayerPrefs.SetString(LOG_CHANNEL_PREFS_KEY, logChannelsString);
		
			Log.channelsToLog.Add(channel);
		}
	}
	
	public static void RemoveChannel(LogChannel channel)
	{
		if (!IsChannelLogging(channel))
			Debug.LogWarning("A channel that's already not being logged is trying to stop logging (" + channel + ")");
		else
		{
			Log.channelsToLog.Remove(channel);
			
			string channelsAsString = "";
			foreach (LogChannel channelBeingLogged in Log.channelsToLog)
			{
				channelsAsString += channelBeingLogged.ToString() + " ";
			}			
			
			PlayerPrefs.SetString(LOG_CHANNEL_PREFS_KEY, channelsAsString);
		}
	}
}