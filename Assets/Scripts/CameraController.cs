using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField, Range(0.0f, 50.0f)]
    private float speed = 2.0f;
    [SerializeField, Range(0.0f, 5.0f)]
    private float distance = 1.5f;

    void Update()
    {
        if (!target)
        {
            return;
        }
        this.transform.position = Vector3.Lerp(this.transform.position,
            new Vector3(this.transform.position.x,
            this.transform.position.y,
            target.transform.position.z - distance)
            , speed * Time.deltaTime);
    }
}
