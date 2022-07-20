using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        UIManager.SetRandomScale(gameObject);
        if (gameObject.tag == "BreakableObstacle") 
        {
            animator = GetComponent<Animator>();
            animator.speed = 0; 
        }
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
