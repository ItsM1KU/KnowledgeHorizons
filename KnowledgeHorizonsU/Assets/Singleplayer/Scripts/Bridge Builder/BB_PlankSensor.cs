using UnityEngine;

public class BB_PlankSensor : MonoBehaviour
{
    [Tooltip("Reference to the parent plank's controller.")]
    public BB_PlankController plankController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is a vehicle.
        if (collision.CompareTag("Vehicle"))
        {
            // Get the VehicleController component to read the vehicle's weight.
            BB_VehicleController vc = collision.GetComponent<BB_VehicleController>();
            if (vc != null)
            {
                // Notify the parent PlankController.
                plankController.CheckVehicleWeight(vc.weight);
            }
        }
    }
}
