using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CTI_Enemy : MonoBehaviour
{
    public float jumpDelay = 5f;
    private CTI_IceBlock currentBlock;
    public List<CTI_IceBlock> pathToCenter;
    private int stepIndex = 0;

    private void Start()
    {
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        while (stepIndex < pathToCenter.Count)
        {
            MoveToBlock(pathToCenter[stepIndex]);
            stepIndex++;
            yield return new WaitForSeconds(jumpDelay);
        }

        Debug.Log("Enemy reached the center!");
    }

    void MoveToBlock(CTI_IceBlock newBlock)
    {
        if (currentBlock != null)
        {
            currentBlock.EnemyLeft();
        }

        currentBlock = newBlock;
        transform.position = newBlock.transform.position;
        currentBlock.EnemyLanded(this);
    }
}
