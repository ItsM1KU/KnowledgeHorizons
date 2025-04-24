using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERT_BulletInfo : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] float damage;
    [SerializeField] GameObject Player;
    [SerializeField] bool isEnemyBullet;

    private Rigidbody2D rb;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        if (isEnemyBullet)
        {
            Vector3 direction = (Player.transform.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x, direction.y) * 6f;

            float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot);
        }
    }

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
        else if(collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
