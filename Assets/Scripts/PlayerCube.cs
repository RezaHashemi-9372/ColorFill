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
            this.transform.Translate(direction * speed * Time.deltaTime);

            GameObject frontObj = RayCastAround(direction);

            if (frontObj && frontObj.CompareTag("Cube"))
            {
                
                 if (frontObj.GetComponent<Cube>().boxtype == Cube.colorType.highlight)

                {
                    Debug.Log("You hit the highlighed");
                    gameMode.Restart();
                }
                else if (frontObj.GetComponent<Cube>().boxtype == Cube.colorType.filled)
                {
                    isMoving = false;
                    endpoint = this.transform.position;
                    Refill(startPoint, endpoint);
                }

                else if (frontObj && frontObj.GetComponent<Cube>().boxtype == Cube.colorType.unfilled)
                {
                    if (startPoint == Vector3.zero && isMoving == false)
                    {
                        startPoint = this.transform.position;
                    }
                    else if (isMoving == false)
                    {
                        isMoving = true;
                    }
                    else if (shouldMove == false)
                    {
                        shouldMove = true;
                    }

                }
            }
            else
            {
                if (frontObj && frontObj.CompareTag("Wall"))
                {
                    isMoving = false;
                    shouldMove = false;
                    endpoint = this.transform.position;
                    Refill(startPoint, endpoint);
                }
                else if (frontObj && frontObj.CompareTag("Gate"))
                {
                    isMoving = false;
                    shouldMove = false;
                    endpoint = this.transform.position;
                    Refill(startPoint, endpoint);
                }
                else if (frontObj && frontObj.CompareTag("Block"))
                {
                    shouldMove = false;
                    isMoving = false;
                    Debug.Log("You lose from update checking block");
                }
            }
            Cube temp = RayCastFromTop();

            if (temp && temp.boxtype == Cube.colorType.unfilled)
            {
                temp.Highlight(highlightColor, cubecolor);

                if (startPoint == Vector3.zero)
                {
                    startPoint = temp.transform.position;
                }
            }

        }
        
    }
                
    public void Move(Vector3 dir)
    {
        Debug.Log("Move");
        direction = dir;
        GameObject rayObj = RayCastAround(dir);
        shouldMove = false;

        if (rayObj && rayObj.CompareTag("Cube"))
        {
            if (rayObj.GetComponent<Cube>().boxtype == Cube.colorType.unfilled)
            {
                shouldMove = true;
                isMoving = true;
                this.transform.position = rayObj.transform.position;
                if (startPoint == Vector3.zero)
                {
                    startPoint = rayObj.transform.position;
                }
            }
            else if (rayObj.GetComponent<Cube>().boxtype == Cube.colorType.filled)
            {
                this.transform.position = rayObj.transform.position;
                if (shouldMove == false)
                {
                    shouldMove = true;
                }
                
            }
            
            else if (rayObj.GetComponent<Cube>().boxtype == Cube.colorType.highlight)
            {
              //gameMode.Restart();
            }

        }
        else
        {
            if ( rayObj && rayObj.CompareTag("Wall"))
            {
                shouldMove = false;
                isMoving = false;
            }
            else if (rayObj && rayObj.CompareTag("Gate"))
            {
                shouldMove = false;
                isMoving = false;
            }
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

    private void filler(Vector3 start, Vector3 end)
    {
        float xDifferen = end.x - start.x;
        float zDifferent = end.z - start.z;


    }
    private void Refill(Vector3 start, Vector3 end)
    {
        if (start == Vector3.zero || end == Vector3.zero)
        {
            return;
        }

        if (start.x < end.x && start.z < end.z)
        {
            Debug.Log("Right");
            Vector3[] dir = new Vector3[] { Vector3.right, Vector3.forward };
            Raycast(dir);
        }
        else if (start.x > end.x && start.z < end.z)
        {
            Debug.Log("lef");
            Vector3[] dir = new Vector3[] { Vector3.left, Vector3.forward };
            Raycast(dir);
        }
        else if (start.x > end.x && start.z > end.z)
        {
            Debug.Log("upright");
            Vector3[] dir = new Vector3[] { Vector3.left, Vector3.back };
            Raycast(dir);
        }
        else if (start.x < end.x && start.z > end.z)
        {
            Debug.Log("uplef");
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
