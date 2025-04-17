using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERT_EnemyAI : MonoBehaviour
{
    [Header("Patrol References")]
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform firePosition;
    [SerializeField] GameObject projectilePrefab;

    [Header("Enemy Settings")]
    [SerializeField] float health;
    [SerializeField] float speed;
    [SerializeField] float MinDistanceToPlayer;
    [SerializeField] float shotSpeed;
    [SerializeField] float fireRate;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    private float shotCounter;
    private float currentHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointA;
        anim.SetBool("isMoving", true);
        currentHealth = health;
    }

    private void Update()
    {
        

        if (Vector2.Distance(transform.position, playerTransform.position) < MinDistanceToPlayer)
        {

            rb.velocity = Vector2.zero;
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                shotCounter = fireRate;
                anim.SetBool("isShooting", true);
                AttackFunction();
            }
        }
        else
        {
            anim.SetBool("isShooting", false);
            Patrol();
            shotCounter = 0;
        }
    }


    private void Patrol()
    {
        if (currentPoint == pointB)
        {
            anim.SetBool("isMoving", true);
            transform.localScale = new Vector3(-1, 1, 1);
            rb.velocity = new Vector2(speed, 0);
        }
        else if (currentPoint == pointA)
        {
            anim.SetBool("isMoving", true);
            transform.localScale = new Vector3(1, 1, 1);
            rb.velocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB)
        {
            StartCoroutine(Idle(pointA));
        }
        else if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA)
        {
            StartCoroutine(Idle(pointB));
        }
    }

    private void AttackFunction()
    {
        FaceThePlayer();
        GameObject GO = Instantiate(projectilePrefab, firePosition.position, firePosition.rotation);
        Rigidbody2D rb = GO.GetComponent<Rigidbody2D>();
        Vector2 direction = (playerTransform.position - firePosition.position).normalized;   
        rb.AddForce(direction * shotSpeed, ForceMode2D.Impulse);
        Destroy(GO, 1f);
    }


    private void FaceThePlayer()
    {
        Vector3 localScale = transform.localScale;
        if(transform.position.x < playerTransform.position.x)
        {
            localScale.x = -1;
        }
        else
        {
            localScale.x = 1;
        }
        transform.localScale = localScale;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.position, 0.5f);
        Gizmos.DrawLine(transform.position, playerTransform.position);
    }

    #region Enemy Coroutines

    public IEnumerator Idle(Transform point)
    {
        rb.velocity = Vector2.zero;
        anim.SetBool("isMoving", false);
        yield return new WaitForSeconds(2f);
        anim.SetBool("isMoving", true);
        if(point == pointA)
        {
            currentPoint = pointA;
            transform.localScale = new Vector3(1, 1, 1);
            rb.velocity = new Vector2(-speed, 0);
        }
        else if (point == pointB)
        {
            currentPoint = pointB;
            transform.localScale = new Vector3(-1, 1, 1);
            rb.velocity = new Vector2(speed, 0);
        }
        yield return null;
    }

    #endregion
}
