using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTS_Spawner : MonoBehaviour
{
    [SerializeField] GameObject lilypadPrefab;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float spawnRate = 0.5f;

    private float spawnTime;

    private void Update()
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0)
        {
            spawnTime = spawnRate;
            SpawnLilypad();
        }
    }

    void SpawnLilypad()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];
        GameObject GO = Instantiate(lilypadPrefab, spawnPoint.position, spawnPoint.rotation);
        Destroy(GO, 10f);
    }
}
