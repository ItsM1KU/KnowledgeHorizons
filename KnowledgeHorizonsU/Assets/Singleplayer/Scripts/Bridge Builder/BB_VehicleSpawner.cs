using UnityEngine;
using System.Collections;

public class BB_VehicleSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    // Array of different vehicle prefabs; assign these in the Inspector.
    public GameObject[] vehiclePrefabs;

    // The spawn point where vehicles appear.
    public Transform spawnPoint;

    // Time delay before spawning the next vehicle after the current one is destroyed.
    public float spawnDelay = 2f;

    // Keep a reference to the current vehicle.
    private GameObject currentVehicle;

    // Flag to prevent multiple spawn coroutines.
    private bool waitingForSpawn = false;

    void Start()
    {
        // Spawn the first vehicle.
        SpawnVehicle();
    }

    // Spawns a vehicle at the spawn point.
    void SpawnVehicle()
    {
        if (vehiclePrefabs.Length > 0)
        {
            // Choose a random vehicle prefab from the array.
            int randomIndex = Random.Range(0, vehiclePrefabs.Length);
            currentVehicle = Instantiate(vehiclePrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
        }
    }

    void Update()
    {
        // Check if the current vehicle has been destroyed.
        if (currentVehicle == null && !waitingForSpawn)
        {
            waitingForSpawn = true;
            StartCoroutine(ResetSpawn());
        }
    }

    // Waits for a specified delay then spawns the next vehicle.
    IEnumerator ResetSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        waitingForSpawn = false;
        SpawnVehicle();
    }
}
