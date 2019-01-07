using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixingCar : ActivityLogicProgressive
{
	public override string ReturnEffectOfActivity(float time)
	{
		base.ReturnEffectOfActivity(time);
		return string.Format("{0}%", completionProgress);
	}

}