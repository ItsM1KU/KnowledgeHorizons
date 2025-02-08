using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FC_GameManager : MonoBehaviour
{
   public static FC_GameManager instance;

    [SerializeField] GameObject[] _slots;

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
    }

    private void SpawnFirstSet()
    {
        AnswerSet = FC_AnimalPicker.instance.PickFirstSet();
        int[] selectedset = AnswerSet;
        for (int i= 0; i < selectedset.Length; i++)
        {
            int animalIndex = selectedset[i];
            if (animalIndex >= 0 && animalIndex < animalPrefabs.Length) 
            {
                GameObject go = Instantiate(animalPrefabs[animalIndex], _spawnPoints[i].position, Quaternion.identity);
                go.gameObject.transform.SetParent(Canvas.transform);
            }
        }
    }

    public void submit()
    {

        if (_slots[0].GetComponent<FC_SlotHolder>().isoccupied && _slots[1].GetComponent<FC_SlotHolder>().isoccupied && _slots[2].GetComponent<FC_SlotHolder>().isoccupied)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].transform.GetChild(0).GetComponent<FC_AnimalInfo>().AnimalID == AnswerSet[i])
                {
                    Debug.Log("Correct");
                    currentPlace++;
                }
                else
                {
                    Debug.Log("Incorrect");
                }
            }
            if(currentPlace >= _slots.Length)
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
}
