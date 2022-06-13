using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class PlayerPhysicsManager
{
    public float Speed;

    public Transform Transform;
    public Rigidbody2D Rigidbody;

    public float MinimumXPossible;
    public float MaximumXPossible;

    public float Amplitude;
    public float FloatSpeed;

    float HorizontalSpeed;

    public void Move()
    {
    #if !UNITY_EDITOR
        HorizontalSpeed = Input.acceleration.x * Speed * Time.deltaTime;
    #else
        HorizontalSpeed = Input.GetAxisRaw("Horizontal") * Speed * Time.deltaTime;
    #endif
        Rigidbody.velocity = new Vector2(HorizontalSpeed, 0f);

        if (Transform.position.x < MinimumXPossible) MovePosition(new Vector3(MaximumXPossible, Transform.position.y, Transform.position.z));
        else if (Transform.position.x > MaximumXPossible) MovePosition(new Vector3(MinimumXPossible, Transform.position.y, Transform.position.z));

        FloatObject();
    }

    public void FloatObject()
    {
        /*Vector3 FloatPosition = new Vector3
        (
            Transform.position.x,
            Amplitude * Mathf.Sin(FloatSpeed * Time.time),
            Transform.position.z
        );*/
        Transform.position = new Vector3(Transform.position.x, Amplitude * Mathf.Sin(FloatSpeed * Time.time), Transform.position.z);
    }

    private void MovePosition(Vector3 position)
    {
        Transform.position = position;
    }

}
