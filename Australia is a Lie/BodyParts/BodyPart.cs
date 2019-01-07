using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public enum Type
    {
        Torso,
        ArmLeft,
        ArmRight,
        Neck,
        Head,
        LegLeft,
        LegRight
    };

    [System.Serializable]
    public struct PartDetails
    {
        public Sprite sprite;
        public bool isFemale, isDisabled, isLocked;
        public Character.SkinColor skinColor;
    }

    [System.Serializable]
    public struct PatternDetails
    {
        public Sprite sprite;
        public bool isAgent, isFemale, isActive, isLocked;
        public string ID;
        public Constants.DifferenceType differenceType;
    };

    private void OnValidate()
    {
        bool wasSet = false;
        //if(patternDetails[patternDetails.Count - 1].sprite != null)
        //{
        //    PatternDetails temp = patternDetails[patternDetails.Count - 1];
        //    bool isFemale = temp.sprite.name.Contains("_f_") ? true : false;
        //    bool isAgent = temp.sprite.name.Contains("_agent_") ? true : false;
        //    temp.isFemale = isFemale;
        //    temp.isAgent = isAgent;          
        //}
        for(int i = 0; i < patternDetails.Count; i++)
        {
            PatternDetails temp = patternDetails[i];
            if (temp.sprite != null)
            {
                wasSet = true;
                bool isFemale = temp.sprite.name.Contains("_f_") ? true : false;
                bool isAgent = temp.sprite.name.Contains("_agent_") ? true : false;
                
                temp.ID = temp.ID ?? GenerateID(temp.sprite.name, isAgent, isFemale);
                temp.isFemale = isFemale;
                temp.isAgent = isAgent;
                temp.isActive = !isAgent;
                patternDetails[i] = temp;
            }
        }
        if (wasSet)
        {
            patternDetails.Sort((s1, s2) => s1.sprite.name.CompareTo(s2.sprite.name));
            wasSet = false;
        }
        for (int i = 0; i < partDetails.Count; i++)
        {
            PartDetails temp = partDetails[i];
            if (temp.sprite != null)
            {
                wasSet = true;
                bool isFemale = temp.sprite.name.Contains("_f_") ? true : false;
                Character.SkinColor skin = temp.sprite.name.Contains("_b_") ? Character.SkinColor.Black : Character.SkinColor.White;
                temp.isFemale = isFemale;
                temp.skinColor = skin;
                partDetails[i] = temp;
            }
        }
        if (wasSet)
        {
            partDetails.Sort((s1, s2) => s1.sprite.name.CompareTo(s2.sprite.name));
        }

        CommonMethods.ValidateIfNoDuplicates(patternDetails);
        CommonMethods.ValidateIfNoDuplicates(partDetails);
    }

    public Type partType;
    public List<PartDetails> partDetails;
    public List<PatternDetails> patternDetails;
    public SpriteRenderer pattern;

    public void SetBodyPart(int selectedSet)
    {
        GetComponent<SpriteRenderer>().sprite = partDetails[selectedSet].sprite;
        if (pattern)
        {
            pattern.sprite = null;
        }
    }

    public Sprite SetPattern(ref bool isAgent, bool isFemale)
    {
        bool _isAgent = isAgent;
        List<PatternDetails> availablePatterns = patternDetails.Where(ptrn => ptrn.isAgent == _isAgent && ptrn.isFemale == isFemale && ptrn.isActive).ToList();
        if(availablePatterns.Count == 0)
        {
            availablePatterns = patternDetails.Where(ptrn => ptrn.isAgent == !_isAgent && ptrn.isFemale == isFemale && ptrn.isActive).ToList();
            isAgent = !isAgent;
        }
      //  Debug.Log(partType + "    " + availablePatterns.Count);
        pattern.sprite = availablePatterns[CommonMethods.RandomizeIndex(availablePatterns)].sprite;

        GetComponent<BodyPartGraphicOrder>().SetOrder();

        return pattern.sprite;

    }

    string GenerateID(string name, bool isAgent, bool isFemale)
    {
        string[] splitName = name.Split('_');
        string type = splitName[0];
        string female = isFemale ? "f" : "m";
        string agent = isAgent ? "a" : "c";
        string number = splitName.Last();
        return type + female + agent + number;

    }
}