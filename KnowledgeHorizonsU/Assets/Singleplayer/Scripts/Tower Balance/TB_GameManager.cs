using UnityEngine;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class TB_GameManager : MonoBehaviour
{
    [Header("Game References")]
    public GameObject blockPrefab;
    public Transform basePlatform;
    public CinemachineVirtualCamera virtualCamera; // Changed from Camera

    [Header("Block Settings")]
    public float blockHeight = 0.5f;
    public float initialPivotHeight = 15f;
    public float initialRopeLength = 6f;

    [Header("Camera Settings")]
    public float cameraFollowSpeed = 3f;
    public float cameraYOffset = 3f;
    public float initialCameraSize = 8f;
    public float zoomOutFactor = 0.02f;
    public float maxZoomOut = 14f;
    public float spawnerHeightAboveCamera = 8f;

    private List<GameObject> placedBlocks = new List<GameObject>();
    private GameObject currentBlock;
    private GameObject swingPivot;
    private int currentBlockIndex = 0;
    private float currentCameraSize;
    private float targetCameraY;
    private float highestBlockY;
    private bool firstBlockPlaced = false;
    private CinemachineFramingTransposer framingTransposer;

    void Start()
    {
        currentCameraSize = initialCameraSize;
        virtualCamera.m_Lens.OrthographicSize = currentCameraSize;

        // Get the framing transposer component
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        // Set initial camera position
        targetCameraY = (initialPivotHeight + basePlatform.position.y) / 2f;
        virtualCamera.transform.position = new Vector3(0, targetCameraY, -10);

        // Create pivot point at top
        swingPivot = new GameObject("SwingPivot");
        UpdateSpawnerPosition();

        SpawnNewBlock();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentBlock != null)
        {
            PlaceCurrentBlock();
        }

        UpdateCameraPosition();
    }

    void UpdateSpawnerPosition()
    {
        swingPivot.transform.position = new Vector3(
            0,
            virtualCamera.transform.position.y + spawnerHeightAboveCamera,
            0
        );
    }

    void SpawnNewBlock()
    {
        Vector3 spawnPosition = swingPivot.transform.position - new Vector3(0, initialRopeLength, 0);
        currentBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);

        TB_MovingBlock movingBlock = currentBlock.GetComponent<TB_MovingBlock>();
        if (movingBlock != null)
        {
            movingBlock.Initialize(initialRopeLength);
        }

        // Set the virtual camera to follow the current block
        virtualCamera.Follow = currentBlock.transform;
        virtualCamera.LookAt = currentBlock.transform;

        currentBlockIndex++;
    }

    void PlaceCurrentBlock()
    {
        if (currentBlock == null) return;

        currentBlock.GetComponent<TB_MovingBlock>()?.PlaceBlock();
        placedBlocks.Add(currentBlock);

        highestBlockY = Mathf.Max(highestBlockY, currentBlock.transform.position.y);

        if (!firstBlockPlaced)
        {
            firstBlockPlaced = true;
            targetCameraY = basePlatform.position.y + cameraYOffset;

            // Switch to follow the tower instead of individual blocks
            var emptyGO = new GameObject("CameraTarget");
            emptyGO.transform.position = new Vector3(0, targetCameraY, 0);
            virtualCamera.Follow = emptyGO.transform;
            virtualCamera.LookAt = null;
        }
        else
        {
            targetCameraY = highestBlockY + cameraYOffset;
            virtualCamera.Follow.position = new Vector3(0, targetCameraY, 0);
        }

        UpdateSpawnerPosition();
        StartCoroutine(WaitAndSpawn());
    }

    IEnumerator WaitAndSpawn()
    {
        yield return new WaitForSeconds(0.3f);
        SpawnNewBlock();
    }

    void UpdateCameraPosition()
    {
        // Handle zooming
        float targetZoom = Mathf.Min(
            initialCameraSize + (currentBlockIndex * zoomOutFactor),
            maxZoomOut
        );
        currentCameraSize = Mathf.Lerp(
            currentCameraSize,
            targetZoom,
            cameraFollowSpeed * Time.deltaTime
        );
        virtualCamera.m_Lens.OrthographicSize = currentCameraSize;
    }
}