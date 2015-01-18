using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum LogChannel
{
	DEBUG, CHARACTER_MOVEMENT, EDITOR_SETUP, INPUT, CHARACTER_VISION
};

public class Log
{
	private static List<LogChannel> channelsToLog = new List<LogChannel> {
		//LogChannel.EDITOR_SETUP,
		//LogChannel.CHARACTER_MOVEMENT,
		//LogChannel.CHARACTER_VISION,
		//LogChannel.INPUT,
		//LogChannel.DEBUG
	};
	
	public static bool IsChannelLogging(LogChannel channel)
	{
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
			Log.channelsToLog.Add(channel);
	}
	
	public static void RemoveChannel(LogChannel channel)
	{
		if (!IsChannelLogging(channel))
			Debug.LogWarning("A channel that's already not being logged is trying to stop logging (" + channel + ")");
		else
			Log.channelsToLog.Remove(channel);
	}
}