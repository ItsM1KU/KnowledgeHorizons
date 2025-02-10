using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FC_GameManager : MonoBehaviour
{
    public static FC_GameManager instance;

    [SerializeField] GameObject[] slotsA;
    [SerializeField] GameObject[] slotsB;
    [SerializeField] GameObject[] slotsC;
    [SerializeField] GameObject[] slotsD;
    [SerializeField] GameObject[] slotsE;

    [SerializeField] GameObject[] animalPrefabs;

    [SerializeField]
    Transform[] _spawnPoints;

    [SerializeField] Canvas Canvas;

    private int[] AnswerSet;

    int currentPlace;
    //private bool canSubmit = false;

    [SerializeField] public List<GameObject[]> slotsPerRound = new List<GameObject[]>();


    private int currentRound = 0;

    
    private List<GameObject> spawnedAnimals = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        currentRound = 0;

        slotsPerRound.Add(slotsA);
        slotsPerRound.Add(slotsB);
        slotsPerRound.Add(slotsC);
        slotsPerRound.Add(slotsD);
        slotsPerRound.Add(slotsE);
    }

    private void Start()
    {
        SpawnFirstSet();
        //ActivateSlots();
    }

    private void PickRound()
    {
        switch (currentRound)
        {
            case 0:
                SpawnFirstSet();
                break;
            case 1:
                StartCoroutine(SecondRound());
                break;
            case 2:
                StartCoroutine(ThirdRound());
                break;
            case 3:
                StartCoroutine(FourthRound());
                break;
            case 4:
                StartCoroutine(FifthRound());
                break;
            default:
                Debug.Log("Game Over");
                break;
        }
    }
    private void SpawnFirstSet()
    {
        ActivateSlots(slotsA);
        DeactivateSlots(slotsB);
        DeactivateSlots(slotsC);
        DeactivateSlots(slotsD);
        DeactivateSlots(slotsE);


        AnswerSet = FC_AnimalPicker.instance.PickFirstSet();
        int[] selectedset = AnswerSet;

        List<int> spawnIndices = new List<int> { 0, 1, 2 };
        shuffleList(spawnIndices);
        for (int i = 0; i < selectedset.Length; i++)
        {
            int animalIndex = selectedset[i];
            if (animalIndex >= 0 && animalIndex < animalPrefabs.Length)
            {
                GameObject go = Instantiate(animalPrefabs[animalIndex], _spawnPoints[spawnIndices[i]].position, Quaternion.identity);
                go.gameObject.transform.SetParent(Canvas.transform);
                spawnedAnimals.Add(go);
            }
        }
    }

    private void SpawnSecondSet()
    {
        DeactivateSlots(slotsA);
        ActivateSlots(slotsB);
        DeactivateSlots(slotsC);
        DeactivateSlots(slotsD);
        DeactivateSlots(slotsE);

        DestroyAnimals(spawnedAnimals);

        AnswerSet = FC_AnimalPicker.instance.PickSecondSet();
        int[] selectedset = AnswerSet;

        List<int> spawnIndices = new List<int> { 0, 1, 2};
        shuffleList(spawnIndices);
        for (int i = 0; i < selectedset.Length; i++)
        {
            int animalIndex = selectedset[i];
            if (animalIndex >= 0 && animalIndex < animalPrefabs.Length)
            {
                GameObject go = Instantiate(animalPrefabs[animalIndex], _spawnPoints[spawnIndices[i]].position, Quaternion.identity);
                go.gameObject.transform.SetParent(Canvas.transform);
                spawnedAnimals.Add(go);
            }
        }
    }

    private void SpawnThirdSet()
    {
        DeactivateSlots(slotsA);
        ActivateSlots(slotsC);
        DeactivateSlots(slotsB);
        DeactivateSlots(slotsD);
        DeactivateSlots(slotsE);

        DestroyAnimals(spawnedAnimals);

        AnswerSet = FC_AnimalPicker.instance.PickThirdSet();
        int[] selectedset = AnswerSet;

        List<int> spawnIndices = new List<int> { 0, 1, 2, 3};
        shuffleList(spawnIndices);
        for (int i = 0; i < selectedset.Length; i++)
        {
            int animalIndex = selectedset[i];
            if (animalIndex >= 0 && animalIndex < animalPrefabs.Length)
            {
                GameObject go = Instantiate(animalPrefabs[animalIndex], _spawnPoints[spawnIndices[i]].position, Quaternion.identity);
                go.gameObject.transform.SetParent(Canvas.transform);
                spawnedAnimals.Add(go);
            }
        }
    }

    private void SpawnFourthSet()
    {
        DeactivateSlots(slotsA);
        ActivateSlots(slotsD);
        DeactivateSlots(slotsC);
        DeactivateSlots(slotsB);
        DeactivateSlots(slotsE);

        DestroyAnimals(spawnedAnimals);

        AnswerSet = FC_AnimalPicker.instance.PickFourthSet();
        int[] selectedset = AnswerSet;

        List<int> spawnIndices = new List<int> { 0, 1, 2, 3};
        shuffleList(spawnIndices);
        for (int i = 0; i < selectedset.Length; i++)
        {
            int animalIndex = selectedset[i];
            if (animalIndex >= 0 && animalIndex < animalPrefabs.Length)
            {
                GameObject go = Instantiate(animalPrefabs[animalIndex], _spawnPoints[spawnIndices[i]].position, Quaternion.identity);
                go.gameObject.transform.SetParent(Canvas.transform);
                spawnedAnimals.Add(go);
            }
        }
    }

    private void SpawnFifthSet()
    {
        DeactivateSlots(slotsA);
        ActivateSlots(slotsB);
        DeactivateSlots(slotsC);
        DeactivateSlots(slotsD);
        DeactivateSlots(slotsE);

        DestroyAnimals(spawnedAnimals);

        AnswerSet = FC_AnimalPicker.instance.PickFifthSet();
        int[] selectedset = AnswerSet;

        List<int> spawnIndices = new List<int> { 0, 1, 2, 3, 4};
        shuffleList(spawnIndices);
        for (int i = 0; i < selectedset.Length; i++)
        {
            int animalIndex = selectedset[i];
            if (animalIndex >= 0 && animalIndex < animalPrefabs.Length)
            {
                GameObject go = Instantiate(animalPrefabs[animalIndex], _spawnPoints[spawnIndices[i]].position, Quaternion.identity);
                go.gameObject.transform.SetParent(Canvas.transform);
                spawnedAnimals.Add(go);
            }
        }
    }



    public void submit()
    {

        if(currentRound > 5)
        {
            Debug.Log("Game Over");
        }

        GameObject[] currentSlots = slotsPerRound[currentRound];

        bool areOccupied = true;
        foreach (GameObject slot in currentSlots)
        {
            if (!slot.GetComponent<FC_SlotHolder>().isoccupied)
            {
                areOccupied = false;
                break;
            }
        }

        if (areOccupied)
        {
            currentPlace = 0;
            for (int i = 0; i < AnswerSet.Length; i++)
            {
                if (slotsA[i].transform.GetChild(0).GetComponent<FC_AnimalInfo>().AnimalID == AnswerSet[i])
                {
                    Debug.Log("Correct");
                    currentPlace++;
                }
                else
                {
                    Debug.Log("Incorrect");
                }
            }
            if (currentPlace >= AnswerSet.Length)
            {
                Debug.Log("You have completed the food chain");
                currentRound++;
                PickRound();

            }
            else
            {
                Debug.Log("You have " + currentPlace + " Correct");
                
            }
        }
        else
        {
            Debug.Log("CANT SUBMIT");
        }
    }

    public IEnumerator SecondRound()
    {
        yield return new WaitForSeconds(1);
        SpawnSecondSet();
    }

    public IEnumerator ThirdRound()
    {
        yield return new WaitForSeconds(1);
        SpawnThirdSet();
    }

    public IEnumerator FourthRound()
    {
        yield return new WaitForSeconds(1);
        SpawnFourthSet();
    }

    public IEnumerator FifthRound()
    {
        yield return new WaitForSeconds(1);
        SpawnFifthSet();
    }


    private void shuffleList(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i);
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private void ActivateSlots(GameObject[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetActive(true);
        }
    }

    private void DeactivateSlots(GameObject[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetActive(false);
        }
    }

    private void DestroyAnimals(List<GameObject> animals)
    {
        foreach (GameObject animal in animals)
        {
            Destroy(animal);
        }
    }

}
