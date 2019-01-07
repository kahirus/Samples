using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NextTurnController : MonoBehaviour
{

    public GameObject nextTurnDialog;
    public TextMeshProUGUI nextTurnDialogText;
    public TimeController timeController;
    public float fadeInTime = 0.5f, fadeOutTime = 0.5f, fadeInDelay = 0.25f;

    public void OpenNextTurnDialog ()
    {
        nextTurnDialogText.text = "";
        var characters = FindObjectsOfType<Character> ();
        int iterator = 0;
        foreach (Character c in characters)
        {
            if (c.currentActivity == null)
            {
                nextTurnDialogText.text += c.GetComponent<CharacterData> ().ID + ", ";
                iterator++;
            }
        }
        if (iterator > 1)
        {
            nextTurnDialogText.text = nextTurnDialogText.text.Remove (nextTurnDialogText.text.Length - 2) + " are unassigned, do you still want to continue?";
        }
        else
        {
            nextTurnDialogText.text = nextTurnDialogText.text.Remove (nextTurnDialogText.text.Length - 2) + " is unassigned, do you still want to continue?";
        }
        nextTurnDialog.SetActive (true);
    }

    public void StartNextTurn ()
    {
        Activity[] activities = FindObjectsOfType<Activity> ();
        List<Activity> queuedActivities = new List<Activity> ();
        foreach (Activity a in activities)
        {
            if (a.timeLeft > 0)
            {
                queuedActivities.Add (a);
            }
        }
        List<Activity> sortedActivities = queuedActivities.OrderBy (o => o.timeLeft).ToList ();
        float shortestLength = sortedActivities[0].timeLeft;
        for (int i = 0; i < sortedActivities.Count; i++)
        {
            queuedActivities[i].timeLeft -= shortestLength;
            if (queuedActivities[i].GetComponent<ActivityCamp> ())
            {
                queuedActivities[i].GetComponent<ActivityCamp> ().ReduceCharacterPrimaryBarsAndSkillTime ();
                queuedActivities[i].GetComponent<ActivityCamp> ().CheckSkillProgress ();
            }
            // TODO: Skutki wykonania czynności tutaj
            if (queuedActivities[i].timeLeft == 0)
            {
                queuedActivities[i].ExecuteActivity ();
            }
        }
        timeController.ChangeClock(shortestLength);
        //GameManager.instance.GlobalTime -= shortestLength;
        GameManager.instance.uiController.UpdateCharacterUI ();
        Debug.Log (GameManager.instance.GlobalTime);
    }

    public bool IsEveryoneAssigned ()
    {
        var characters = FindObjectsOfType<Character> ();
        foreach (Character c in characters)
        {
            if (c.currentActivity == null)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsAnyActivityAssigned ()
    {
        Activity[] activities = FindObjectsOfType<Activity> ();
        foreach (Activity a in activities)
        {
            if (a.timeLeft > 0)
            {
                return true;
            }
        }
        return false;
    }
}