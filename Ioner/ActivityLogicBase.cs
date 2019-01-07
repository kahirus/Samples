using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivityLogicBase : MonoBehaviour
{
    public abstract string ReturnEffectOfActivity (float time);
    public abstract void ExecuteActivityLogic (float time);
    public abstract bool CheckLimit (CharacterData charData);
}