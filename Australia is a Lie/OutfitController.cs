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
    public Sprite SetOutfit(ref bool isAgent, Constants.Genders gender, Constants.SkinColor skinColor)
    {
        bodyPartsWithPatterns = new List<BodyPart>();
        List<Structs.PartDetails> availableParts = bodyParts[0].partDetails.Where(part => (part.gender == gender || part.gender == Constants.Genders.Both) && !part.isDisabled).ToList();

        Structs.PartDetails selectedBodyPart = availableParts[CommonMethods.RandomizeIndex(availableParts)];
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

        hairController.SetHair(gender);
        faceController.SetFace(Constants.FaceType.Basic, GetComponent<Character>(), gender, skinColor);
        return bodyPartsWithPatterns[CommonMethods.RandomizeIndex(bodyPartsWithPatterns)].SetPattern(ref isAgent, gender);

    }
}