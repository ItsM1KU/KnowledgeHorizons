using UnityEngine;
using UnityEngine.UI;

public class MM_VolumeSliderControl : MonoBehaviour
{
    public AudioSource backgroundMusic; // Reference to the background music AudioSource
    private Slider slider; // Reference to the Slider component

    private void Awake()
    {
        // Get the Slider component
        slider = GetComponent<Slider>();

        // Ensure the slider is initialized with the current volume
        if (backgroundMusic != null)
        {
            slider.value = backgroundMusic.volume;
        }

        // Add listener for value change
        slider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float volume)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = volume; // Adjust the background music volume
        }
    }

    private void OnDestroy()
    {
        // Remove listener when the object is destroyed
        slider.onValueChanged.RemoveListener(SetVolume);
    }
}
