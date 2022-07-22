using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinChangeController : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController[] AnimatorController;
    [SerializeField] Animator Cow;
    void Start()
    {
        Cow.runtimeAnimatorController = AnimatorController[GameData.Skin];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
