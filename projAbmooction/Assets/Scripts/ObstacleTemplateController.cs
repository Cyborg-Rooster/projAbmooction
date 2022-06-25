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
        MovementController.SetSpeed(GameData.ObstacleSpeed);
    }

    private void Update()
    {
        if (Started && !Spawned && transform.localPosition.y > YPositionToSpawnNextTemplate) SpawnNextTemplate();
        if (transform.localPosition.y >  YPositionToDestroySelf) Destroy(gameObject);
        if (Started && GameData.Phase == GamePhase.OnFinish) FinishRoutine();
    }

    private void SpawnNextTemplate()
    {
        MovementController.SetSpeed(GameData.ObstacleSpeed);
        Parent.SpawnNextTemplate();
        Spawned = true;
    }

    private void FinishRoutine()
    {
        Started = false;
        MovementController.SetIsMoving(false);
    }
}
