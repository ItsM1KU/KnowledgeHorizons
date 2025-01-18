using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NJ_AimlineController : MonoBehaviour
{
    [SerializeField] Transform BallDropTransform;
    [SerializeField] Transform BottomTransform;

    float topPos;
    float bottomPos;
    float x;

    [SerializeField] LineRenderer lineRenderer;

    private void Update()
    {
        x = BallDropTransform.position.x;

        topPos = BallDropTransform.position.y;
        bottomPos = BottomTransform.position.y;

        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;

        lineRenderer.SetPosition(0, new Vector3(x, topPos));
        lineRenderer.SetPosition(1, new Vector3(x, bottomPos));
    }

    private void OnValidate()
    {
        x = BallDropTransform.position.x;

        topPos = BallDropTransform.position.y;
        bottomPos = BottomTransform.position.y;

        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;

        lineRenderer.SetPosition(0, new Vector3(x, topPos));
        lineRenderer.SetPosition(1, new Vector3(x, bottomPos));
    }

}
