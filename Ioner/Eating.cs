using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eating : ActivityLogicBase
{

    int hourlyFoodEatenRate = -6;
    public override string ReturnEffectOfActivity (float time)
    {
        return hourlyFoodEatenRate * time + " food";
    }

    public override void ExecuteActivityLogic (float time)
    {
        GameManager.instance.FoodCount += time * hourlyFoodEatenRate;
    }

    public override bool CheckLimit (CharacterData charData)
    {
        return true;
    }
}