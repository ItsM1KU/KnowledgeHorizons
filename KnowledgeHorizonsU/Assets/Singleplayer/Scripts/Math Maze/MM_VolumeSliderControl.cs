using UnityEngine;
using UnityEngine.UI;

public class MM_VolumeSliderControl : MonoBehaviour
{
    public AudioSource backgroundMusic; // Reference to the background music AudioSource
    private Slider slider; // Reference to the Slider component

    private const string VolumeKey = "BackgroundMusicVolume"; // Key to save the volume in PlayerPrefs

    private void Awake()
    {
        // Get the Slider component
        slider = GetComponent<Slider>();

        // Load the saved volume or set a default value
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 0.5f); // Default to 0.5 if no volume is saved
        slider.value = savedVolume;

        // Apply the saved volume to the background music
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = savedVolume;
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

        // Save the volume setting
        PlayerPrefs.SetFloat(VolumeKey, volume);
    }

    private void OnDestroy()
    {
        // Remove listener when the object is destroyed
        slider.onValueChanged.RemoveListener(SetVolume);
    }
}
