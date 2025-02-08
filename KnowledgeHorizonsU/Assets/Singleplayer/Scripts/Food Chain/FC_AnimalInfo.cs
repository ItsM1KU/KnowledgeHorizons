using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FC_AnimalInfo : MonoBehaviour
{
    [SerializeField] private int animalID;
    [SerializeField] private string animalName;

    public int AnimalID
    {
        get { return animalID; }
        set { animalID = value; }
    }
    public string AnimalName
    {
        get { return animalName; }
        set { animalName = value; }
    }
}
