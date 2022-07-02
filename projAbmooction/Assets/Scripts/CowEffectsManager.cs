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
    public GameObject ItemCaughtEffect;
    public GameObject DoubledCoinEffect;
    public GameObject MagneticEffect;

    public CircleCollider2D MagneticCollider;
    public CapsuleCollider2D DefaultCollider;

    private Coroutine Confused;
    private Coroutine Doubled;
    private Coroutine Magnetic;

    private bool confused = false;
    private bool doubled = false;
    private bool magnetic = false;

    public void CheckCollision(Collider2D collision)
    {
        CheckObstacleCollision(collision);
        CheckItemCollision(collision);
    }

    IEnumerator GetItem(GameObject item)
    {
        GameObject.Destroy(item);
        GameObject effect = GameObject.Instantiate
        (
            ItemCaughtEffect,
            CowController.transform
        );
        yield return new WaitForSeconds(0.625f);
        GameObject.Destroy(effect);
    }

    #region "Effects"

    private void GetCoin()
    {
        if (doubled) CowController.AddCoins(2);
        else CowController.AddCoins(1);
    }

    IEnumerator GetConfused(GameObject obstacle)
    {
        CowController.OnHit();
        obstacle.GetComponent<ObstacleController>().OnCollidingWithPlayer();
        confused = true;
        yield return SetEffect(ConfuseEffect, GameData.ConfusedTime);
        Debug.Log("cabo a confusão");
        confused = false;
    }

    IEnumerator GetDoubleCoins()
    {
        doubled = true;
        yield return SetEffect(DoubledCoinEffect, GameData.DoubledTime);
        doubled = false;
    }

    IEnumerator GetMagnetic()
    {
        magnetic = true;

        MagneticCollider.enabled = true;
        yield return SetEffect(MagneticEffect, GameData.MagneticTime);
        MagneticCollider.enabled = false;

        magnetic = false;
    }

    IEnumerator SetEffect(GameObject effect, float time)
    {
        SpriteEffectController effectController = GameObject.Instantiate
        (
            effect,
            CowController.transform
        ).GetComponent<SpriteEffectController>();

        yield return new WaitForSeconds(time / 2);

        effectController.BlinkOppacity(time / 2);

        yield return new WaitForSeconds(time / 2);

        GameObject.Destroy(effectController.gameObject);
    }
    #endregion

    private void CheckObstacleCollision(Collider2D collision)
    {
        if (collision.IsTouching(DefaultCollider))
        {
            if (collision.CompareTag("Obstacle")) CowController.EndGame();
            else if (collision.CompareTag("BreakableObstacle"))
            {
                if (!confused)
                    Confused = CowController.StartCoroutine(GetConfused(collision.gameObject));
                else CowController.EndGame();
            }
        }
    }

    private void CheckItemCollision(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            if (collision.IsTouching(MagneticCollider))
            {
                collision.transform.parent = CowController.transform.parent;
                collision.GetComponent<ItemController>().GetMagnetic(CowController.transform);
            }
            if(collision.IsTouching(DefaultCollider))
            {
                if (collision.name == "Coin(Clone)") GetCoin();
                else if (collision.name == "DoubleCoins(Clone)") Doubled = CowController.StartCoroutine(GetDoubleCoins());
                else if (collision.name == "Magnet(Clone)") Magnetic = CowController.StartCoroutine(GetMagnetic());
                else if (collision.name == "Shield(Clone)")
                {
                }
                else if (collision.name == "SlowMotion(Clone)")
                {
                }
                CowController.StartCoroutine(GetItem(collision.gameObject));
            }
        }
    }
}
