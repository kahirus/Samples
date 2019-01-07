using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OutfitController : MonoBehaviour
{
    public List<BodyPart> bodyParts;
    public HairController hairController;
    public FaceController faceController;
    int selectedSet;
    List<BodyPart> bodyPartsWithPatterns;
    public Sprite SetOutfit(ref bool isAgent, bool isFemale, Character.SkinColor skinColor)
    {
        bodyPartsWithPatterns = new List<BodyPart>();
        List<BodyPart.PartDetails> availableParts = bodyParts[0].partDetails.Where(part => part.isFemale == isFemale && !part.isDisabled).ToList();

        BodyPart.PartDetails selectedBodyPart = availableParts[CommonMethods.RandomizeIndex(availableParts)];
        selectedSet = bodyParts[0].partDetails.IndexOf(selectedBodyPart);

        foreach (BodyPart bodyPart in bodyParts)
        {
            bodyPart.SetBodyPart(selectedSet);
            if (bodyPart.pattern)
            {
                bodyPartsWithPatterns.Add(bodyPart);
            }
        }
        //Na razie random 1 elementu

        hairController.SetHair(isFemale);
        faceController.SetFace(FaceController.FaceType.Basic, GetComponent<Character>(), isFemale, skinColor);
        return bodyPartsWithPatterns[CommonMethods.RandomizeIndex(bodyPartsWithPatterns)].SetPattern(ref isAgent, isFemale);

    }
}