using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Vector3 finalPosition;

    public bool isParallax;
    public bool isMoving;
    public bool keepX, keepY;
    public bool isRect;

    public float speed;

    Movement Movement;

    // Start is called before the first frame update
    void Awake()
    {
        KeepCoordinade(isRect);
        if(isRect)
            Movement = new Movement()
            {
                RectTransform = GetComponent<RectTransform>(),
                IsParallax = isParallax,
                Speed = speed,
                TargetPos = finalPosition,
                InitialPos = GetComponent<RectTransform>().anchoredPosition
            };
        else
            Movement = new Movement()
            {
                Transform = transform,
                IsParallax = isParallax,
                Speed = speed,
                TargetPos = finalPosition,
                InitialPos = transform.position
            };
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving) Move(isRect);
        if (ReturnIfItIsInTargetPosition()) OnMovementFinish();
    }

    void Move(bool isRect)
    {
        Movement.Move(isRect);
    }

    void OnMovementFinish()
    {
        if (isParallax) transform.position = Movement.InitialPos;
        else isMoving = false;
    }

    public void KeepCoordinade(bool isRect)
    {
        if (isRect)
        {
            if (keepX) finalPosition.x = GetComponent<RectTransform>().anchoredPosition.x;
            if (keepY) finalPosition.y = GetComponent<RectTransform>().anchoredPosition.y;
        }
        else
        {
            if (keepX) finalPosition.x = transform.position.x;
            if (keepY) finalPosition.y = transform.position.y;
        }
    }

    public bool ReturnIfItIsInTargetPosition()
    {
        return Movement.CheckIfItIsInTargetPosition(isRect);
    }

    public void ChangeTargetPositionAndMove(Vector3 target)
    {
        finalPosition = target;
        KeepCoordinade(isRect);
        Movement.ChangeFinalPosition(finalPosition);
        isMoving = true;
    }

    public void SetIsMoving(bool moving)
    {
        isMoving = moving;
    }

    public void ReturntoStartPosition(bool isRect)
    {
        if (isRect) GetComponent<RectTransform>().anchoredPosition = Movement.InitialPos;
        else transform.position = Movement.InitialPos;
    }

    public IEnumerator WaitForComeToTargetPosition()
    {
        while (!ReturnIfItIsInTargetPosition())
            yield return null;
    }
}
