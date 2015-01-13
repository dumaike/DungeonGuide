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
		LogChannel.INPUT,
		LogChannel.DEBUG
	};
	
	public static bool IsChannelActive(LogChannel channel)
	{
		if (Log.channelsToLog.Contains(channel))
		{
			return true;
		}
		
		return false;
	}
	
	public static void Print(string message, LogChannel channel = LogChannel.DEBUG, Object context = null)
	{
		if (IsChannelActive(channel))
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
}