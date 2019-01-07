using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FaceController : MonoBehaviour
{

    public List<Face> faces;
    SpriteRenderer spriteRenderer;

    [Serializable]
    public struct Face
    {
        public Sprite sprite;
        public FaceType type;
        public bool isFemale;
        public Character.SkinColor skinColor;
    }

    public enum FaceType
    {
        Basic,
        Happy,
        O,
        Sad
    }

    private void OnValidate()
    {
        for (int i = 0; i < faces.Count; i++)
        {
            Face temp = faces[i];
            if (temp.sprite != null)
            {
                bool isFemale = temp.sprite.name.Contains("_f_") ? true : false;
                if (temp.sprite.name.Contains("_basic_"))
                    temp.type = FaceType.Basic;
                else if (temp.sprite.name.Contains("_happy_"))
                    temp.type = FaceType.Happy;
                else if (temp.sprite.name.Contains("_o_"))
                    temp.type = FaceType.O;
                else if (temp.sprite.name.Contains("_sad_"))
                    temp.type = FaceType.Sad;
                temp.skinColor = temp.sprite.name.Contains("_b_") ? Character.SkinColor.Black : Character.SkinColor.White;
                temp.isFemale = isFemale;
                faces[i] = temp;
            }
        }
        faces.Sort((s1, s2) => s1.sprite.name.CompareTo(s2.sprite.name));

    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetFace(FaceType face, Character chr, bool isFemale, Character.SkinColor skinColor)
    {
        chr.currentFace = face;
        spriteRenderer.sortingOrder = GetComponentInParent<SpriteRenderer>().sortingOrder++;
        List<Face> availableFaces = faces.Where(x => x.isFemale == isFemale && x.type == face && x.skinColor == skinColor).ToList();
        spriteRenderer.sprite = availableFaces[CommonMethods.RandomizeIndex(availableFaces)].sprite;
    }
}
