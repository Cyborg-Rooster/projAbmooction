using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CowEffectsManager
{
    public CowController CowController;

    public GameObject ConfuseEffect;

    private Coroutine Confused;

    private bool confused = false;
    public void CheckCollision(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            CowController.EndGame();
        }
        else if (collision.CompareTag("BreakableObstacle"))
        { 
            if(!confused) 
                Confused = CowController.StartCoroutine(GetConfused(collision.gameObject)); 
            else CowController.EndGame();
        }
    }

    IEnumerator GetConfused(GameObject obstacle)
    {
        CowController.OnHit();
        obstacle.GetComponent<ObstacleController>().OnCollidingWithPlayer();
        confused = true;
        SpriteEffectController effect = GameObject.Instantiate
        (
            ConfuseEffect, 
            CowController.transform
        ).GetComponent<SpriteEffectController>();

        yield return new WaitForSeconds(GameData.ConfusedTime / 2);

        effect.BlinkOppacity(GameData.ConfusedTime / 2);

        yield return new WaitForSeconds(GameData.ConfusedTime / 2);

        GameObject.Destroy(effect.gameObject);
        Debug.Log("cabo a confusão");
        confused = false;
    }
}
