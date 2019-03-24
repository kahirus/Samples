using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairController : MonoBehaviour
{

    public List<Structs.HairWithPosition> male, female;
    SpriteRenderer spriteRenderer;
    public SpriteRenderer backHairObject;

    private void OnValidate()
    {
        for (int i = 0; i < female.Count; i++)
        {
            Structs.HairWithPosition temp = female[i];
            if (temp.hair != null)
            {
                temp.isFemale = true;
                female[i] = temp;
            }
        }
        //hair.Sort((s1, s2) => s1.sprite.name.CompareTo(s2.sprite.name));

    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetHair(Constants.Genders gender)
    {
        spriteRenderer.sortingOrder = GetComponentInParent<SpriteRenderer>().sortingOrder++;
        Structs.HairWithPosition hair = gender == Constants.Genders.Female ? RandomizeIndex(female) : RandomizeIndex(male);
        spriteRenderer.sprite = hair.hair;
        transform.localPosition = new Vector3(hair.position.x, hair.position.y, -1.5f);
        bool hasBackHair = hair.backHair != null;
        backHairObject.gameObject.SetActive(hasBackHair);
        backHairObject.sprite = hair.backHair;
        backHairObject.gameObject.transform.localPosition = new Vector3(hair.positionBack.x, hair.positionBack.y, -1.5f);

    }

    Structs.HairWithPosition RandomizeIndex(List<Structs.HairWithPosition> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}