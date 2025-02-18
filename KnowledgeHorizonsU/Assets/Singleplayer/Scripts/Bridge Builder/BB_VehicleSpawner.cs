using UnityEngine;
using System.Collections;

public class BB_VehicleSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] vehiclePrefabs;
    public Transform spawnPoint;
    public float spawnDelay = 2f;

    // Keep a reference to the current vehicle.
    private GameObject currentVehicle;

    // Flag to prevent multiple spawn coroutines.
    private bool waitingForSpawn = false;

    // NEW: Reference to the UI manager script
    public BB_VehicleWeightDisplay weightDisplay;

    void Start()
    {
        SpawnVehicle();
    }

    void SpawnVehicle()
    {
        if (vehiclePrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, vehiclePrefabs.Length);
            currentVehicle = Instantiate(vehiclePrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

            // Update the UI with the weight of the spawned vehicle.
            BB_VehicleController vc = currentVehicle.GetComponent<BB_VehicleController>();
            if (vc != null && weightDisplay != null)
            {
                weightDisplay.UpdateVehicleWeight(vc.weight);
            }
        }
    }

    void Update()
    {
        if (currentVehicle == null && !waitingForSpawn)
        {
            waitingForSpawn = true;
            StartCoroutine(ResetSpawn());
        }
    }

    IEnumerator ResetSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        waitingForSpawn = false;
        SpawnVehicle();
    }
}
