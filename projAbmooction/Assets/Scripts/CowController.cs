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
            ConfuseEffect = ConfuseEffect
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CanMove) CowEffectsManager.CheckCollision(collision);
    }
}
