using UnityEngine;
using System.Collections;

public class HiddenOnAwakeComponent : MonoBehaviour 
{
	// Use this for initialization
	void Awake () 
	{		
		if (Time.time < 0.1f)
		{
			Log.Warning("You left " + this.gameObject.name + " active. Deactivating at startup" +
				", but you should really do this in the editor", LogChannel.EDITOR_SETUP, this);
				this.gameObject.SetActive(false);
		}
	}
}
