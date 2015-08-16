using UnityEngine;
using System.Collections;

public class LayerAccessor
{
	public static int DEFAULT {get; private set;}
	public static int BLOCKS_NOTHING {get; private set;}
	public static int BLOCKS_MOVEMENT {get; private set; }
	public static int BLOCKS_SIGHT { get; private set; }
	public static int BLOCKS_BOTH { get; private set; }

	public static void InitLayerInformation()
	{
		DEFAULT = LayerMask.NameToLayer("Default");
		BLOCKS_NOTHING = LayerMask.NameToLayer ("BlocksNothing");
		BLOCKS_SIGHT = LayerMask.NameToLayer ("BlocksSight");
		BLOCKS_MOVEMENT = LayerMask.NameToLayer("BlocksMovement");
		BLOCKS_BOTH = LayerMask.NameToLayer("BlocksBoth");
	}
}
