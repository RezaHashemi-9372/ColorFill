using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhoneController : MonoBehaviour
{
    private Vector2 startPoint = new Vector3();
    private Vector2 endPoint = new Vector3();

    private PlayerCube player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerCube>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            TouchProcess(Input.GetTouch(0));
        }

#if UNITY_EDITOR
        //This line of code just for testing purposes.
        if (Input.GetMouseButtonDown(0))
        {
            TouchProcess(new Touch() { position = Input.mousePosition, phase = TouchPhase.Began });
        }
        else if (Input.GetMouseButtonUp(0))
        {
            TouchProcess(new Touch() { position = Input.mousePosition, phase = TouchPhase.Ended });
        }
#endif
    }

    public void TouchProcess(Touch touch)
    {
        Debug.Log("Touch process");
        switch (touch.phase)
        {
            case TouchPhase.Began:
                startPoint = touch.position;
                break;
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
            case TouchPhase.Ended:
                endPoint = touch.position;
                SetDirection(startPoint, endPoint);
                break;
        }
    }

    public void SetDirection(Vector3 start, Vector3 end)
    {
        float xDifferent;
        float yDifferent;

        xDifferent = end.x - start.x;
        yDifferent = end.y - start.y;

        if (Mathf.Abs(xDifferent) > Mathf.Abs(yDifferent))
        {
            if (xDifferent > 0 )
            {
                Debug.Log("Right");
                player.Move(Vector3.right);
            }
            else
            {
                Debug.Log("Left");
                player.Move(Vector3.left);
            }
        }
        else
        {
            if (yDifferent > 0)
            {
                Debug.Log("UP");
                player.Move(Vector3.forward);
            }
            else
            {
                Debug.Log("Down");
                player.Move(Vector3.back);
            }
        }
    }
}
