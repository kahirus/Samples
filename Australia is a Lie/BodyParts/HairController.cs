using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairController : MonoBehaviour
{

    public List<HairWithPosition> male, female, hairs;
    SpriteRenderer spriteRenderer;
    public SpriteRenderer backHairObject;

    [System.Serializable]
    public struct HairWithPosition
    {
        public Sprite hair;
        public float positionY;
        public Sprite backHair;
        public float positionYBack;
        public bool isActive, isFemale;
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetHair(bool isFemale)
    {
        spriteRenderer.sortingOrder = GetComponentInParent<SpriteRenderer>().sortingOrder++;
        HairWithPosition hair = isFemale ? RandomizeIndex(female) : RandomizeIndex(male);
        spriteRenderer.sprite = hair.hair;
        transform.localPosition = new Vector3(0, hair.positionY, -1.5f);
        bool hasBackHair = hair.backHair != null;
        backHairObject.gameObject.SetActive(hasBackHair);
        backHairObject.sprite = hair.backHair;
        backHairObject.gameObject.transform.localPosition = new Vector3(0, hair.positionYBack, -1.5f);

    }

    HairWithPosition RandomizeIndex(List<HairWithPosition> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}