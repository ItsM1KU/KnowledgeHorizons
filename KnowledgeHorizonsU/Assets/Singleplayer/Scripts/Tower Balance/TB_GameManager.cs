using UnityEngine;
using Cinemachine;
using System.Collections;

public class TB_GameManager : MonoBehaviour
{
    [Header("References")]
    public GameObject blockPrefab;
    public Transform basePlatform; // Should be at (0, -7.5, 0)
    public CinemachineVirtualCamera virtualCamera;
    public LineRenderer ropeRenderer;

    [Header("Settings")]
    public float blockHeight = 1f;
    public float swingRadius = 3f;
    public float cameraFollowSpeed = 2f;
    public float safeDistance = 2f;

    private GameObject currentBlock;
    private Transform spawnPivot;
    private Transform cameraTarget;
    private float currentTowerHeight;
    private bool canPlaceBlock = true;
    private CinemachineFramingTransposer transposer;
    private float targetCameraY;

    void Start()
    {
        InitializeCameraSystem();
        CreateSpawnPivot();
        SpawnNewBlock();
        targetCameraY = 0f; // Start camera at base view
    }

    void InitializeCameraSystem()
    {
        // Configure camera
        Camera.main.transform.position = new Vector3(0, 0, -10);
        virtualCamera.m_Lens.OrthographicSize = 7.5f; // Matches base position

        // Create camera target
        cameraTarget = new GameObject("CameraTarget").transform;
        cameraTarget.position = Vector3.zero;
        virtualCamera.Follow = cameraTarget;

        // Configure Cinemachine
        transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_YDamping = cameraFollowSpeed;
        transposer.m_DeadZoneHeight = 0.4f;
    }

    void CreateSpawnPivot()
    {
        spawnPivot = new GameObject("SpawnPivot").transform;
        UpdateSpawnPosition();
    }

    void UpdateSpawnPosition()
    {
        // Calculate safe spawn position
        float towerTop = basePlatform.position.y + currentTowerHeight;
        float minSpawnY = towerTop + safeDistance + blockHeight;
        float cameraTop = virtualCamera.State.FinalPosition.y + virtualCamera.m_Lens.OrthographicSize;

        spawnPivot.position = new Vector3(
            0,
            Mathf.Max(minSpawnY, cameraTop - (blockHeight / 2f)),
            0
        );
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canPlaceBlock)
        {
            StartCoroutine(BlockPlacementRoutine());
        }

        UpdateCameraPosition();
        UpdateRopeVisual();
    }

    IEnumerator BlockPlacementRoutine()
    {
        canPlaceBlock = false;

        currentBlock.GetComponent<TB_MovingBlock>().Place();
        yield return new WaitForSeconds(0.3f);

        // Update tower height
        currentTowerHeight += blockHeight;

        // Move camera target up if needed
        float visibleTop = virtualCamera.State.FinalPosition.y + virtualCamera.m_Lens.OrthographicSize;
        if (visibleTop < basePlatform.position.y + currentTowerHeight + 3f)
        {
            targetCameraY += blockHeight;
        }

        UpdateSpawnPosition();
        SpawnNewBlock();

        canPlaceBlock = true;
    }

    void UpdateCameraPosition()
    {
        cameraTarget.position = new Vector3(
            0,
            Mathf.Lerp(cameraTarget.position.y, targetCameraY, Time.deltaTime * cameraFollowSpeed),
            0
        );
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
        if (currentBlock != null)
        {
            ropeRenderer.positionCount = 2;
            ropeRenderer.SetPosition(0, spawnPivot.position);
            ropeRenderer.SetPosition(1, currentBlock.transform.position);
        }
    }

    void LateUpdate()
    {
        UpdateSpawnPosition();
    }
}