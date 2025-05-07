using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource bgAudio;
    [SerializeField] private Slider volumeSlider;

    private void Update()
    {
        bgAudio.volume = volumeSlider.value;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Islands");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
