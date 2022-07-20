using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class ObstacleTemplateController : MonoBehaviour
{
    public bool Started = true;

    public float YPositionToSpawnNextTemplate;
    public float YPositionToDestroySelf;

    SpawnerController Parent;
    MovementController MovementController;

    bool Spawned;

    private void Start()
    {
        Parent = transform.parent.GetComponent<SpawnerController>();
        MovementController = GetComponent<MovementController>();
    }

    private void Update()
    {
        MovementController.SetSpeed(Mechanics.ObstacleSpeed);

        if (transform.localPosition.y > YPositionToDestroySelf) Destroy(gameObject);
        if(Started)
        {
            if(!Spawned && transform.localPosition.y > YPositionToSpawnNextTemplate) SpawnNextTemplate();
            if(Mechanics.Phase == GamePhase.OnFinish) FinishRoutine();
        }
    }

    private void SpawnNextTemplate()
    {
        Parent.SpawnNextTemplate();
        Spawned = true;
    }

    private void FinishRoutine()
    {
        Started = false;
        MovementController.SetIsMoving(false);
    }
}
