using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    private List<Gate> gate;

    private PlayerCube player;
    private int currentStage = 1;
    private bool isSetted = false;
    private Vector3 nextposition;
    private List<List<Cube>> stagelist = new List<List<Cube>>();

    private GameObject[] cubeObj;

    private void Awake()
    {
        player = FindObjectOfType<PlayerCube>();
        
        for (int i = 1 ; i <= 4; i++)
        {
            List<Cube> stagei = new List<Cube>();
            stagelist.Add(stagei);
        }

        
    }
    void Start()
    {
        cubeObj = GameObject.FindGameObjectsWithTag("Cube");
        for (int i = 0; i < cubeObj.Length; i++)
        {
            stagelist[cubeObj[i].GetComponent<Cube>().stage].Add(cubeObj[i].GetComponent<Cube>());
        }
    }

    void Update()
    {

        if (isSetted)
        {
            player.GetComponent<PlayerCube>().enabled = false;
            player.GetComponent<Rigidbody>().isKinematic = true;
            player.GetComponent<PlayerCube>().enabled = false;

            player.transform.position = Vector3.Lerp(player.transform.position, nextposition, 4 * Time.deltaTime);
            player.GetComponent<PlayerCube>().direction = Vector3.zero;
            if (Vector3.Distance(player.transform.position, nextposition) <= .001f)
            {
                player.GetComponent<PlayerCube>().enabled = true;
                player.GetComponent<Rigidbody>().isKinematic = false;
                player.GetComponent<PlayerCube>().enabled = true;
                isSetted = false;
                if (gate.Count % 2 != 0)
                {
                    SetposZ();
                }
            }
        }
        Counter();
    }

    public void Counter()
    {
        if (currentStage > 3)
        {
            return;
        }
        bool isFilled = true;
        for (int i = 0; i < stagelist[currentStage].Count; i++)
        {
            if (stagelist[currentStage][i].boxtype != Cube.colorType.filled)
            {
                isFilled = false;
            }
        }
        if (isFilled)
        {
            currentStage += 1;
            if (currentStage > 3)
            {
                NextLevel();
            }
            SetPosX();
        }
    }

    private void SetPosX()
    {
        if (gate.Count <= 0)
        {
            return;
        }
        nextposition = new Vector3(gate[0].transform.position.x, player.transform.position.y, player.transform.position.z);
        gate[0].GetComponent<Gate>().Open();
        gate.Remove(gate[0]);
        isSetted = true;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void SetposZ()
    {
        nextposition = new Vector3(player.transform.position.x, player.transform.position.y, gate[0].transform.position.z + .01f);
        isSetted = true;
        gate[0].GetComponent<Gate>().Open();
        gate.Remove(gate[0]);

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
