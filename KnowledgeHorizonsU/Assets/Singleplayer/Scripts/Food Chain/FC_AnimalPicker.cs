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
    
    public List<int[]> secondAnimalSet = new List<int[]>
    {
        new int[] {0, 1, 2},
        new int[] {1, 2, 3},
        new int[] {2, 3, 4}
    };

    public List<int[]> thirdAnimalSet = new List<int[]>
    {
        new int[] {0, 1, 2, 3},
        new int[] {1, 2, 3, 4},
        new int[] {2, 3, 4, 5}
    };

    public List<int[]> fourthAnimalSet = new List<int[]>
    {
        new int[] {0, 1, 2, 4},
        new int[] {1, 2, 3, 2},
        new int[] {2, 3, 4, 5}
    };

    public List<int[]> fifthAnimalSet = new List<int[]>
    {
        new int[] {0, 1, 2, 3, 4},
        new int[] {1, 2, 3, 4, 1},
        new int[] {2, 3, 4, 5, 1}
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
