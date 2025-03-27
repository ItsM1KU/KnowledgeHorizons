using UnityEngine;

public class BB_PlankSensor : MonoBehaviour
{
    [Tooltip("Reference to the parent plank's controller.")]
    public BB_PlankController plankController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vehicle"))
        {
            BB_VehicleController vc = collision.GetComponent<BB_VehicleController>();
            if (vc != null)
            {
                plankController.CheckVehicleWeight(vc.weight);
            }
        }
    }
}