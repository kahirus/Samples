using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringFood : ActivityLogicBase
{

    double min, max, difference;
    CharacterData.SkillStruct skill;
    public override string ReturnEffectOfActivity (float time)
    {
        CountEffect (time);
        Debug.Log ((min) + "-" + (int) (min + difference));
        return "" + (int) (min) + "-" + (int) (min + difference);
    }

    public override void ExecuteActivityLogic (float time)
    {
        CountEffect (time);
        int effect = Random.Range ((int) min, (int) max);
        GameManager.instance.FoodCount += effect;
    }

    public override bool CheckLimit (CharacterData charData)
    {
        return true;
    }

    void CountEffect (float time)
    {
        if (GameManager.instance.selectedObject)
        {
            skill = GameManager.instance.selectedObject.GetComponent<CharacterData> ().SkillsList.Find (i => i.name == GetComponent<Activity> ().usedSkill);
            min = (skill.currentLvl / 10f + 2) * time;
            max = (skill.currentLvl / 10f + 3) * time;
            difference = (max - min) / 2 + 1;
        }
    }
}