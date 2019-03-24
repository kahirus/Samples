using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class SpritesLoader : MonoBehaviour
{
    const string charactersFolder = "Assets/Graphics/Characters/";
    const string differencesFolder = "Differences", setsFolder = "Sets";
    [MenuItem("Custom/Fill bodyparts")]
    public static void FillBodyparts()
    {
        List<BodyPart> allBodyParts = FindObjectsOfType<BodyPart>().ToList();
        //testClasss test = FindObjectOfType<testClasss>();

        List<string> differencesSprites = FindAssetsPath(charactersFolder + differencesFolder);
        List<string> setsSprites = FindAssetsPath(charactersFolder + setsFolder);

        foreach (string bodyPartType in Enum.GetNames(typeof(Constants.BodyPartType)))
        {
            Debug.Log(bodyPartType.ToLower());

            List<BodyPart> currentBodyPartType = allBodyParts.Where(x => x.partType.ToString() == bodyPartType).ToList();

            foreach (BodyPart bp in currentBodyPartType)
            {
                Debug.Log(bp.name.ToString().ToLower());
                bp.patternDetails.Clear();
                bp.partDetails.Clear();
                List<string> differencesForPart = FindSetsForPart(differencesSprites, bp);
                List<string> setsForPart = FindSetsForPart(setsSprites, bp);

                foreach (string path in differencesForPart)
                {
                    bp.patternDetails.Add(new Structs.PatternDetails { sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path) });
                }
                foreach (string path in setsForPart)
                {
                    bp.partDetails.Add(new Structs.PartDetails { sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path) });
                }
                bp.OnValidate();
            }
        }
        //foreach(string s in assets)
        //{
        //    string path = AssetDatabase.GUIDToAssetPath(s);
        //    Debug.Log(path);
        //}

    }

    static List<string> FindAssetsPath(string folderPath)
    {
        string[] assets = AssetDatabase.FindAssets("t:sprite", new[] { folderPath });
        List<string> parsedPaths = new List<string>();
        foreach (string s in assets)
        {
            string path = AssetDatabase.GUIDToAssetPath(s);
            parsedPaths.Add(path.ToLower());
            Debug.Log(path);
        }
        return parsedPaths;
    }
    
    static List<string> FindSetsForPart(List<string> setsSpritesList, BodyPart bp)
    {
        return setsSpritesList.Where(x => x.ToLower().Contains(bp.partType.ToString().ToLower())).ToList();
    }

}
