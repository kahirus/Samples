using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FaceController : MonoBehaviour
{

    public List<Structs.Face> faces;
    SpriteRenderer spriteRenderer;

    private void OnValidate()
    {
        for (int i = 0; i < faces.Count; i++)
        {
            Structs.Face temp = faces[i];
            if (temp.sprite != null)
            {
                bool isFemale = temp.sprite.name.Contains("_f_") ? true : false;
                if (temp.sprite.name.Contains("_basic_"))
                    temp.type = Constants.FaceType .Basic;
                else if (temp.sprite.name.Contains("_happy_"))
                    temp.type = Constants.FaceType .Happy;
                else if (temp.sprite.name.Contains("_o_"))
                    temp.type = Constants.FaceType .O;
                else if (temp.sprite.name.Contains("_sad_"))
                    temp.type = Constants.FaceType .Sad;
                temp.skinColor = temp.sprite.name.Contains("_b_") ? Constants.SkinColor.Black : Constants.SkinColor.White;
                temp.gender = isFemale ? Constants.Genders.Female : Constants.Genders.Male;
                faces[i] = temp;
            }
        }
        faces.Sort((s1, s2) => s1.sprite.name.CompareTo(s2.sprite.name));

    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetFace(Constants.FaceType face, Character chr, Constants.Genders gender, Constants.SkinColor skinColor)
    {
        chr.currentFace = face;
        spriteRenderer.sortingOrder = GetComponentInParent<SpriteRenderer>().sortingOrder++;
        List<Structs.Face> availableFaces = faces.Where(x => x.gender == gender && x.type == face && x.skinColor == skinColor).ToList();
        spriteRenderer.sprite = availableFaces[CommonMethods.RandomizeIndex(availableFaces)].sprite;
    }
}
