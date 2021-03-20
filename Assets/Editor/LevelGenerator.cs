using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelGenerator : EditorWindow
{
    private GameObject cubePrefab;
    private PlayerCube playerCube;
    private Cube gridPrefab;
    private MoveAbleBlock moveAblePref;
    private GameObject constBlock;
    private GameObject levels;
    private float scalex;
    private Color playerColor, blockColor, highlightColor;
    private int row, column, stageNum, moveAmount;

    private enum Direction
    {
        up , 
        down,
    }

    [MenuItem("Level Creator/ Generate Level")]
    public static void OpenWindow()
    {
        GetWindow<LevelGenerator>();
    }
    private void OnGUI()
    {
        gridPrefab = EditorGUILayout.ObjectField("Grid Prefab:", gridPrefab, typeof(Cube), true) as Cube;
        cubePrefab = EditorGUILayout.ObjectField("Cube Prefab:", cubePrefab, typeof(GameObject), true) as GameObject;
        moveAblePref = EditorGUILayout.ObjectField("Mova Able Prefab", moveAblePref, typeof(MoveAbleBlock), true) as MoveAbleBlock;
        constBlock = EditorGUILayout.ObjectField("Const Block", constBlock, typeof(GameObject), true) as GameObject;
        playerCube = EditorGUILayout.ObjectField("Player Prefab", playerCube, typeof(PlayerCube), true) as PlayerCube;

        playerColor = EditorGUILayout.ColorField("Player Color", playerColor);
        blockColor = EditorGUILayout.ColorField("Block Color", blockColor);
        highlightColor = EditorGUILayout.ColorField("Highlight color", highlightColor);
        moveAmount = EditorGUILayout.IntField("Move amount: ", moveAmount);
        row = EditorGUILayout.IntField("Row: ", row);
        column = EditorGUILayout.IntField("Column: ", column);
        stageNum = EditorGUILayout.IntField("Stage Number: ", stageNum);
        if (GUILayout.Button("Generate"))
        {
            GenerateLevel();
        }
        if (GUILayout.Button("Change To grid"))
        {
            ChangetoGrid();
        }
        if (GUILayout.Button("Change To Wall"))
        {
            ChangeToWall();
        }
        if (GUILayout.Button("ADD Block"))
        {
            ChangeToBlock();
        }
        if (GUILayout.Button("Const Block"))
        {
            CreateConstBlock();
        }
        if (GUILayout.Button("Create Player"))
        {
            Createplayer();
        }
    }

    private void GenerateLevel()
    {
        Vector3 pos = new Vector3();
        scalex = cubePrefab.transform.localScale.x * cubePrefab.GetComponent<BoxCollider>().size.z + .015f;
        levels = new GameObject();
        levels.transform.name = "Level";
        levels.tag = "Levels";
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Instantiate(cubePrefab, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity, levels.transform);
                pos.x += scalex;
            }
            pos.x = 0;
            pos.z += scalex;
        }
    }

    private void CreateConstBlock()
    {
        levels = GameObject.FindGameObjectWithTag("Levels");
        while (Selection.gameObjects.Length > 0)
        {
            Vector3 pos = Selection.gameObjects[0].transform.position;
            DestroyImmediate(Selection.gameObjects[0].gameObject);
            Instantiate(constBlock, pos, Quaternion.identity, levels.transform);
        }
    }


    private void ChangetoGrid()
    {
        levels = GameObject.FindGameObjectWithTag("Levels");
        while (Selection.gameObjects.Length > 0)
        {
            Vector3 pos = Selection.gameObjects[0].transform.position;
            pos.y = 0;
            DestroyImmediate(Selection.gameObjects[0].gameObject);
            Cube cube= Instantiate(gridPrefab, pos, Quaternion.identity, levels.transform);
            cube.stage = stageNum;
            cube.fillColor = playerColor;
        }
    }

    private void ChangeToBlock()
    {
        levels = GameObject.FindGameObjectWithTag("Levels");
        Vector3 pos = Selection.activeGameObject.transform.position;
        pos.y += 0.048f;
        MoveAbleBlock blo = Instantiate(moveAblePref, pos, Quaternion.identity, levels.transform);
        blo.Setup(moveAmount * scalex, blockColor);
    }

    private void ChangeToWall()
    {
        levels = GameObject.FindGameObjectWithTag("Levels");

        while (Selection.gameObjects.Length > 0)
        {
            Vector3 pos = Selection.gameObjects[0].transform.position;
            pos.y = pos.y + scalex / 2;
            DestroyImmediate(Selection.gameObjects[0].gameObject);
            Instantiate(cubePrefab, pos, Quaternion.identity, levels.transform);
        }
    }

    private void Createplayer()
    {

        Vector3 pos = Selection.activeGameObject.transform.position;
        pos.y += scalex;
        PlayerCube cube = Instantiate(playerCube, pos, Quaternion.identity);
        cube.highlightColor = highlightColor;
        cube.cubecolor = playerColor;
    }
}

fileFormatVersion: 2
guid: b6e4aacc94f692b4d82eb973c1cbc0f8
MonoImporter:
  externalObjects: {}
  serializedVersion: 2
  defaultReferences: []
  executionOrder: 0
  icon: {instanceID: 0}
  userData: 
  assetBundleName: 
  assetBundleVariant: 
