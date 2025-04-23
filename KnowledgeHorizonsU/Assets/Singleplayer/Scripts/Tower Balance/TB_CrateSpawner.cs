using UnityEngine;
using System.Collections;

public class TB_CrateSpawner : MonoBehaviour
{
    public GameObject cratePrefab;
    private GameObject currentCrate;

    public float swingSpeed = 2f;
    public float swingRange = 3f;
    private float startX;

    public static bool cameraShouldFollow = false;
    public static int droppedCrateCount = 0;
    public float cameraRisePerDrop = 1f; // How much the camera rises per crate drop
    public float cameraMoveDuration = 0.5f; // Smooth move time

    void Start()
    {
        startX = transform.position.x;
        SpawnNewCrate();
    }

    void Update()
    {
        // Spawner follows camera AFTER the first crate drop
        if (cameraShouldFollow)
        {
            // Keep the spawner fixed above the camera by a fixed height
            float cameraY = Camera.main.transform.position.y;
            transform.position = new Vector3(startX, cameraY + 5f, 0); // 5 units above camera
        }

        if (currentCrate != null)
        {
            float swing = Mathf.Sin(Time.time * swingSpeed) * swingRange;
            currentCrate.transform.position = new Vector3(startX + swing, transform.position.y, 0);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                DropCrate();
            }
        }
    }

    void SpawnNewCrate()
    {
        currentCrate = Instantiate(cratePrefab, transform.position, Quaternion.identity);
        currentCrate.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    void DropCrate()
    {
        currentCrate.GetComponent<Rigidbody2D>().isKinematic = false;
        currentCrate = null;

        droppedCrateCount++;

        if (droppedCrateCount == 1)
        {
            cameraShouldFollow = true;
        }
        else if (cameraShouldFollow)
        {
            StartCoroutine(SmoothCameraAndSpawnerRise());
        }

        Invoke(nameof(SpawnNewCrate), 1f);
    }

    IEnumerator SmoothCameraAndSpawnerRise()
    {
        Vector3 startCamPos = Camera.main.transform.position;
        Vector3 endCamPos = startCamPos + new Vector3(0, cameraRisePerDrop, 0);

        float elapsed = 0f;

        while (elapsed < cameraMoveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / cameraMoveDuration;

            // Apply ease-in-out to camera movement
            float easedT = Mathf.SmoothStep(0f, 1f, t);

            Camera.main.transform.position = Vector3.Lerp(startCamPos, endCamPos, easedT);

            // Spawner stays 5 units above the camera
            transform.position = new Vector3(startX, Camera.main.transform.position.y + 5f, 0);

            yield return null;
        }

        // Final positions to correct floating point mismatch
        Camera.main.transform.position = endCamPos;
        transform.position = new Vector3(startX, endCamPos.y + 5f, 0);
    }
}
