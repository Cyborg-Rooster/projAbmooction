using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager
{
    public static void SpawnObject(GameObject obj, Transform parent)
    {
        Object.Instantiate(obj, parent);
    }
}
