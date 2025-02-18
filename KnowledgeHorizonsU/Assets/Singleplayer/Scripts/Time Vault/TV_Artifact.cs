using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV_Artifact : MonoBehaviour
{
    [SerializeField] private string artifactName;

    [TextArea]
    [SerializeField] private string artifactDescription;

    [SerializeField] private Sprite artifactSprite;

    public string ArtifactName => artifactName;
    public string ArtifactDescription => artifactDescription;
    public Sprite ArtifactSprite => artifactSprite;

}
