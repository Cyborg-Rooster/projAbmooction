using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionController : MonoBehaviour
{
    [SerializeField] GameObject Target;
    // Start is called before the first frame update
    void Awake()
    {
        transform.position = new Vector3
        (
            transform.position.x,
            Target.transform.position.y,
            transform.position.z
        );
    }
}
