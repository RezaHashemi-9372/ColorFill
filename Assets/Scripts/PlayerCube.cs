using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCube : MonoBehaviour
{
    [SerializeField, Range(0, 50)]
    private float speed = 10;

    private Vector3 startPoint = Vector3.zero;
    public Vector3 endpoint = Vector3.zero;
    public Vector3 direction = Vector3.zero;
    private bool shouldMove = false;
    public static bool isMoving = true;
    public Color cubecolor;
    public Color highlightColor;
    private GameMode gameMode;

    private void Awake()
    {
        this.GetComponent<MeshRenderer>().material.color = cubecolor;
        gameMode = FindObjectOfType<GameMode>();
    }
    private void Start()
    {
        Cube temp = RayCastFromTop();
        if (temp && temp.boxtype == Cube.colorType.unfilled)
        {
            temp.GetComponent<Cube>().Highlight(highlightColor, cubecolor);
            startPoint = temp.transform.position;
        }
    }
    private void Update()
    {
        if (shouldMove)
        {
            if (startPoint == Vector3.zero)
            {
                startPoint = this.transform.position;
            }
            this.transform.Translate(direction * speed * Time.deltaTime);
            
            Cube temp = RayCastFromTop();
            
            if (temp && temp.boxtype == Cube.colorType.unfilled)
            {
                temp.GetComponent<Cube>().Highlight(highlightColor, cubecolor);
            }


            GameObject obj = RayCastAround(direction);
            if (obj && obj.CompareTag("Wall"))
            {
                isMoving = false;
                shouldMove = false;
                endpoint = this.transform.position;
                Refill(startPoint, endpoint);
            }
            if (obj && obj.CompareTag("Gate"))
            {
                isMoving = false;
                shouldMove = false;
                endpoint = this.transform.position;
                Refill(startPoint, endpoint);
            }

            if (obj && obj.CompareTag("Cube"))
            {

                if (obj.GetComponent<Cube>().boxtype == Cube.colorType.unfilled)
                {
                    isMoving = true;
                }
                if (obj.GetComponent<Cube>().boxtype == Cube.colorType.filled)
                {
                    isMoving = false;
                    endpoint = this.transform.position;
                    Refill(startPoint, endpoint);
                }
                
                if (obj.GetComponent<Cube>().boxtype == Cube.colorType.highlight)
                {
                    Debug.Log("The section to chec for Restart()");
                    gameMode.Restart();
                }
                
            }

            
        }
        
    }
    
                
    public void Move(Vector3 dir)
    {
        direction = dir;
        shouldMove = true;
        isMoving = true;

        GameObject rayObj = RayCastAround(dir);

        if (rayObj && rayObj.CompareTag("Cube"))
        {
            this.transform.position = rayObj.transform.position;
            if (rayObj.CompareTag("Cube") && rayObj.GetComponent<Cube>().boxtype == Cube.colorType.filled)
            {
                isMoving = false;
            }
            if (rayObj.CompareTag("Cube") && rayObj.GetComponent<Cube>().boxtype == Cube.colorType.unfilled)
            {
                this.transform.position = rayObj.transform.position;
                shouldMove = true;
                isMoving = true;

            }
        }

        if (rayObj && rayObj.CompareTag("Wall") )
        {
            isMoving = false;
            shouldMove = false;
        }
        if (rayObj && rayObj.CompareTag("Gate"))
        {
            isMoving = false;
            shouldMove = false;
        }
        if (rayObj && rayObj.CompareTag("Block"))
        {
            isMoving = false;
            shouldMove = false;
        }

    }

    private GameObject RayCastAround(Vector3 dir)
    {
        RaycastHit hit;

        if (Physics.Raycast(this.transform.position, dir, out hit))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private Cube RayCastFromTop()
    {
        RaycastHit hit;

        if (Physics.Raycast(new Vector3(this.transform.position.x,
            this.transform.position.y + 1, this.transform.position.z), Vector3.down, out hit))
        {
            return hit.collider.GetComponent<Cube>();
        }
        return null;
    }


    private void Refill(Vector3 start, Vector3 end)
    {
        if (start == Vector3.zero || end == Vector3.zero)
        {
            return;
        }

        if (start.x < end.x && start.z < end.z)
        {
            Vector3[] dir = new Vector3[] { Vector3.right, Vector3.forward };
            Raycast(dir);
        }
        if (start.x > end.x && start.z < end.z)
        {
            Vector3[] dir = new Vector3[] { Vector3.left, Vector3.forward };
            Raycast(dir);
        }
        if (start.x > end.x && start.z > end.z)
        {
            Vector3[] dir = new Vector3[] { Vector3.left, Vector3.back };
            Raycast(dir);
        }
        if (start.x < end.x && start.z > end.z)
        {
            Vector3[] dir = new Vector3[] { Vector3.right, Vector3.back };
            Raycast(dir);
        }

        startPoint = new Vector3();
        endpoint = new Vector3();
        
    }

    private void Raycast(Vector3[] dirs)
    {
        RaycastHit hit;

        for (int i = 0; i < dirs.Length; i++)
        {
            Physics.Raycast(startPoint, dirs[i], out hit);

            if (hit.collider)
            {
                if (hit.collider.CompareTag("Cube") && hit.collider.GetComponent<Cube>().boxtype == Cube.colorType.unfilled)
                {
                    hit.collider.GetComponent<Cube>().FillIt();
                    hit.collider.GetComponent<Cube>().Raycast(dirs);
                }
            }
            else if (hit.collider && hit.collider.CompareTag("Block"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Block"))
        {
            gameMode.Restart();
        }
    }


}
