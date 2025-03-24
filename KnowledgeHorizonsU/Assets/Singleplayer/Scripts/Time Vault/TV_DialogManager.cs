using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TV_DialogManager : MonoBehaviour
{
    public static TV_DialogManager Instance;

    [SerializeField] List<string> dialogA;

    [SerializeField] GameObject dialogBox;
    [SerializeField] TextMeshProUGUI dialogText;

    public bool isPresenting;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(StartDialog(dialogA));
    }


    public IEnumerator StartDialog(List<string> dialog)
    {
        isPresenting = true;
        dialogText.text = "";
        dialogBox.SetActive(true);

        for (int i = 0; i < dialog.Count; i++)
        {
            dialogText.text = "";
            yield return typeDialog(dialog[i]);
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1f);
        isPresenting = false;
        dialogBox.SetActive(false);

    }

    public IEnumerator typeDialog(string sentence)
    {
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1/15f);
        }
    }
}
