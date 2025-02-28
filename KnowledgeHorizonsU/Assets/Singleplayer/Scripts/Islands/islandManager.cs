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
    [SerializeField] GameObject sciencePanel;
    [SerializeField] GameObject socialPanel;

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

    public void openSciencePanel()
    {
        sciencePanel.SetActive(true);
    }

    public void closeSciencePanel()
    {
        sciencePanel.SetActive(false);
    }

    public void openSocialPanel()
    {
        socialPanel.SetActive(true);
    }

    public void closeSocialPanel()
    {
        socialPanel.SetActive(false);
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

    public void playFC()
    {
        SceneManager.LoadScene("FC_gamescene");
    }

    public void playFB()
    {
        SceneManager.LoadScene("FB_gamescene");
    }

    public void playTV()
    {
        SceneManager.LoadScene("TV_gamescene");
    }








    /* NOTES
     * function for each island that gets executed when the island is pressed
     * 
     * 
     */
}
