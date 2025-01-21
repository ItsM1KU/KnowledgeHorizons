using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class islandManager : MonoBehaviour
{
    public static islandManager instance;

    //[SerializeField] Button mathButton;

    [SerializeField] GameObject mathPanel;

    private void Awake()
    {
        if (instance == null) 
        {   
            instance = this;
        }
    }
    
    public void openMathPanel()
    {
        mathPanel.SetActive(true);
    }

    public void closeMathPanel() 
    { 
        mathPanel.SetActive(false);
    }

    public void playMM()
    {
        SceneManager.LoadScene("MM_gamescene");
    }

    public void playNJ()
    {
        SceneManager.LoadScene("NJ_gamescene");
    }

    public void playBP()
    {
        SceneManager.LoadScene("BP_gamescene");
    }





    




    /* NOTES
     * function for each island that gets executed when the island is pressed
     * 
     * 
     */
}
