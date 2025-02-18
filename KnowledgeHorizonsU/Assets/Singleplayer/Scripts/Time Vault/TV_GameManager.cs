using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TV_GameManager : MonoBehaviour
{
    public static TV_GameManager Instance;

    // Artifact Details UI
    [SerializeField] GameObject artifactUI;
    [SerializeField] Image artifactImage;
    [SerializeField] TextMeshProUGUI artifactName;
    [SerializeField] TextMeshProUGUI artifactDescription;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void SetupArtifactUI(Collider2D artifactCollider)
    {
       if(artifactCollider != null)
        {
            TV_Artifact artifact = artifactCollider.GetComponent<TV_Artifact>();
            if (artifact != null)
            {
                artifactUI.SetActive(true);
                artifactImage.sprite = artifact.ArtifactSprite;
                artifactName.text = artifact.ArtifactName;
                artifactDescription.text = artifact.ArtifactDescription;
            }
        }
    }

    public void CloseArtifactUI()
    {
        artifactUI.SetActive(false);
    }
}
