using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum LogChannel
{
	DEBUG, CHARACTER_MOVEMENT
};

public class Log
{
	private static List<LogChannel> channelsToLog = new List<LogChannel> {
		LogChannel.CHARACTER_MOVEMENT,
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
			Debug.Log(message, context);
		}
	}
	
	public static void Warning(string message, LogChannel channel = LogChannel.DEBUG, Object context = null)
	{
		Debug.LogWarning(message, context);
	}
	
	public static void Error(string message, LogChannel channel = LogChannel.DEBUG, Object context = null)
	{
		Debug.LogError(message, context);
	}
}