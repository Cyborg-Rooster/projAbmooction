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
        return Objects[new System.Random().Next(0, Objects.Length)];
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
