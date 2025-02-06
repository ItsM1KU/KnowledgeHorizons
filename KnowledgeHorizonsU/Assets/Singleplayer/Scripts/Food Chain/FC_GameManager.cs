using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FC_GameManager : MonoBehaviour
{
   public static FC_GameManager instance;

    [SerializeField] List<FC_SlotHolder> slots;
    [SerializeField] GameObject[] _slots;

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

    public void submit()
    {
       /* if (slots[0].isoccupied && slots[1].isoccupied && slots[2].isoccupied)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].transform.GetChild(0).GetComponent<FC_dragNdrop>().animalName == correctChain[i])
                {
                    Debug.Log("Correct");

                }
                else
                {
                    Debug.Log("Incorrect");
                }
            }
        }
        else
        {
            Debug.Log("CANT SUBMIT");
        }
       */

        if (_slots[0].GetComponent<FC_SlotHolder>().isoccupied && _slots[1].GetComponent<FC_SlotHolder>().isoccupied && _slots[2].GetComponent<FC_SlotHolder>().isoccupied)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].transform.GetChild(0).GetComponent<FC_dragNdrop>().animalName == correctChain[i])
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
