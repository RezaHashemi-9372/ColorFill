using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAbleBlock : MonoBehaviour
{
    [SerializeField, Range(0.0f, 15.0f)]
    private float speed = .8f;
    [SerializeField, Range(0.0f, 2.0f)]
    private float moveAmount = 0;
    public Vector3 direction = Vector3.forward;
    private Vector3 nextposition = Vector3.zero;
    private Vector3 originalPosition;
    private Vector3 destinationPos;
    public Color blockColor;


    private void Awake()
    {
        
    }
    void Start()
    {
        originalPosition = this.transform.position;
        destinationPos = this.transform.position + direction * moveAmount;
        nextposition = destinationPos;
        this.GetComponent<MeshRenderer>().material.color = Color.yellow;

    }

    void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, nextposition, speed * Time.deltaTime);

        if (Vector3.Distance(this.transform.position, nextposition) < .01f)
        {
            nextposition = nextposition == destinationPos ? originalPosition : destinationPos;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Cube>() && collision.collider.GetComponent<Cube>().boxtype == Cube.colorType.filled)
        {
            Destroy(this.gameObject);
        }
    }

    public void Setup(float moveamo,  Color bloColor)
    {
        this.moveAmount = moveamo;
        this.blockColor = bloColor;
    }
}
