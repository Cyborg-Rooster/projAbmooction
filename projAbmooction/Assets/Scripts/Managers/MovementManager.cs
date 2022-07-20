using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class MovementManager
{
    public Transform Transform;
    public RectTransform RectTransform;
    public Vector3 TargetPos;
    public Vector3 InitialPos;

    public float Speed;

    public bool IsParallax;

    public void Move(bool isRect)
    {
        if(isRect) RectTransform.anchoredPosition = Vector2.MoveTowards(RectTransform.anchoredPosition, TargetPos, Speed * Time.deltaTime);
        else Transform.position = Vector2.MoveTowards(Transform.position, TargetPos, Speed * Time.deltaTime);
    }

    public void ChangeFinalPosition(Vector2 finalPos)
    {
        TargetPos = finalPos;
    }

    public void ChangeSpeed(float speed)
    {
        Speed = speed;
    }

    public bool CheckIfItIsInTargetPosition(bool isRect)
    {
        if (isRect) return RectTransform.anchoredPosition == (Vector2)TargetPos;
        else return Transform.position == TargetPos;
    }
}
