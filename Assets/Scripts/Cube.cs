using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField]
    public Color fillColor = Color.white;
    private bool isTriggered = false;
    public colorType boxtype = colorType.unfilled;
    private PlayerCube player;
    private GameMode gameMode;
    private Vector3[] directions = new Vector3[] { Vector3.right, Vector3.left, Vector3.up, Vector3.down};
    public int stage = 0;
    public enum colorType
    {
        unfilled = 0,
        highlight,
        filled
    }

    private void Awake()
    {
        player = FindObjectOfType<PlayerCube>();
        gameMode = FindObjectOfType<GameMode>();
    }

    void Update()
    {
        if (this.boxtype == colorType.highlight && !PlayerCube.isMoving)
        {
            FillIt();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("It's not player");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (this.boxtype == colorType.filled && other.GetComponent<MoveAbleBlock>())
        {
            Destroy(other.gameObject);
        }
    }


    public void Highlight(Color hilight, Color fillCol)
    {
        boxtype = colorType.highlight;
        Color temp = hilight;
        this.GetComponent<MeshRenderer>().material.color = temp;
        this.GetComponent<MeshRenderer>().enabled = true;
        this.fillColor = fillCol;
    }

    public void FillIt()
    {
        boxtype = colorType.filled;
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<MeshRenderer>().material.color = fillColor;
    }


    public void Raycast(Vector3[] dir)
    {
        RaycastHit hit;

        for (int i = 0; i < dir.Length; i++)
        {
            Physics.Raycast(this.transform.position, dir[i], out hit);
            if (hit.collider )
            {
                if (hit.collider.CompareTag("Cube") && hit.collider.GetComponent<Cube>().boxtype == colorType.unfilled)
                {
                    hit.collider.GetComponent<Cube>().FillIt();
                    hit.collider.GetComponent<Cube>().Raycast(dir);
                }

            }
            else if (hit.collider && hit.collider.CompareTag("Block"))
            {
                Destroy(hit.collider.gameObject);
            }
        }

        
    }
    public void Setup(Color fillColor)
    {
        this.fillColor = fillColor;
    }
}
