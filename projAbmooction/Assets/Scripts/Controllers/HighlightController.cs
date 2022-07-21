using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightController : MonoBehaviour
{
    [SerializeField] GameObject HighlightPrefab;
    GameObject LastObj;

    public void SetHighlight(Transform obj)
    {
        if (LastObj != null) Destroy(LastObj);
        LastObj = Instantiate
        (
            HighlightPrefab,
            obj
        );
    }
}
