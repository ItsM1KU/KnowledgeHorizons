using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NJ_BallCombinerLogic : MonoBehaviour
{
    private NJ_BallInfo _ballInfo;
    private bool _isCombining = false; // Prevent multiple collisions

    private void Awake()
    {
        _ballInfo = GetComponent<NJ_BallInfo>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ensure this object isn't already combining
        if (_isCombining) return;

        // Check if the collided object is a fruit
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            NJ_BallInfo otherBallInfo = collision.gameObject.GetComponent<NJ_BallInfo>();

            // Ensure the collided object is a valid fruit with the same index
            if (otherBallInfo != null && otherBallInfo.BallIndex == _ballInfo.BallIndex)
            {
                _isCombining = true;

                // Handle the combination logic
                CombineFruits(collision.gameObject);
            }
        }
    }

    private void CombineFruits(GameObject otherFruit)
    {
        NJ_GameManager.instance.IncreaseScore(_ballInfo.pointsWhenCombined);
        // Destroy the current and collided fruit
        

        // Check if this is the last fruit in the sequence
        if (_ballInfo.BallIndex == NJ_BallSelector.Instance.balls.Length - 1)
        {
            DestroyFruits(gameObject, otherFruit);
            return;
        }
        else
        {
            // Calculate the middle position
            Vector3 middlePosition = (transform.position + otherFruit.transform.position) / 2f;

            // Spawn the next fruit
            GameObject nextFruit = InstantiateNextFruit(_ballInfo.BallIndex, middlePosition);

            // Update the score
            NJ_GameManager.instance.IncreaseScore(_ballInfo.pointsWhenCombined);

            DestroyFruits(gameObject, otherFruit);
        }
  
    }

    private void DestroyFruits(GameObject fruit1, GameObject fruit2)
    {
        Destroy(fruit1);
        Destroy(fruit2);
    }

    private GameObject InstantiateNextFruit(int currentIndex, Vector3 position)
    {
        // Get the prefab for the next fruit
        GameObject nextFruitPrefab = NJ_BallSelector.Instance.balls[currentIndex + 1];
        GameObject nextFruit = Instantiate(nextFruitPrefab, position, Quaternion.identity, NJ_GameManager.instance.transform);

        // Optionally mark it as a combined fruit
        NJ_ColliderInformer collider = nextFruit.GetComponent<NJ_ColliderInformer>();
        if (collider != null)
        {
            collider.wasCombined = true;
        }

        return nextFruit;
    }
}
