using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ERT_GameManager : MonoBehaviour
{
    public static ERT_GameManager instance;

    [SerializeField] int FruitsToCollect;
    [SerializeField] TMP_Text FruitText;

    private int FruitsCollected;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void FruitCollection()
    {
        FruitsCollected++;
        FruitText.text = (FruitsCollected.ToString() + "/" + FruitsToCollect.ToString());
        if(FruitsCollected >= FruitsToCollect)
        {
            Debug.Log("Fruits collededt");
        }
    }
}
