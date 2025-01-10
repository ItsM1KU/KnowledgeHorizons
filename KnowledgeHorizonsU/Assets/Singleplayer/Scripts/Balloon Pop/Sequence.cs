using UnityEngine;

public class NumberSequenceManager : MonoBehaviour
{
    public int[] sequence = { 3, 6, 9, 12 }; // Example sequence
    public int currentStep = 3;

    public string GetNextNumber()
    {
        return sequence[currentStep].ToString();
    }
}
