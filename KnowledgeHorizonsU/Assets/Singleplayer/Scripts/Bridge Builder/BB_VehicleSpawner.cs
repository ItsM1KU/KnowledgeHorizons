using UnityEngine;
using UnityEngine.UI;  // For UI feedback if needed

public class BB_VehicleSpawner : MonoBehaviour
{
    public GameObject[] vehiclePrefabs;  // Assign your different vehicle prefabs here.
    public Transform spawnPoint;         // Set this at the left side of your bridge.
    public float spawnDelay = 1f;        // Delay before spawning the next vehicle.

    // Optional: UI Text element to display an educational tip.
    public Text tipText;

    private bool isSpawning = false;

    void Start()
    {
        SpawnVehicle();
    }

    public void SpawnVehicle()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            // Choose a random vehicle from the array.
            int index = Random.Range(0, vehiclePrefabs.Length);
            GameObject vehicle = Instantiate(vehiclePrefabs[index], spawnPoint.position, Quaternion.identity);

            // Optionally display a tip based on vehicle mass.
            Rigidbody2D rb = vehicle.GetComponent<Rigidbody2D>();
            if (tipText != null)
            {
                tipText.text = "Incoming Vehicle Mass: " + rb.mass.ToString() + ". Place supports accordingly!";
            }
        }
    }

    // This method should be called when the current vehicle has finished its run.
    public void OnVehicleFinished()
    {
        Invoke(nameof(SpawnVehicle), spawnDelay);
        isSpawning = false;
    }
}
