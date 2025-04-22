using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ERT_PlayerHealth : MonoBehaviour
{
    [Header("Player Details")]
    [SerializeField] float health;
    [SerializeField] TMP_Text healthText;
    private float currentHealth;

    private void Awake()
    {
        currentHealth = health;
        gameObject.SetActive(true);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthText.text = "Health: " + currentHealth;   
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            ERT_GameManager.instance.EndGame();
        }
    }
}
