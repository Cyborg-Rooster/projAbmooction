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

    Rigidbody2D Rigidbody;
    PlayerPhysicsManager Physics;
    Animator Animator;

    bool Started;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
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
        if (Started)
        {
            Physics.Move();
        }
    }

    public void StartPhase()
    {
        Started = true;
        Animator.Play("floating");
    }
}
