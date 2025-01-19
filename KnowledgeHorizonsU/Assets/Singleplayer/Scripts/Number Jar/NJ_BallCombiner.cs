using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NJ_BallCombiner : MonoBehaviour
{
    private NJ_BallInfo _ballInfo;
    private int layerIndex;

    private void Awake()
    {
        _ballInfo = GetComponent<NJ_BallInfo>();
        layerIndex = gameObject.layer;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerIndex)
        {
            NJ_BallInfo collidedBallInfo = collision.gameObject.GetComponent<NJ_BallInfo>();


            if (collidedBallInfo != null)
            {
                if (collidedBallInfo.BallIndex == _ballInfo.BallIndex)
                {
                    int thisID = gameObject.GetInstanceID();
                    int otherID = collision.gameObject.GetInstanceID();

                    GameObject thisBall = gameObject;
                    GameObject otherBall = collision.gameObject;

                    if (thisID > otherID)
                    {
                        NJ_GameManager.instance.IncreaseScore(_ballInfo.pointsWhenCombined);
                        if (_ballInfo.BallIndex == NJ_BallSelector.Instance.balls.Length - 1)
                        {
                            NJ_GameManager.instance.popSFX.Play();
                            Destroy(collision.gameObject);
                            Destroy(gameObject);
                        }
                        else
                        {
                            Vector3 middlePos = (gameObject.transform.position + collision.gameObject.transform.position) / 2f;
                            GameObject go = Instantiate(pickNextBall(_ballInfo.BallIndex), NJ_GameManager.instance.transform);
                            go.transform.position = middlePos;

                            NJ_ColliderInformer collider = go.GetComponent<NJ_ColliderInformer>();
                            if (collider != null)
                            {
                                collider.wasCombined = true;
                            }
                            NJ_GameManager.instance.popSFX.Play();
                            Destroy(collision.gameObject);
                            Destroy(gameObject);
                        }
                    }

                }
            }
        }
    }

    private GameObject pickNextBall(int ballIndex)
    {
        GameObject go = NJ_BallSelector.Instance.balls[ballIndex + 1];
        return go;
    }
}
