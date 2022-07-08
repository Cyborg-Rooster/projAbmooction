using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CowEffects
{
    public CowController CowController;

    public GameObject ConfuseEffect;
    public GameObject ItemCaughtEffect;
    public GameObject DoubledCoinEffect;
    public GameObject MagneticEffect;
    public GameObject ShieldEffect;
    public GameObject SlowMotionEffect;

    public CircleCollider2D MagneticCollider;
    public CapsuleCollider2D DefaultCollider;

    public Coroutine Confused;
    public Coroutine Doubled;
    public Coroutine Magnetic;
    public Coroutine Shielded;
    public Coroutine SlowDown;

    public bool confused = false;
    public bool doubled = false;
    public bool magnetic = false;
    public bool shielded = false;
    public bool slowDown = false;

    public void GetCoin()
    {
        if (doubled) CowController.AddCoins(2);
        else CowController.AddCoins(1);
    }

    public IEnumerator GetConfused(GameObject obstacle)
    {
        CowController.OnHit();
        obstacle.GetComponent<ObstacleController>().OnCollidingWithPlayer();
        confused = true;
        yield return SetEffect(ConfuseEffect, GameData.ConfusedTime);
        Debug.Log("cabo a confusão");
        confused = false;
    }

    public IEnumerator GetDoubleCoins()
    {
        doubled = true;
        yield return SetEffect(DoubledCoinEffect, GameData.DoubledTime);
        doubled = false;
    }

    public IEnumerator GetMagnetic()
    {
        magnetic = true;

        MagneticCollider.enabled = true;
        yield return SetEffect(MagneticEffect, GameData.MagneticTime);
        MagneticCollider.enabled = false;

        magnetic = false;
    }

    public IEnumerator GetShield()
    {
        shielded = true;
        yield return SetEffect(ShieldEffect, GameData.ShieldTime);
        shielded = false;
    }

    public IEnumerator GetSlowDown()
    {
        SpriteEffectController effectController = GameObject.Instantiate
        (
            SlowMotionEffect
        ).GetComponent<SpriteEffectController>();

        GameData.CanSpeedUp = false;

        if (!slowDown) GameData.SlowMotionLastRange = GameData.SpeedRange;

        slowDown = true;

        effectController.ChangeAlphaNum(true);
        for (float range = GameData.SpeedRange; range > 1; range -= .2f)
        {
            GameData.SpeedRange = range;
            yield return 0.1f;
        }

        yield return new WaitForSeconds(GameData.SlowMotionTime);

        for (float range = GameData.SpeedRange; range < GameData.SlowMotionLastRange; range += .2f)
        {
            GameData.SpeedRange = range;
            yield return 0.1f;
        }

        effectController.ChangeAlphaNum(false);
        yield return new WaitForSeconds(.5f);

        GameObject.Destroy(effectController.gameObject);
        GameData.CanSpeedUp = true;

        slowDown = false;
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
}

    
