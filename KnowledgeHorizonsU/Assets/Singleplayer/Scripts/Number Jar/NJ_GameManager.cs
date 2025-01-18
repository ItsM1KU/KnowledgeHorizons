using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NJ_GameManager : MonoBehaviour
{
    public static NJ_GameManager instance;


    private void Awake()
    {
        if (instance == null) { 
            instance = this;
        }
    }
}
