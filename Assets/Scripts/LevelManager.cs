using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance => instance;

    public System.Action LevelCompleted;

    [Space]
    [SerializeField]
    LevelInfoAsset levelInfoAsset;
    [SerializeField]
    StartCube startCube;

    [Space]
    [SerializeField]
    public Transform blockContainer;

    private static LevelManager instance;

    int currentLevelIndex = 0;

    [HideInInspector]
    public BlockSpawner blockSpawner = new BlockSpawner();

    List<FillAreaController> createdBlocks = new List<FillAreaController>();
    List<FillAreaController> collectedBlocks = new List<FillAreaController>();
    List<GameObject> blocksFromImage = new List<GameObject>();
    List<StartCube> startedCubes = new List<StartCube>();

    Collider startingCollider;

    public Transform startCubeTransform;

    public GameObject startCubes;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        startingCollider = startCubeTransform.GetComponentInChildren<Collider>();
        blockSpawner = GetComponent<BlockSpawner>();
    }

    private void Start()
    {
        Debug.Log(blockSpawner.createdCubes.Count);
    }

    public bool HandleCreateNextLevel()
    {
        if (createdBlocks.Count > 0)
        {
            for (int i = 0; i < createdBlocks.Count; i++)
            {
                Destroy(createdBlocks[i]);
            }
        }

        ++currentLevelIndex;

        if (levelInfoAsset.levelInfos.Count >= currentLevelIndex)
        {
            CreateNextLevel();
            return true;
        }

        return false;
    }

    void CreateNextLevel()
    {
        blocksFromImage = blockSpawner.CreateBlockFromImage(levelInfoAsset.levelInfos[currentLevelIndex - 1], blockContainer);
        CreateBlocks();
    }

    void CreateBlocks()
    {
        float xOffset = 1f;
        float yOffset = 0.15f;
        float zOffset = -4f;

        int modIndex = 0;
        int modCounter = 0;

        for (int i = 0; i < blocksFromImage.Count; i++)
        {
            if (i % 10 == 0)
                modCounter++;

            modIndex = i % 10;
            modCounter = modCounter % 10;

            StartCube tmpCube = Instantiate(startCube, startCubeTransform);
            tmpCube.transform.position = new Vector3(xOffset - modIndex * 0.3f, yOffset + modCounter * 0.3f, zOffset);
            startedCubes.Add(tmpCube);
        }
    }

    public void ActivateBlocks()
    {
        for (int i = 0; i < startedCubes.Count; i++)
        {
            startedCubes[i].rb.isKinematic = false;
            startedCubes[i].GetComponent<Collider>().enabled = true;
        }
    }

    public void OnBlockCreated(FillAreaController blockController)
    {
        createdBlocks.Add(blockController);
        Debug.Log("Collected Block Count " + collectedBlocks.Count);

        Debug.Log(blockSpawner.createdCubes.Capacity);

    }

    public void OnBlockCollected(FillAreaController blockController)
    {
        collectedBlocks.Add(blockController);
        Debug.Log($"{collectedBlocks.Count} / {createdBlocks.Count} <- Collected Block Count");

        if (collectedBlocks.Count == createdBlocks.Count)
        {
            LevelCompleted?.Invoke();
        }
    }
}
