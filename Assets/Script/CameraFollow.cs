using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 offset;

    void Update()
    {
        transform.position = Target.position + offset;
        transform.position = Vector3.Lerp(transform.position, Target.position + offset, 1);
    }
}
