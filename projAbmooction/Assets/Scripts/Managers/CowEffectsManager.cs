using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CowEffectsManager : CowEffects
{
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

    private void CheckObstacleCollision(Collider2D collision)
    {
        if (collision.IsTouching(DefaultCollider) && !shielded)
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
                else if (collision.name == "DoubleCoins(Clone)") Doubled = SetEffect(Doubled, doubled, GetDoubleCoins(doubled));
                else if (collision.name == "Magnet(Clone)") Magnetic = SetEffect(Magnetic, magnetic, GetMagnetic(magnetic));
                else if (collision.name == "Shield(Clone)") Shielded = SetEffect(Shielded, shielded, GetShield(shielded));
                else if (collision.name == "SlowMotion(Clone)") SlowDown = SetEffect(SlowDown, slowDown, GetSlowDown());
                CowController.StartCoroutine(GetItem(collision.gameObject));
            }
        }
    }

    private Coroutine SetEffect(Coroutine coroutine,bool boolean, IEnumerator enumerator)
    {
        if (boolean) CowController.StopCoroutine(coroutine);
        coroutine = CowController.StartCoroutine(enumerator);
        return coroutine;
    }
}
