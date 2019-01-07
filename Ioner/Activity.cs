using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activity : MonoBehaviour
{
    public Character assignedCharacter;
    public Transform characterPosition;
    public CharacterData.Skills usedSkill;
    public string activityName;
    public Sprite activityImage;

    [HideInInspector]
    public float timeLeft;
    public float startingTime;
    protected virtual void Start()
    {
        GetComponent<Clickable>().onClick.AddListener(ShowActivityDialog);
    }

    public virtual void ShowActivityDialog()
    {
        if (GameManager.instance.isCharacterSelected())
        {
            FindObjectOfType<UIController>().ShowActivityDialog(this);
        }
    }

    public virtual void AssignCharacter()
    {
        if (GameManager.instance.isCharacterSelected())
        {
            if (assignedCharacter)
            {
                assignedCharacter.ClearActivity();
            }
            GameManager.instance.selectedObject.GetComponent<Character>().AssignActivity(this);
            GetComponent<SpriteRenderer>().color = Color.yellow;
            assignedCharacter.SetActivityIconOnUI(this);
            assignedCharacter.DeselectCharacter();
        }
    }

    public virtual void ExecuteActivity()
    {
        assignedCharacter.ClearActivity();
        ClearAssignedCharacter();
    }

    public void ClearAssignedCharacter()
    {
        assignedCharacter = null;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}