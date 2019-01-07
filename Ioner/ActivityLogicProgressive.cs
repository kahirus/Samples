using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityLogicProgressive : ActivityLogicBase
{

	public int difficulty;
	protected float completionProgress, currentCompletionProgress;
	public CurrentQuestsContainer questList;
	public GlobalQuestContainer globalQuestList;

	public override bool CheckLimit(CharacterData charData)
	{
		throw new System.NotImplementedException();
	}

	public override void ExecuteActivityLogic(float time)
	{
		CountEffect(time);
		currentCompletionProgress += completionProgress;
		CheckIfQuestProgress((int) currentCompletionProgress);
		if (currentCompletionProgress >= 100)
		{
			gameObject.SetActive(false);
		}
	}

	public override string ReturnEffectOfActivity(float time)
	{
		CountEffect(time);
		return "";
	}

	void CheckIfQuestProgress(int progress)
	{
		List<QuestBase> tempGlobalList = globalQuestList.allQuestsList;
		QuestBase temp;
		for (int i = 0; i <= tempGlobalList.Count; i++)
		{
			if (tempGlobalList[i].progressiveActivity == this)
			{
				if (tempGlobalList[i].wasAdded)
				{
					List<QuestBase> tempCurrentList = questList.currentQuestsList;
					for (int j = 0; j <= tempCurrentList.Count; j++)
					{
						if (tempCurrentList[i].progressiveActivity == this)
						{
							temp = tempCurrentList[i];
							temp.progress = progress;
							tempCurrentList[i] = temp;
							break;
						}
					}
					break;
				}
				else
				{
					temp = tempGlobalList[i];
					temp.progress = progress;
					tempGlobalList[i] = temp;
					break;
				}

			}

		}

	}

	void CountEffect(float time)
	{
		if (GameManager.instance.selectedObject)
		{
			CharacterData.SkillStruct skill = GameManager.instance.selectedObject.GetComponent<CharacterData>().SkillsList.Find(i => i.name == GetComponent<Activity>().usedSkill);
			float totalCompletionTime = (difficulty - skill.currentLvl) / 2f + 4;
			completionProgress = currentCompletionProgress + Mathf.RoundToInt(time / totalCompletionTime * 100);
			if (completionProgress > 100)
				completionProgress = 100;
		}
	}
}