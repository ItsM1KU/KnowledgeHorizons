using UnityEngine;
using TMPro;

public class BB_VehiclePassCounter : MonoBehaviour
{
    // Make this a singleton so it's easy to access.
    public static BB_VehiclePassCounter Instance;

    // Reference to the TextMeshPro UI element.
    public TMP_Text passCountText;

    // Counter for passed vehicles.
    private int passCount = 0;

    void Awake()
    {
        // Singleton pattern: if an instance exists, destroy this one.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Call this method whenever a vehicle successfully passes (i.e., leaves the screen).
    /// </summary>
    public void IncrementCount()
    {
        passCount++;
        if (passCountText != null)
        {
            passCountText.text = "Vehicles Passed: " + passCount.ToString();
        }
        Debug.Log("Vehicles passed: " + passCount);
    }
}
