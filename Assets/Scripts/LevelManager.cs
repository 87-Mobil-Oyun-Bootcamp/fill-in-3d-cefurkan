using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Animator anim;

    public static LevelManager Instance => instance;

    public Action LevelCompleted;

    [Space]
    [SerializeField]
    LevelInfoAsset levelInfoAsset;
    [SerializeField]
    StartCube startCube;

    [Space]
    [SerializeField]
    public Transform fillAreaContainer;

    private static LevelManager instance;

    public Material ab;

    int currentLevelIndex = 0;

    [HideInInspector]
    public FillAreaSpawner fillAreaSpawner = new FillAreaSpawner();

    public List<GameObject> blocksFromImage = new List<GameObject>();
    public List<StartCube> startedCubes = new List<StartCube>();
    public List<GameObject> endGameCubes = new List<GameObject>();

    public Transform startCubeTransform;
    public GameObject startCubes;

    public int startCubeShapes;



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
        fillAreaSpawner = GetComponent<FillAreaSpawner>();

      
    }

    private void Update()
    {
        for (int i = 0; i < startedCubes.Count; i++)
        {
            if (startedCubes[i].isDestroyed == true)
            {
                startedCubes.Remove(startedCubes[i]);             
            }
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            anim.SetBool("isFinished", true);

        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            EndGameAnim();
        }

       
    }
    void EndGameAnim()
    {
        for(int i = 0; i < endGameCubes.Count * .5; i++)
        {
            endGameCubes[i].GetComponent<Rigidbody>().isKinematic = false;
            endGameCubes[i].GetComponent<Rigidbody>().useGravity = true;
            endGameCubes[i].GetComponent<BoxCollider>().isTrigger = false;
            endGameCubes[i].GetComponent<BoxCollider>().enabled = true;
            endGameCubes[i].gameObject.transform.parent = null;
            endGameCubes.Remove(endGameCubes[i]);
        }
    }
    public bool HandleCreateNextLevel()
    {
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
        blocksFromImage = fillAreaSpawner.CreateBlockFromImage(levelInfoAsset.levelInfos[currentLevelIndex - 1], fillAreaContainer);
        CreateBlocks();
    }

    void CreateBlocks()
    {
        float xOffset = 1f;
        float yOffset = 0.15f;
        float zOffset = -4f;

        int modIndex = 0;
        int modCounter = 0;

        for (int i = 0; i < blocksFromImage.Count ; i++)
        {
            if (i % 10 == 0)
                modCounter++;

            modIndex = i % 10;
            modCounter = modCounter % 10;

            StartCube tmpCube = Instantiate(startCube, startCubeTransform);


            switch(startCubeShapes)
            {
                case 1:
                    tmpCube.transform.position = new Vector3(xOffset - modIndex * 0.35f, yOffset + modCounter * 0.5f, zOffset);
                    startedCubes.Add(tmpCube);

                    break;

                case 2:
                   
               
                    break;

            }

            
        }
    }
    public void ActivateBlocks()
    {
        for (int i = 0; i < startedCubes.Count; i++)
        {
            if (startedCubes[i].rb != null)
                startedCubes[i].rb.isKinematic = false;
            startedCubes[i].GetComponent<Collider>().enabled = true;
        }
    }
}
