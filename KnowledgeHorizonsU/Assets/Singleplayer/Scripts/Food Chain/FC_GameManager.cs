using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FC_GameManager : MonoBehaviour
{
    public static FC_GameManager instance;

    [SerializeField] GameObject[] slotsA;

    [SerializeField] GameObject[] animalPrefabs;

    [SerializeField]
    Transform[] _spawnPoints;

    [SerializeField] Canvas Canvas;

    private int[] AnswerSet;

    int currentPlace;
    //private bool canSubmit = false;

    private List<string> correctChain = new List<string> { "Plant", "Ant", "Lion" };
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }


    }

    private void Start()
    {
        SpawnFirstSet();
        //ActivateSlots();
    }

    private void SpawnFirstSet()
    {
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
            }
        }
    }

    public void submit()
    {

        if (slotsA[0].GetComponent<FC_SlotHolder>().isoccupied && slotsA[1].GetComponent<FC_SlotHolder>().isoccupied && slotsA[2].GetComponent<FC_SlotHolder>().isoccupied)
        {
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
                currentPlace = 0;

            }
            else
            {
                Debug.Log("You have " + currentPlace + " Correct");
                currentPlace = 0;
            }
        }
        else
        {
            Debug.Log("CANT SUBMIT");
        }
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

}
