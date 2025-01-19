using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NJ_TriggerLoss : MonoBehaviour
{
    private float timer;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            timer += Time.deltaTime;
            if (timer > NJ_GameManager.instance.timeOverLimit)
            {
                NJ_GameManager.instance.GameOver();
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            timer = 0f;
        }
    }
}
