using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERT_PlayerHealth : MonoBehaviour
{
    [Header("Player Details")]
    [SerializeField] float health;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = health;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            //invoke game over!!
        }
    }
}
