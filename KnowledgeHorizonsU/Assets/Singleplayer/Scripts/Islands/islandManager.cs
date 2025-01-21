using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG;
using DG.Tweening;

public class islandManager : MonoBehaviour
{
    public static islandManager instance;

    [SerializeField] Button mathButton;

    private void Awake()
    {
        if (instance == null) 
        {   
            instance = this;
        }
    }
    




    




    /* NOTES
     * function for each island that gets executed when the island is pressed
     * 
     * 
     */
}
