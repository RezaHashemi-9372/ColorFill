using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerCube player;
    private Vector3 startPoint = new Vector3();
    private Vector3 endPoint = new Vector3();

    private void Awake()
    {
        player = FindObjectOfType<PlayerCube>();
    }
    void Start()
    {
        
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.D))
        {
            player.Move(Vector3.right);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            player.Move(Vector3.left);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            player.Move(Vector3.forward);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            player.Move(Vector3.back);
        }

    }

}
