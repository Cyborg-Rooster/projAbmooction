using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class PlanetController : MonoBehaviour
{
    [SerializeField] Sprite[] Sprites;
    [SerializeField] float[] PosX;

    SpriteRenderer SpriteRenderer;
    Vector3 StartPos;
    MovementController MovementController;

    private void Start()
    {
        StartPos = transform.position;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        MovementController = GetComponent<MovementController>();
        ConfigureObject();
    }

    private void Update()
    {
        if (transform.position.y <= MovementController.finalPosition.y + 0.2f) ConfigureObject();
    }

    private void ConfigureObject()
    {
        SpriteRenderer.sprite = Sprites[UnityEngine.Random.Range(0, Sprites.Length)];
        UIManager.SetRandomScale(gameObject);
        transform.position = StartPos;
        transform.position = new Vector3
        (
            PosX[UnityEngine.Random.Range(0, PosX.Length)],
            transform.position.y,
            transform.position.z
        );
    }

}
