using UnityEngine;

public class CTI_IceBlock : MonoBehaviour
{
    public int tapsToBreak = 5;
    private int currentTaps = 0;

    private bool isEnemyOnBlock = false;
    private CTI_Enemy enemyOnBlock;

    public void EnemyLanded(CTI_Enemy enemy)
    {
        isEnemyOnBlock = true;
        enemyOnBlock = enemy;
        currentTaps = 0; // reset taps
    }

    public void EnemyLeft()
    {
        isEnemyOnBlock = false;
        enemyOnBlock = null;
        currentTaps = 0;
    }

    private void OnMouseDown()
    {
        if (!isEnemyOnBlock) return;

        currentTaps++;
        Debug.Log("Taps: " + currentTaps);

        if (currentTaps >= tapsToBreak)
        {
            SinkEnemy();
        }
    }

    private void SinkEnemy()
    {
        if (enemyOnBlock != null)
        {
            Destroy(enemyOnBlock.gameObject);
        }

        Destroy(gameObject); // Sink the block
    }
}
