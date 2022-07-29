using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class VerticalParallaxController : MonoBehaviour
{
    [SerializeField] SpriteRenderer parallax1, parallax2;
    [SerializeField] Sprite stars;

    MovementController MovementController;

    private void Start()
    {
        MovementController = GetComponent<MovementController>();
    }

    public void Move()
    {
        MovementController.SetIsMoving(true);
    }

    public IEnumerator WaitForComeToTargetLocation()
    {
        MovementController.isParallax = false;
        MovementController.ChangeTargetPositionAndMove(new Vector3(0, -4.8f, 0));
        yield return MovementController.WaitForComeToTargetPosition();
    }

    public void TurnStars()
    {
        parallax1.sprite = stars;
        parallax2.sprite = stars;

        transform.position = new Vector3(0, 9.6f, 0);
        MovementController.ChangeTargetPositionAndMove(new Vector3(0, 0f, 0));
        MovementController.isParallax = true;
    }
}
