using UnityEngine;
using System.Collections.Generic;

public class TB_GameManager : MonoBehaviour
{
    public GameObject blockPrefab;
    public Transform basePlatform;
    public float blockHeight = 0.5f;
    public float pivotHeight = 8f;

    private List<GameObject> placedBlocks = new List<GameObject>();
    private GameObject currentBlock;
    private int currentBlockIndex = 0;
    private GameObject swingPivot;

    void Start()
    {
        CreateSwingPivot();
        SpawnNewBlock();
    }

    void CreateSwingPivot()
    {
        swingPivot = new GameObject("SwingPivot");
        swingPivot.transform.position = new Vector3(0, pivotHeight, 0);

        var pivotSprite = swingPivot.AddComponent<SpriteRenderer>();
        // Uncomment if you have a sprite asset:
        // pivotSprite.sprite = Resources.Load<Sprite>("CircleSprite");
        pivotSprite.color = new Color(0, 0, 0, 0); // Fully transparent
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentBlock != null)
        {
            PlaceCurrentBlock();
        }
    }

    void SpawnNewBlock()
    {
        Vector3 spawnPosition;

        if (currentBlockIndex == 0)
        {
            spawnPosition = new Vector3(0, swingPivot.transform.position.y - 5f, 0);
        }
        else
        {
            float lastBlockHeight = placedBlocks[placedBlocks.Count - 1].transform.position.y;
            spawnPosition = new Vector3(0, lastBlockHeight + blockHeight, 0);
        }

        currentBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);
        currentBlockIndex++;
    }

    void PlaceCurrentBlock()
    {
        currentBlock.GetComponent<TB_MovingBlock>().PlaceBlock();
        placedBlocks.Add(currentBlock);
        SpawnNewBlock();
    }
}