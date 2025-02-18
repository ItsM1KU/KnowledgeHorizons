using UnityEngine;
using TMPro;

public class BB_VehicleWeightDisplay : MonoBehaviour
{
    public TMP_Text weightText; // Assign your TextMeshPro UI Text in the Inspector

    public void UpdateVehicleWeight(float weight)
    {
        if (weightText != null)
        {
            weightText.text = "Vehicle Weight: " + weight.ToString();
            Debug.Log("Updated Vehicle Weight UI: " + weight);
        }
        else
        {
            Debug.LogError("weightText reference is missing in BB_VehicleWeightDisplay!");
        }
    }
}
