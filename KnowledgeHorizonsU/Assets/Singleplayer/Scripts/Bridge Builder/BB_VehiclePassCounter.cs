using UnityEngine;
using TMPro;

public class BB_VehiclePassCounter : MonoBehaviour
{
    public static BB_VehiclePassCounter Instance;

    public TMP_Text passCountText;

    private int passCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
