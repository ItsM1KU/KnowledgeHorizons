using UnityEngine;

public class TB_BlockSpawner : MonoBehaviour
{
    public GameObject cratePrefab;
    private GameObject currentCrate;
    public Transform cameraTransform;

    public float swingSpeed = 2f;
    public float swingRange = 3f;
    public float distanceAboveTopCrate = 6f;
    public float cameraOffsetBelowSpawner = 3f;

    private float startX;
    private bool firstCrateDropped = false;

    void Start()
    {
        startX = transform.position.x;
        SpawnNewCrate();
    }

    void Update()
    {
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

    void DropCrate()
    {
        if (currentCrate != null)
        {
            currentCrate.GetComponent<Rigidbody2D>().isKinematic = false;
            currentCrate = null;

            if (!firstCrateDropped)
            {
                firstCrateDropped = true;
            }
            else
            {
                // Only move the camera up slightly after each crate drop (after the first)
                Camera.main.GetComponent<TB_SmoothCameraFollow>().MoveCameraUp();
            }

            Invoke(nameof(SpawnNewCrate), 1f);
        }
    }

    void SpawnNewCrate()
    {
        float newY;

        if (!firstCrateDropped)
        {
            newY = transform.position.y;
        }
        else
        {
            // Find topmost crate
            GameObject[] crates = GameObject.FindGameObjectsWithTag("Crate");
            float topY = -Mathf.Infinity;

            foreach (GameObject crate in crates)
            {
                if (crate.transform.position.y > topY)
                    topY = crate.transform.position.y;
            }

            newY = topY + distanceAboveTopCrate;
        }

        // Update spawner position
        transform.position = new Vector3(startX, newY, 0);

        // Spawn crate
        currentCrate = Instantiate(cratePrefab, transform.position, Quaternion.identity);
        currentCrate.tag = "Crate";
        currentCrate.GetComponent<Rigidbody2D>().isKinematic = true;

        // Set current crate as camera target (but only if not first)
        if (firstCrateDropped)
        {
          
        }
    }
}
