using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class SpawnerController : MonoBehaviour
{
    public bool SpawnOnStart;
    [SerializeField] GameObject[] Objects;
    private void Start()
    {
        if (SpawnOnStart) SpawnObject(ReturnRandomObject());
    }

    private GameObject ReturnRandomObject()
    {
        GameObject obj = Objects[UnityEngine.Random.Range(0, Objects.Length)];
        if (!Mechanics.CanSpawnBox() && obj.CompareTag("Box"))
        {
            do obj = Objects[UnityEngine.Random.Range(0, Objects.Length)];
            while (obj.CompareTag("Box"));
        }
        return obj;
    }

    private void SpawnObject(GameObject obj)
    {
        SpawnerManager.SpawnObject(obj, transform);
    }

    public void SpawnNextTemplate()
    {
        SpawnObject(ReturnRandomObject());
    }
}
