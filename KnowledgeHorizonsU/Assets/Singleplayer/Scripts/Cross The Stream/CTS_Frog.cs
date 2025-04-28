using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CTS_Frog : MonoBehaviour
{
    public bool isOnLilypad = false;
    private Rigidbody2D rb;
    private Camera mainCamera;


    [SerializeField] List<GameObject> lives = new List<GameObject>();

    private int livesCount = 3;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            move(Vector2.up);

        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            move(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            move(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            move(Vector2.right);
        }

        if (isOnLilypad)
        {
            CheckIfOffScreen();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("FinishPoint"))
        {
            CTS_Score.score += 10;
            RespawnFrog();
        }
    }

    private void move(Vector2 direction)
    {
        Vector2 newPosition = rb.position + direction; 
        newPosition = clampToCamera(newPosition);      
        rb.MovePosition(newPosition);
    }


    private Vector2 clampToCamera(Vector2 position)
    {
        Vector2 pos = position;

        Vector2 min = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);
        return pos;
    }

    private void CheckIfOffScreen()
    {
        Vector2 screenPoint = mainCamera.WorldToViewportPoint(transform.position);

        bool offScreen = screenPoint.x < 0 || screenPoint.x > 1;

        if (offScreen && isOnLilypad)
        {
            CTS_Score.score = 0;
            LoseLife();
        }
    }

    public void LoseLife()
    {
        if (livesCount > 0)
        {
            livesCount--;
            Destroy(lives[livesCount]);
            RespawnFrog();
        }
        if (livesCount <= 0)
        {
            Debug.Log("Game Over");
            CTS_SceneManager.instance.EndGame();
        }
    }

    private void RespawnFrog()
    {
        
        rb.position = new Vector2(0, -4); 

        isOnLilypad = false;
        transform.SetParent(null);
    }
}
