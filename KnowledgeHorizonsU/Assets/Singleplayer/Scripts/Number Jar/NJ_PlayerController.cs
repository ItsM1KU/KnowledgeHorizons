using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NJ_PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] BoxCollider2D BoundaryCollider;
    [SerializeField] Transform BallTransformPos;

    private Bounds bounds;

    private float leftBound;
    private float rightBound;

    private float leftStartingBound;
    private float rightStartingBound;

    private float offset;

    private void Awake()
    {
        bounds = BoundaryCollider.bounds;

        offset = transform.position.x - BallTransformPos.position.x;

        leftBound = bounds.min.x + offset;
        rightBound = bounds.max.x + offset;

        leftStartingBound = bounds.min.x;
        rightStartingBound = bounds.max.x;
    }

    private void Update()
    {
        Vector3 newPos = transform.position + new Vector3(NJ_UserInput.MoveInput.x * moveSpeed * Time.deltaTime, 0, 0);
        newPos.x = Mathf.Clamp(newPos.x, leftBound, rightBound);

        transform.position = newPos;
    }

    public void ChangeBoundary(float extraWidth)
    {
        leftBound = leftStartingBound;
        rightBound = rightStartingBound;

        leftBound += NJ_ThrowBallController.Instance.bounds.extents.x + extraWidth;
        rightBound -= NJ_ThrowBallController.Instance.bounds.extents.x + extraWidth;
    }


}
