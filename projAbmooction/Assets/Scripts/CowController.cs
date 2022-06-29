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

    Rigidbody2D Rigidbody;
    PlayerPhysicsManager Physics;
    Animator Animator;
    SpriteEffectController SpriteEffectController; 

    bool Started;
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
        if (Started) Physics.Move();
    }

    void SetStarted(bool started)
    {
        Started = started;
    }

    public void StartPhase()
    {
        Animator.Play("floating");
        SetStarted(true);
    }

    public void EndGame()
    {
        SetStarted(false);
        Rigidbody.velocity = Vector2.zero;
        SpriteEffectController.BlinkBlank(0.125f);
        GameController.Finish();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Started)
        {
            if (collision.tag == "Obstacle") EndGame();
            else if (collision.tag == "BreakableObstacle") GetConfused(collision.gameObject);
        }
    }

    private void GetConfused(GameObject obstacle)
    {
        obstacle.GetComponent<ObstacleController>().OnCollidingWithPlayer();
    }
}
