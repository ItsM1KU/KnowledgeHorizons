using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TB_GameManager : MonoBehaviour
{
    [Header("Game References")]
    public GameObject blockPrefab;
    public Transform basePlatform;
    public Camera mainCamera;

    [Header("Block Settings")]
    public float blockHeight = 0.5f;
    public float initialPivotHeight = 8f;
    public float initialRopeLength = 5f;
    public float spawnerCameraOffset = 4f; // Distance between camera and spawner

    [Header("Camera Settings")]
    public float cameraFollowSpeed = 3f;
    public float cameraYOffset = 3f;
    public float cameraMoveUpAmount = 0.5f; // Fixed upward movement per block
    public float initialCameraSize = 6f;
    public float zoomOutFactor = 0.03f;
    public float maxZoomOut = 12f;

    private List<GameObject> placedBlocks = new List<GameObject>();
    private GameObject currentBlock;
    private GameObject swingPivot;
    private int currentBlockIndex = 0;
    private Vector3 cameraVelocity = Vector3.zero;
    private float currentPivotHeight;
    private float highestBlockY;
    private float currentCameraSize;
    private float targetCameraY;

    void Start()
    {
        currentCameraSize = initialCameraSize;
        mainCamera.orthographicSize = currentCameraSize;
        currentPivotHeight = initialPivotHeight;
        targetCameraY = basePlatform.position.y + cameraYOffset;

        // Create pivot point
        swingPivot = new GameObject("SwingPivot");
        UpdateSpawnerPosition();

        // Set initial camera position
        mainCamera.transform.position = new Vector3(
            0,
            targetCameraY,
            mainCamera.transform.position.z
        );

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
        // Position spawner above current camera view
        swingPivot.transform.position = new Vector3(
            0,
            targetCameraY + spawnerCameraOffset,
            0
        );
    }

    void SpawnNewBlock()
    {
        // Calculate spawn position below pivot point
        Vector3 spawnPosition = swingPivot.transform.position - new Vector3(0, initialRopeLength, 0);
        currentBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);

        TB_MovingBlock movingBlock = currentBlock.GetComponent<TB_MovingBlock>();
        if (movingBlock != null)
        {
            movingBlock.Initialize(initialRopeLength);
        }

        currentBlockIndex++;
    }

    void PlaceCurrentBlock()
    {
        if (currentBlock == null) return;

        currentBlock.GetComponent<TB_MovingBlock>()?.PlaceBlock();
        placedBlocks.Add(currentBlock);

        // Update highest block position
        highestBlockY = currentBlock.transform.position.y;

        // Move camera target up by fixed amount
        targetCameraY += cameraMoveUpAmount;

        // Update spawner position to stay at top
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
        if (mainCamera == null) return;

        // Calculate target zoom
        float targetZoom = Mathf.Min(
            initialCameraSize + (currentBlockIndex * zoomOutFactor),
            maxZoomOut
        );

        // Smooth camera movement to target Y
        Vector3 targetPosition = new Vector3(
            0,
            targetCameraY,
            mainCamera.transform.position.z
        );

        mainCamera.transform.position = Vector3.SmoothDamp(
            mainCamera.transform.position,
            targetPosition,
            ref cameraVelocity,
            cameraFollowSpeed * Time.deltaTime
        );

        // Smooth zoom
        mainCamera.orthographicSize = Mathf.Lerp(
            mainCamera.orthographicSize,
            targetZoom,
            cameraFollowSpeed * Time.deltaTime
        );
    }
}