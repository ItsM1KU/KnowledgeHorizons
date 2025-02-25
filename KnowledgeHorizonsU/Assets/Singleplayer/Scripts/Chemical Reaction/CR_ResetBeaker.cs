using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    public Beaker beaker; // Reference to Beaker script

    public void ResetBeaker()
    {
        if (beaker != null)
        {
            beaker.ClearReactants(); // Call a function to reset
        }
        else
        {
            Debug.LogError("Beaker reference not set!");
        }
    }
}
