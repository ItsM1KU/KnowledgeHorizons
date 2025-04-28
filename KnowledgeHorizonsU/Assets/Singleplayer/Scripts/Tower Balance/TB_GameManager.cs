using UnityEngine;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class TB_GameManager : MonoBehaviour
{
    [Header("Game References")]
    public GameObject blockPrefab;
    public Transform basePlatform;
    public CinemachineVirtualCamera virtualCamera;

    [Header("Block Settings")]
    public float blockHeight = 0.5f;
    public float initialPivotHeight = 12f;
    public float initialRopeLength = 5f;
    public float spawnerHeightAboveHighestBlock = 8f;

    [Header("Camera Settings")]
    public float initialCameraSize = 7f;
    public float zoomOutFactor = 0.02f;
    public float maxZoomOut = 14f;
    [Range(0.1f, 2f)] public float cameraFollowDamping = 0.7f;
    public float cameraYOffset = 3f;

    private List<GameObject> placedBlocks = new List<GameObject>();
    private GameObject currentBlock;
    private GameObject swingPivot;
    private Transform cameraTarget;
    private int currentBlockIndex = 0;
    private float highestBlockY;
    private CinemachineFramingTransposer framingTransposer;
    private bool firstBlockPlaced = false;

    void Start()
    {
        // Force main camera to start at Y=0
        Camera.main.transform.position = new Vector3(0, 0, -10);

        // Initialize virtual camera at Y=0
        virtualCamera.transform.position = new Vector3(0, 0, -10);
        virtualCamera.m_Lens.OrthographicSize = initialCameraSize;

        // Configure framing transposer
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        framingTransposer.m_XDamping = cameraFollowDamping;
        framingTransposer.m_YDamping = cameraFollowDamping;
        framingTransposer.m_ZDamping = cameraFollowDamping;
        framingTransposer.m_DeadZoneHeight = 0.4f;

        // Create camera target at Y=0 initially
        cameraTarget = new GameObject("CameraTarget").transform;
        cameraTarget.position = new Vector3(0, 0, 0);
        virtualCamera.Follow = cameraTarget;

        // Create initial pivot point
        swingPivot = new GameObject("SwingPivot");
        swingPivot.transform.position = new Vector3(0, initialPivotHeight, 0);

        SpawnNewBlock();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentBlock != null)
        {
            PlaceCurrentBlock();
        }
    }

    void UpdateSpawnerPosition()
    {
        // Position spawner above highest block (or at initial height if no blocks placed)
        float spawnerY = firstBlockPlaced ?
            highestBlockY + spawnerHeightAboveHighestBlock :
            initialPivotHeight;

        swingPivot.transform.position = new Vector3(0, spawnerY, 0);
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

        currentBlockIndex++;
    }

    void PlaceCurrentBlock()
    {
        if (currentBlock == null) return;

        currentBlock.GetComponent<TB_MovingBlock>()?.PlaceBlock();
        placedBlocks.Add(currentBlock);

        // Update highest block position
        highestBlockY = Mathf.Max(highestBlockY, currentBlock.transform.position.y);

        if (!firstBlockPlaced)
        {
            firstBlockPlaced = true;
            // After first block, start camera follow at ground level
            cameraTarget.position = new Vector3(0, basePlatform.position.y + cameraYOffset, 0);
        }
        else
        {
            // Move camera target upward by fixed amount per block
            cameraTarget.position += new Vector3(0, blockHeight, 0);
        }

        UpdateCameraZoom();
        UpdateSpawnerPosition();
        StartCoroutine(WaitAndSpawn());
    }

    void UpdateCameraZoom()
    {
        float targetZoom = Mathf.Min(
            initialCameraSize + (currentBlockIndex * zoomOutFactor),
            maxZoomOut
        );

        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
            virtualCamera.m_Lens.OrthographicSize,
            targetZoom,
            Time.deltaTime * 2f
        );
    }

    IEnumerator WaitAndSpawn()
    {
        yield return new WaitForSeconds(0.3f);
        SpawnNewBlock();
    }
}