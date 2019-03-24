using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public void OnValidate()
    {
        bool wasSet = false;

        for (int i = 0; i < patternDetails.Count; i++)
        {
            Structs.PatternDetails temp = patternDetails[i];
            if (temp.sprite != null)
            {
                wasSet = true;
                bool isAgent = !temp.sprite.name.Contains("_civil_") ? true : false;

                temp.forGender = setGender(temp.sprite.name);
                temp.isAgent = isAgent;
                temp.isActive = !isAgent;
                temp.differenceType = isAgent ? parseDifferenceType(temp.sprite.name) : Constants.DifferenceType.Civil;
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
            Structs.PartDetails temp = partDetails[i];
            if (temp.sprite != null)
            {
                wasSet = true;
                bool isFemale = temp.sprite.name.Contains("_f_") ? true : false;
                Constants.SkinColor skin = temp.sprite.name.Contains("_b_") ? Constants.SkinColor.Black : Constants.SkinColor.White;
                temp.gender = isFemale ? Constants.Genders.Female : Constants.Genders.Male;
                temp.skinColor = skin;

                partDetails[i] = temp;
            }
        }
        if (wasSet)
        {
            partDetails.Sort((s1, s2) => s1.sprite.name.CompareTo(s2.sprite.name));
        }

        //CommonMethods.ValidateIfNoDuplicates(patternDetails);
        //CommonMethods.ValidateIfNoDuplicates(partDetails);
    }

    public Constants.BodyPartType partType;
    public List<Structs.PartDetails> partDetails;
    public List<Structs.PatternDetails> patternDetails;
    public SpriteRenderer pattern;

    public void SetBodyPart(int selectedSet)
    {
        GetComponent<SpriteRenderer>().sprite = partDetails[selectedSet].sprite;
        if (pattern)
        {
            pattern.sprite = null;
        }
    }

    public Sprite SetPattern(ref bool isAgent, Constants.Genders gender)
    {
        bool _isAgent = isAgent;
        List<Structs.PatternDetails> availablePatterns = patternDetails.Where(ptrn => ptrn.isAgent == _isAgent 
        && (ptrn.forGender == gender || ptrn.forGender == Constants.Genders.Both) 
        && ptrn.isActive).ToList();
        if (availablePatterns.Count == 0)
        {
            availablePatterns = patternDetails.Where(ptrn => ptrn.isAgent == !_isAgent && (ptrn.forGender == gender || ptrn.forGender == Constants.Genders.Both) && ptrn.isActive).ToList();
            isAgent = !isAgent;
        }
          Debug.Log(partType + "    " + availablePatterns.Count);
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

    Constants.DifferenceType parseDifferenceType(string spriteName)
    {
        foreach (string bodyPartType in Enum.GetNames(typeof(Constants.DifferenceType)))
        {
            if (spriteName.Contains(bodyPartType.ToLower()))
            {
                return (Constants.DifferenceType)Enum.Parse(typeof(Constants.DifferenceType), bodyPartType, true);
            }
        }
        return Constants.DifferenceType.Civil;
    }

    Constants.Genders setGender(string spriteName)
    {
        if (spriteName.Contains("_m_"))
            return Constants.Genders.Male;
        else if (spriteName.Contains("_f_"))
            return Constants.Genders.Female;
        else
            return Constants.Genders.Both;
    }
}