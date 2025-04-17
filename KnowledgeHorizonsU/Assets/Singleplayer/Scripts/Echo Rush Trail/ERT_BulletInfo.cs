using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERT_BulletInfo : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ERT_PlayerHealth player = collision.gameObject.GetComponent<ERT_PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            ERT_EnemyAI enemy = collision.gameObject.GetComponent<ERT_EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
