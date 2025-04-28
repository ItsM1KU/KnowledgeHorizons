using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CTS_Stream : MonoBehaviour
{
    private float checkDelay = 0.2f;
    private float checkTimer = 0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkTimer += Time.deltaTime;

            if (checkTimer >= checkDelay)
            {
                CTS_Frog frog = collision.GetComponent<CTS_Frog>();
                if (frog != null && !frog.isOnLilypad)
                {
                    Debug.Log("Game Over: Frog fell in water!");
                    CTS_Score.score = 0;
                    frog.LoseLife();
                }

                checkTimer = 0f; 
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkTimer = 0f; 
        }
    }
}
