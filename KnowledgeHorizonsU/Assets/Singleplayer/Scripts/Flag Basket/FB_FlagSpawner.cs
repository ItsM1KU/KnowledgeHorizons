using UnityEngine;

public class FB_FlagSpawner : MonoBehaviour
{
    public GameObject flagPrefab; // Assign in Inspector
    public Transform[] spawnPoints; // Assign all 4 points
    public float spawnInterval = 3f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnFlags();
            timer = 0;
        }
    }

    void SpawnFlags()
    {
        foreach (Transform point in spawnPoints)
        {
            Instantiate(flagPrefab, point.position, Quaternion.identity);
        }
    }
}