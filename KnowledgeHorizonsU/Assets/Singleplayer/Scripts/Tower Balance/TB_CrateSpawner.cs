using UnityEngine;

public class TB_CrateSpawner : MonoBehaviour
{
    public GameObject cratePrefab;
    private GameObject currentCrate;
    public float swingSpeed = 2f;
    public float swingRange = 3f;
    private float startX;

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

    void SpawnNewCrate()
    {
        currentCrate = Instantiate(cratePrefab, transform.position, Quaternion.identity);
        currentCrate.GetComponent<Rigidbody2D>().isKinematic = true; // Don't fall until dropped
    }

    void DropCrate()
    {
        currentCrate.GetComponent<Rigidbody2D>().isKinematic = false;
        currentCrate = null;
        Invoke("SpawnNewCrate", 1f); // Delay before spawning next crate
    }
}
