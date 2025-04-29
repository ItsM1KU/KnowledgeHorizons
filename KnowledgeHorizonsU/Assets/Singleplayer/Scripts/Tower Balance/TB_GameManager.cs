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
    public float spawnerHeightAboveBlock = 8f;

    [Header("Camera Settings")]
    public float initialCameraSize = 7f;
    public float cameraYOffset = 3f;
    public float zoomOutFactor = 0.02f;
    public float maxZoomOut = 14f;
    [Range(0.1f, 2f)] public float cameraFollowDamping = 0.7f;

    private List<GameObject> placedBlocks = new List<GameObject>();
    private GameObject currentBlock;
    private GameObject swingPivot;
    private Transform cameraTarget;
    private int currentBlockIndex = 0;
    private float highestBlockY;
    private CinemachineFramingTransposer framingTransposer;

    void Start()
    {
        InitializeCameraSystem();
        CreateSpawnPivot();
        SpawnNewBlock();
    }

    void InitializeCameraSystem()
    {
        // Set up Cinemachine Brain
        if (!Camera.main.GetComponent<CinemachineBrain>())
            Camera.main.gameObject.AddComponent<CinemachineBrain>();

        // Create camera target
        cameraTarget = new GameObject("CameraTarget").transform;
        cameraTarget.position = new Vector3(0, 0, 0);

        // Configure virtual camera
        virtualCamera.transform.position = new Vector3(0, 0, -10);
        virtualCamera.m_Lens.OrthographicSize = initialCameraSize;
        virtualCamera.Follow = cameraTarget;

        // Configure framing transposer
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        framingTransposer.m_YDamping = cameraFollowDamping;
        framingTransposer.m_DeadZoneHeight = 0.4f;
    }

    void CreateSpawnPivot()
    {
        swingPivot = new GameObject("SwingPivot");
        swingPivot.transform.position = new Vector3(0, initialPivotHeight, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentBlock != null)
        {
            StartCoroutine(HandleBlockPlacement());
        }
    }

    IEnumerator HandleBlockPlacement()
    {
        PlaceBlock();
        yield return new WaitForSeconds(0.3f);
        UpdateCameraAndSpawner();
        SpawnNewBlock();
    }

    void PlaceBlock()
    {
        currentBlock.GetComponent<TB_MovingBlock>().PlaceBlock();
        placedBlocks.Add(currentBlock);

        // Update highest block position
        highestBlockY = Mathf.Max(highestBlockY, currentBlock.transform.position.y);
    }

    void UpdateCameraAndSpawner()
    {
        // Update camera position
        cameraTarget.position = new Vector3(0, highestBlockY + cameraYOffset, 0);

        // Update spawner position
        swingPivot.transform.position = new Vector3(
            0,
            highestBlockY + spawnerHeightAboveBlock,
            0
        );

        // Update zoom
        virtualCamera.m_Lens.OrthographicSize = Mathf.Min(
            initialCameraSize + (currentBlockIndex * zoomOutFactor),
            maxZoomOut
        );
    }

    void SpawnNewBlock()
    {
        Vector3 spawnPosition = swingPivot.transform.position - new Vector3(0, initialRopeLength, 0);
        currentBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);
        currentBlock.GetComponent<TB_MovingBlock>().Initialize(initialRopeLength);
        currentBlockIndex++;
    }
}