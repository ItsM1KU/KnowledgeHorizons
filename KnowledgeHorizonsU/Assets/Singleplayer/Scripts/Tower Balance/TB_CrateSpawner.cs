using UnityEngine;

public class TB_CrateSpawner : MonoBehaviour
{
    public GameObject cratePrefab;
    private GameObject currentCrate;
    public float swingSpeed = 2f;
    public float swingRange = 3f;
    private float startX;
    private TB_CameraFollow cameraFollow;

    void Start()
    {
        startX = transform.position.x;
        cameraFollow = Camera.main.GetComponent<TB_CameraFollow>(); // Get reference
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

    void SpawnNewCrate()
    {
        currentCrate = Instantiate(cratePrefab, transform.position, Quaternion.identity);
        currentCrate.GetComponent<Rigidbody2D>().isKinematic = true;

        // Set camera to follow this crate
        if (cameraFollow != null)
        {
            cameraFollow.target = currentCrate.transform;
        }
    }



    void DropCrate()
    {
        currentCrate.GetComponent<Rigidbody2D>().isKinematic = false;

        // Set the camera to follow the falling crate
        if (cameraFollow != null)
        {
            cameraFollow.target = currentCrate.transform;
        }

        currentCrate = null;
        Invoke("SpawnNewCrate", 1f);
    }

}
