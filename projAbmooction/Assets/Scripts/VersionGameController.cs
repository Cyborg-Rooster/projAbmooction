using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionGameController : MonoBehaviour
{
    void Start()
    {
        UIManager.SetText(gameObject, $"v{Application.version}");
    }
}
