using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityCamp : Activity
{
	public ActivityLogicBase activityLogic;

	public int reducedSatiationPerHour, reducedTirednessPerHour;

	protected override void Start()
	{
		base.Start();
		activityLogic = GetComponent<ActivityLogicBase>();
	}
	public override void ExecuteActivity()
	{
		CheckSkillProgress();
		activityLogic.ExecuteActivityLogic(startingTime);
		base.ExecuteActivity();

	}
	public void CheckSkillProgress()
	{
		List<CharacterData.SkillStruct> skillsList = assignedCharacter.GetComponent<CharacterData>().SkillsList;
		for (int i = 0; i < skillsList.Count; i++)
		{
			if (skillsList[i].name == usedSkill)
			{
				var copy = skillsList[i];
				copy.timeToNextLvl -= startingTime;
				assignedCharacter.GetComponent<Character>().CheckForLevelUpSkill(ref copy, ref copy.usedAttribute);
				skillsList[i] = copy;
			}
		}
	}

	public int RecountHunger(float time)
	{
		return (int) (time * reducedSatiationPerHour);
	}

	public int RecountTiredness(float time)
	{
		return (int) (time * reducedTirednessPerHour);
	}

	public int FindLongestPossibleTimeBasedOnSatiationAndTiredness(CharacterData charData)
	{
		int multiplierSatiation = -1 * charData.Satiation / reducedSatiationPerHour;
		int multiplierTiredness = -1 * charData.Tiredness / reducedTirednessPerHour;

		if (multiplierSatiation <= 0)
		{
			return multiplierTiredness;
		}
		else if (multiplierTiredness <= 0)
		{
			return multiplierSatiation;
		}
		else if (multiplierSatiation < multiplierTiredness)
		{
			return multiplierSatiation;
		}
		else
		{
			return multiplierTiredness;
		}
	}

	public void ReduceCharacterPrimaryBarsAndSkillTime()
	{
		CharacterData temp = assignedCharacter.GetComponent<CharacterData>();
		int passedTime = (int) (startingTime - timeLeft);
		temp.Satiation += passedTime * reducedSatiationPerHour;
		temp.Tiredness += passedTime * reducedTirednessPerHour;
	}
}