using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField, Range(0.0f, 5.0f)]
    private float speed = .5f;
    private bool isOpened = false;
    private float nextpos;
    private void Start()
    {
        nextpos = this.transform.position.y - .06f;
    }
    private void Update()
    {
        if (isOpened)
        {
            this.GetComponent<BoxCollider>().enabled = false;
            this.transform.position = Vector3.Lerp(this.transform.position,
                new Vector3(this.transform.position.x, nextpos, this.transform.position.z), speed * Time.deltaTime);

            if (this.transform.position.y <= nextpos - .005f)
            {
                isOpened = false;
                this.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
    public void Open()
    {
        isOpened = true;
    }

}
