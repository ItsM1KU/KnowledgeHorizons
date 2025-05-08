using UnityEngine;
using Cinemachine;
using System.Collections;

public class TB_GameManager : MonoBehaviour
{
    [Header("References")]
    public GameObject blockPrefab;
    public Transform basePlatform;
    public CinemachineVirtualCamera virtualCamera;
    public LineRenderer ropeRenderer;

    [Header("Settings")]
    public float blockHeight = 1f;
    public float swingRadius = 3f;
    public float cameraMoveSpeed = 3f;
    public float safeDistance = 4f; // Increased from 4 to match camera movement
    public float spawnDelay = 0.5f;
    public int blocksBeforeMovement = 3;
    public float cameraMoveDuration = 0.5f; // Added for smooth movement

    private GameObject currentBlock;
    private Transform spawnPivot;
    private Transform cameraTarget;
    private float currentTowerHeight;
    private bool canPlaceBlock = true;
    private int blocksPlaced = 0;
    private float baseY;
    private CinemachineFramingTransposer transposer;

    void Start()
    {
        baseY = basePlatform.position.y;
        InitializeCameraSystem();
        CreateSpawnPivot();
        SpawnNewBlock();
    }
    void InitializeCameraSystem()
    {
        Camera.main.transform.position = new Vector3(0, baseY + 7.5f, -10);
        virtualCamera.m_Lens.OrthographicSize = 7.5f;

        cameraTarget = new GameObject("CameraTarget").transform;
        cameraTarget.position = new Vector3(0, baseY + 7.5f, 0);
        virtualCamera.Follow = cameraTarget;

        // Remove damping for direct control
        transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_YDamping = 0;
    }

    void CreateSpawnPivot()
    {
        spawnPivot = new GameObject("SpawnPivot").transform;
        UpdateSpawnPosition();
    }

    void UpdateSpawnPosition()
    {
        // Always keep spawner at top of camera view with safe distance
        float cameraTop = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0)).y;
        spawnPivot.position = new Vector3(0, cameraTop - (blockHeight / 2f), 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canPlaceBlock)
        {
            StartCoroutine(BlockPlacementRoutine());
        }
        UpdateRopeVisual();
    }

    IEnumerator BlockPlacementRoutine()
    {
        canPlaceBlock = false;

        Vector3 placedPosition = currentBlock.transform.position;
        currentBlock.GetComponent<TB_MovingBlock>().Place();
        blocksPlaced++;

        yield return new WaitForSeconds(0.3f);

        currentTowerHeight = Mathf.Max(
            currentTowerHeight,
            placedPosition.y - baseY
        );

        if (blocksPlaced >= blocksBeforeMovement)
        {
            // Calculate target position based on safe distance
            float targetY = cameraTarget.position.y + safeDistance;

            // Smooth camera movement
            yield return StartCoroutine(SmoothCameraMove(cameraTarget.position.y, targetY));
        }

        UpdateSpawnPosition();
        yield return new WaitForSeconds(spawnDelay);
        SpawnNewBlock();

        canPlaceBlock = true;
    }

    IEnumerator SmoothCameraMove(float startY, float targetY)
    {
        float elapsed = 0f;

        while (elapsed < cameraMoveDuration)
        {
            float newY = Mathf.Lerp(startY, targetY, elapsed / cameraMoveDuration);
            cameraTarget.position = new Vector3(0, newY, 0);
            elapsed += Time.deltaTime;

            // Update spawn position during movement
            UpdateSpawnPosition();
            yield return null;
        }

        cameraTarget.position = new Vector3(0, targetY, 0);
    }


    void SpawnNewBlock()
    {
        Vector3 spawnPos = new Vector3(
            0,
            spawnPivot.position.y - swingRadius,
            0
        );

        currentBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity);
        currentBlock.GetComponent<TB_MovingBlock>().Initialize(spawnPivot, swingRadius);
    }

    void UpdateRopeVisual()
    {
        if (currentBlock != null && !currentBlock.GetComponent<TB_MovingBlock>().IsPlaced)
        {
            ropeRenderer.positionCount = 2;
            ropeRenderer.SetPosition(0, spawnPivot.position);
            ropeRenderer.SetPosition(1, currentBlock.transform.position);
        }
        else
        {
            ropeRenderer.positionCount = 0;
        }
    }

    void LateUpdate()
    {
        UpdateSpawnPosition();
    }
}