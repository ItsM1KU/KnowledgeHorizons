using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class islandManager : MonoBehaviour
{
    public static islandManager instance;

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
