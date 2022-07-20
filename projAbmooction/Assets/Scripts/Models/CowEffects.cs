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
        yield return SetEffect(ConfuseEffect, Mechanics.ConfusedTime, null, false);
        Debug.Log("cabo a confusão");
        confused = false;
    }

    public IEnumerator GetDoubleCoins(bool restart)
    {
        doubled = true;
        yield return SetEffect(DoubledCoinEffect, Mechanics.DoubledTime, Strings.itemDouble, restart);
        doubled = false;
    }

    public IEnumerator GetMagnetic(bool restart)
    {
        magnetic = true;

        MagneticCollider.enabled = true;
        yield return SetEffect(MagneticEffect, Mechanics.MagneticTime, Strings.itemMagnetic, restart);
        MagneticCollider.enabled = false;

        magnetic = false;
    }

    public IEnumerator GetShield(bool restart)
    {
        shielded = true;
        yield return SetEffect(ShieldEffect, Mechanics.ShieldTime, Strings.itemShield, restart);
        shielded = false;
    }

    public IEnumerator GetSlowDown()
    {
        CowController.AddItemBox(Strings.itemSlowMotion);
        SpriteEffectController sec = SlowMotionEffect.GetComponent<SpriteEffectController>();

        /*SpriteEffectController effectController = GameObject.Instantiate
        (
            SlowMotionEffect
        ).GetComponent<SpriteEffectController>();*/

        if (!slowDown) 
        {
            Mechanics.CanSpeedUp = false;
            SlowMotionEffect.SetActive(true);
            Mechanics.SlowMotionLastRange = Mechanics.SpeedRange; 
        }
        else sec.StopEffects();

        slowDown = true;

        sec.ChangeAlphaNum(true);
        for (float range = Mechanics.SpeedRange; range > 1; range -= .2f)
        {
            Mechanics.SpeedRange = range;
            yield return 0.1f;
        }

        yield return new WaitForSeconds(Mechanics.SlowMotionTime);

        for (float range = Mechanics.SpeedRange; range < Mechanics.SlowMotionLastRange; range += .2f)
        {
            Mechanics.SpeedRange = range;
            yield return 0.1f;
        }

        sec.ChangeAlphaNum(false);
        yield return new WaitForSeconds(.5f);

        //GameObject.Destroy(effectController.gameObject);

        SlowMotionEffect.SetActive(false);
        Mechanics.CanSpeedUp = true;

        slowDown = false;
    }

    IEnumerator SetEffect(GameObject effectController, float time, string item, bool effect)
    {
        SpriteEffectController sec = effectController.GetComponent<SpriteEffectController>();
        if(item != null) CowController.AddItemBox(item);
        /*SpriteEffectController effectController = GameObject.Instantiate
        (
            effect,
            CowController.transform
        ).GetComponent<SpriteEffectController>();*/

        if (!effect) effectController.SetActive(true);
        else sec.StopEffects();

        yield return new WaitForSeconds(time / 2);

        sec.BlinkOppacity(time / 2);

        yield return new WaitForSeconds(time / 2);

        effectController.SetActive(false);

        //GameObject.Destroy(effectController.gameObject);
    }
}

    
