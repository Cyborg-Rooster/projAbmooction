using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowController : MonoBehaviour
{
    [Header("Floating")]
    [SerializeField] float Amplitude;
    [SerializeField] float FloatSpeed;

    [Header("Horizontal Movement")]
    [SerializeField] float Speed;
    [SerializeField] Transform MinimumXPossible;
    [SerializeField] Transform MaximumXPossible;

    [Header("Controllers")]
    [SerializeField] GameController GameController;

    [Header("Itens Effect")]
    [SerializeField] GameObject ConfuseEffect;
    [SerializeField] GameObject ItemCaughtEffect;
    [SerializeField] GameObject DoubledCoinEffect;
    [SerializeField] GameObject MagneticEffect;
    [SerializeField] GameObject ShieldEffect;
    [SerializeField] GameObject SlowMotionEffect;

    Rigidbody2D Rigidbody;
    PlayerPhysicsManager Physics;
    Animator Animator;
    SpriteEffectController SpriteEffectController;
    CowEffectsManager CowEffectsManager;

    bool CanMove;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        SpriteEffectController = GetComponent<SpriteEffectController>();

        Physics = new PlayerPhysicsManager()
        {
            Speed = Speed,
            MinimumXPossible = MinimumXPossible.position.x,
            MaximumXPossible = MaximumXPossible.position.x,
            Amplitude = Amplitude,
            FloatSpeed = FloatSpeed,
            Rigidbody = Rigidbody,
            Transform = transform
        };

        CowEffectsManager = new CowEffectsManager()
        {
            CowController = this,
            ConfuseEffect = ConfuseEffect,
            ItemCaughtEffect = ItemCaughtEffect,
            DoubledCoinEffect = DoubledCoinEffect,
            MagneticEffect = MagneticEffect,
            ShieldEffect = ShieldEffect,
            SlowMotionEffect = SlowMotionEffect,
            MagneticCollider = GetComponent<CircleCollider2D>(),
            DefaultCollider = GetComponent<CapsuleCollider2D>()
        };
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Speed = Speed;
        Physics.Amplitude = Amplitude;
        Physics.FloatSpeed = FloatSpeed;
    }

    void FixedUpdate()
    {
        if (CanMove) Physics.Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CanMove) CowEffectsManager.CheckCollision(collision);
    }

    private void SetCanMove(bool move)
    {
        CanMove = move;
    }

    public void OnHit()
    {
        GameController.ShakeCamera();
        SpriteEffectController.BlinkBlank(0.125f);
    }

    public void StartPhase()
    {
        Animator.Play("floating");
        SetCanMove(true);
    }

    public void EndGame()
    {
        SetCanMove(false);
        OnHit();
        Rigidbody.velocity = Vector2.zero;
        GameController.Finish();
    }

    public void SetPause(bool active)
    {
        SetCanMove(active);
        Animator.enabled = active;
    }

    public void AddCoins(int coins)
    {
        GameController.AddCoins(coins);
    }
}
