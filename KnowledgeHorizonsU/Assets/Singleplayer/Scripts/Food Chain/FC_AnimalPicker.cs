using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FC_AnimalPicker : MonoBehaviour
{
    public static FC_AnimalPicker instance;

    public List<int[]> firstAnimalSet = new List<int[]>
    {
        new int[] {0, 1, 2},
        new int[] {1, 2, 3},
        new int[] {2, 3, 5}
    };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public int[] PickFirstSet()
    {
        int randomIndex = Random.Range(0, firstAnimalSet.Count);
        int[] selectedSet = firstAnimalSet[randomIndex];
        return selectedSet;
    }
}
