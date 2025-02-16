using UnityEngine;
using System.Collections;

public class BB_VehicleSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    // Array of different vehicle prefabs; assign these in the Inspector.
    public GameObject[] vehiclePrefabs;

    // The spawn point where vehicles appear.
    public Transform spawnPoint;

    // Time delay between vehicle spawns
    public float spawnDelay = 2f;

    // Control to ensure one vehicle spawns at a time.
    private bool canSpawn = true;

    void Start()
    {
        // Optionally, spawn the first vehicle immediately.
        SpawnVehicle();
    }

    public void SpawnVehicle()
    {
        if (canSpawn && vehiclePrefabs.Length > 0)
        {
            // Choose a random vehicle prefab from the array
            int randomIndex = Random.Range(0, vehiclePrefabs.Length);
            Instantiate(vehiclePrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
            canSpawn = false;
            // Use a coroutine to wait for the delay, then enable spawning
            StartCoroutine(ResetSpawn());
        }
    }

    // Coroutine to enable spawning after a delay
    IEnumerator ResetSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        canSpawn = true;
        // Optionally, spawn the next vehicle automatically:
        SpawnVehicle();
    }
}
