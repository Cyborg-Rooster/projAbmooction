using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        SetRandomScale();
        if (gameObject.tag == "BreakableObstacle") 
        {
            animator = GetComponent<Animator>();
            animator.speed = 0; 
        }
    }

    private void SetRandomScale()
    {
        System.Random r = new System.Random();
        if (r.Next(0, 2) == 1) gameObject.transform.localScale = new Vector3(-1, 1, 1);
    }

    public void OnCollidingWithPlayer()
    {
        if (gameObject.tag == "BreakableObstacle") StartCoroutine(Break());
    }

    IEnumerator Break()
    {
        animator.speed = 1;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
