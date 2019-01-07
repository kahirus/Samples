using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartGraphicOrder : MonoBehaviour
{

    public int ChangeOrderLayer;
    public bool isPatternAboveBodyPart;
    public void SetOrder()
    {
        int increment = isPatternAboveBodyPart ? 1 : -1;
        GetComponent<SpriteRenderer>().sortingOrder += ChangeOrderLayer;
        if (GetComponent<BodyPart>() && GetComponent<BodyPart>().pattern)
        {
            GetComponent<BodyPart>().pattern.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + increment;
        }
    }
}