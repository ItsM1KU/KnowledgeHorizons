using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FC_AnimalPicker : MonoBehaviour
{
    public static FC_AnimalPicker instance;

    public List<int[]> firstAnimalSet = new List<int[]>
    {
        new int[] {0, 5, 9},
        new int[] {3, 4, 8},
        new int[] {6, 7, 14}
    };
    
    public List<int[]> secondAnimalSet = new List<int[]>
    {
        new int[] {1, 2, 8},
        new int[] {0, 11, 13},
        new int[] {2, 5, 10}
    };

    public List<int[]> thirdAnimalSet = new List<int[]>
    {
        new int[] {0, 2, 4, 8},
        new int[] {0, 5, 7, 9},
        new int[] {1, 2, 6, 14}
    };

    public List<int[]> fourthAnimalSet = new List<int[]>
    {
        new int[] {1, 2, 10, 14},
        new int[] {3, 4, 7, 8},
        new int[] {0, 11, 7, 10}
    };

    public List<int[]> fifthAnimalSet = new List<int[]>
    {
        new int[] {0, 2, 4, 7, 8},
        new int[] {1, 3, 4, 7, 14},
        new int[] {0, 5, 7, 9, 13},
        new int[] {1, 2, 4, 7, 10}
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

    public int[] PickSecondSet()
    {
        int randomIndex = Random.Range(0, secondAnimalSet.Count);
        int[] selectedSet = secondAnimalSet[randomIndex];
        return selectedSet;
    }

    public int[] PickThirdSet()
    {
        int randomIndex = Random.Range(0, thirdAnimalSet.Count);
        int[] selectedSet = thirdAnimalSet[randomIndex];
        return selectedSet;
    }

    public int[] PickFourthSet()
    {
        int randomIndex = Random.Range(0, fourthAnimalSet.Count);
        int[] selectedSet = fourthAnimalSet[randomIndex];
        return selectedSet;
    }

    public int[] PickFifthSet()
    {
        int randomIndex = Random.Range(0, fifthAnimalSet.Count);
        int[] selectedSet = fifthAnimalSet[randomIndex];
        return selectedSet;
    }
}
