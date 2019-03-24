using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Character : MonoBehaviour
{
    public InputManager inputManager;
    public OutfitController outfit;
    public FaceController faceController;
    public Constants.Genders gender;
    public bool isAgent, isMoving;
    private bool breathOut;
    public float incrementBreathRate, rotationSpeed;
    //z racji tego że nie wiem po co jest ten array to zrobiłem sobie swój
    public Transform[] bodyParts;
    public List<Transform> rotatingBodyparts;
    public Transform head;
    public float rotationScale = 8f;
    private float startingScale;
    public Vector3 lastTarget;
    public Constants.FaceType currentFace;
    public Sprite selectedPattern;
    public Constants.SkinColor skinColor;

    private void Start()
    {
        if (FindObjectOfType<TutorialController>().tutorialChar != this)
        {
            outfit = GetComponentInChildren<OutfitController>();
            startingScale = transform.localScale.x;
            isAgent = IsAgentOrFemale();
            gender = IsAgentOrFemale() ? Constants.Genders.Female : Constants.Genders.Male;
            skinColor = IsBlack() ? Constants.SkinColor.Black : Constants.SkinColor.White;
            selectedPattern = outfit.SetOutfit(ref isAgent, gender, skinColor);
            StartBreathing();
            faceController.SetFace(Constants.FaceType.Basic, this, gender, skinColor);
            lastTarget = transform.position;
        }
    }

    public bool MoveCharacter(Constants.Move direction, float time, float endPoint)
    {
        GetComponent<Transform>().DOMoveX(endPoint * (int) direction, time);
        GetComponent<Transform>().DOLocalRotate(Vector3.zero, 1);
        isMoving = true;
        SetBodyOrder();
        return isMoving;
    }

    public void StartBreathing()
    {
        StartCoroutine(asyncBreath());
    }

    private void FixedUpdate()
    {

        var nextTarget = Vector3.Lerp(lastTarget, transform.position, 0.1f);
        foreach (Transform t in rotatingBodyparts)
        {
            t.localEulerAngles = -transform.eulerAngles + new Vector3(0, 0,
                (transform.position.x - nextTarget.x > 0) ?
                (-Vector3.Distance(transform.position, nextTarget) * rotationScale) :
                (Vector3.Distance(transform.position, nextTarget) * rotationScale)
            );
        }
        head.localEulerAngles = -transform.eulerAngles + new Vector3(0, 0,
            (transform.position.x - nextTarget.x > 0) ?
            (Vector3.Distance(transform.position, nextTarget) * rotationScale) :
            (-Vector3.Distance(transform.position, nextTarget) * rotationScale)
        );

        lastTarget = nextTarget;
    }

    void ScaleUp() { transform.DOScale(new Vector3(1.015f, 1.015f, 1.015f), 1f).OnComplete(ScaleDown); }
    void ScaleDown() { transform.DOScale(new Vector3(0.985f, 0.985f, 0.985f), 1f).OnComplete(ScaleUp); }

    public void SetStartingPosition()
    {
        GetComponent<Transform>().localScale = new Vector2(1, 1);
        GetComponent<Transform>().localPosition = new Vector3(RandomXInQueue(), inputManager.characters.IndexOf(this) * 0.35f);
        GetComponent<Transform>().localEulerAngles = new Vector3(0, 0, 0);

    }
    public void OnCompleteMovement()
    {

        GetComponent<SpriteRenderer>().sortingOrder = -10;

        foreach (Transform bp in bodyParts)
        {
            bp.GetComponent<SpriteRenderer>().sortingOrder = -10;
        }
        inputManager.characters.Add(this);
        SetStartingPosition();

        StartCoroutine(waitForReturn());
        StartBreathing();

        faceController.SetFace(Constants.FaceType.Basic, this, gender, skinColor);

    }

    public void MovementBeforeRelease(float mouseX, float dragPositionOffset)
    {
        transform.localPosition = new Vector2(mouseX - dragPositionOffset, transform.localPosition.y);
        if (Mathf.Abs(transform.localPosition.x) < inputManager.safeDragZoneX)
        {
            if (Mathf.Abs(transform.localPosition.x) > inputManager.faceChangeZoneX)
            {
                if (currentFace == Constants.FaceType.O)
                {
                    if (transform.localPosition.x < 0)
                    {
                        faceController.SetFace(Constants.FaceType.Sad, this, gender, skinColor);
                    }
                    else
                    {
                        faceController.SetFace(Constants.FaceType.Happy, this, gender, skinColor);
                    }
                }
            }
            else if (currentFace == Constants.FaceType.Sad || currentFace == Constants.FaceType.Happy)
            {
                faceController.SetFace(Constants.FaceType.O, this, gender, skinColor);
            }

            if (transform.localRotation.z == 0)
            {
                transform.DOComplete();
            }
            transform.DOLocalRotate(new Vector3(0, 0, (mouseX - dragPositionOffset) * -rotationSpeed), 0.15f);
        }
        else
        {
            GetComponent<Transform>().DOLocalRotate(Vector3.zero, 0.6f);
        }
    }

    public void MovementInQueue(Vector3 positionInQueue)
    {
        GetComponent<Transform>().DOComplete();
        GetComponent<Transform>().DOLocalMove(new Vector3(RandomXInQueue(), positionInQueue.y), 0.5f);
        SetBodyOrder();
    }

    public bool IsAgentOrFemale()
    {
        if (Random.Range(0, 100) > 50)
        {
            return true;
        }
        return false;
    }

    bool IsBlack()
    {
        if (Random.Range(0, 100) > 75)
        {
            return true;
        }
        return false;
    }

    public void SetBodyOrder()
    {
        int index = inputManager.characters.IndexOf(this);
        GetComponent<SpriteRenderer>().sortingOrder = (inputManager.startingCharactersCount - index) * 10;

        foreach (Transform bp in bodyParts)
        {
            bp.GetComponent<SpriteRenderer>().sortingOrder = (inputManager.startingCharactersCount - index) * 10;
        }
        foreach (BodyPart bp in outfit.bodyParts)
        {
            if (bp.GetComponent<BodyPartGraphicOrder>())
                bp.GetComponent<BodyPartGraphicOrder>().SetOrder();
        }
        outfit.hairController.GetComponent<BodyPartGraphicOrder>().SetOrder();
        outfit.hairController.backHairObject.GetComponent<BodyPartGraphicOrder>().SetOrder();
        outfit.faceController.GetComponent<BodyPartGraphicOrder>().SetOrder();
    }
    public float RandomXInQueue()
    {
        return Random.Range(-100, 100) * 0.01f;
    }

    IEnumerator waitForReturn()
    {

        yield return new WaitForSeconds(0.4f);

        SetBodyOrder();
    }
    IEnumerator asyncBreath()
    {
        yield return new WaitForSeconds(0.3f);

        float randomed = Random.Range(0, 50) / 100f;
        yield return new WaitForSeconds(randomed);
        ScaleUp();

    }

}