using UnityEngine;

public class BB_VehicleSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] vehiclePrefabs;
    public Transform spawnPoint;

    // Reference to the current vehicle.
    private GameObject currentVehicle;

    // NEW: Reference to the UI manager script
    public BB_VehicleWeightDisplay weightDisplay;

    void Start()
    {
        SpawnVehicle();
    }

    void Update()
    {
        // When currentVehicle is destroyed, it will evaluate to null.
        if (currentVehicle == null)
        {
            Debug.Log("No current vehicle found. Spawning a new vehicle.");
            SpawnVehicle();
        }
    }

    void SpawnVehicle()
    {
        if (vehiclePrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, vehiclePrefabs.Length);
            currentVehicle = Instantiate(vehiclePrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
            Debug.Log("Spawned new vehicle: " + currentVehicle.name);

            // Update the UI with the weight of the spawned vehicle.
            BB_VehicleController vc = currentVehicle.GetComponent<BB_VehicleController>();
            if (vc != null && weightDisplay != null)
            {
                weightDisplay.UpdateVehicleWeight(vc.weight);
                Debug.Log("Updated UI with vehicle weight: " + vc.weight);
            }
        }
    }
}
