using UnityEngine;

public class TB_BlockSpawner : MonoBehaviour
{
    public GameObject cratePrefab;  // Prefab for the crate
    private GameObject currentCrate;  // The current crate being spawned
    private float swingSpeed = 2f;  // Speed of the swinging motion
    private float swingRange = 3f;  // How far the crate swings left to right
    private float startX;  // Starting X position for the crate
    private static int droppedCrateCount = 0;  // Number of crates dropped

    public GameObject ground;  // Reference to the ground where crates will land
    public float cameraRisePerDrop = 1f;  // Amount the camera moves up after each drop

    void Start()
    {
        startX = transform.position.x;  // Set the start position of the spawner
        SpawnNewCrate();  // Spawn the first crate
    }

    void Update()
    {
        // Swing the current crate if it's active
        if (currentCrate != null)
        {
            // Swing the crate back and forth on the X-axis
            float swing = Mathf.Sin(Time.time * swingSpeed) * swingRange;
            currentCrate.transform.position = new Vector3(startX + swing, transform.position.y, 0);

            // Drop the crate when Spacebar is pressed
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DropCrate();
            }
        }
    }

    void SpawnNewCrate()
    {
        // Instantiate the crate and make it kinematic
        currentCrate = Instantiate(cratePrefab, new Vector3(startX, transform.position.y), Quaternion.identity);
        currentCrate.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    void DropCrate()
    {
        // Enable physics on the crate so it falls naturally
        Rigidbody2D rb = currentCrate.GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        rb.gravityScale = 1f; // Make sure gravity scale is reasonable

        currentCrate = null;
        droppedCrateCount++;

        if (droppedCrateCount > 1)
        {
            MoveCameraAndSpawnerUp();
        }

        // Spawn the next crate after a short delay
        Invoke(nameof(SpawnNewCrate), 1f);
    }


    void MoveCameraAndSpawnerUp()
    {
        // Move the camera up gradually after every crate drop
        Vector3 camPos = Camera.main.transform.position;
        camPos.y += cameraRisePerDrop;  // Move up by the specified amount
        Camera.main.transform.position = camPos;

        // Move the spawner up to maintain consistent distance
        Vector3 spawnerPos = transform.position;
        spawnerPos.y += cameraRisePerDrop;  // Same as the camera
        transform.position = spawnerPos;
    }
}
