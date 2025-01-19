using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NJ_TriggerLoss : MonoBehaviour
{
    private float timer;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Loss")
        {
            timer += Time.deltaTime;
            if(timer > NJ_GameManager.instance.timeOverLimit)
            {
                NJ_GameManager.instance.GameOver();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Loss")
        {
            timer = 0f;
        }
    }
}
