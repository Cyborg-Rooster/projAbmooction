using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class ItemController : MonoBehaviour
{
    bool Magnetic;

    Transform Target;
    MovementController MovementController;
    private void Start()
    {
        MovementController = GetComponent<MovementController>();
    }
    public void GetMagnetic(Transform target)
    {
        Target = target;
        Magnetic = true;
    }

    private void Update()
    {
        if (Magnetic) MovementController.ChangeTargetPositionAndMove(Target.position);
    }
}
