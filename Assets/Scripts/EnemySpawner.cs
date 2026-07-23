// Name: EnemySpawner.cs
// Author: Connor Larsen
// Date: 07/23/2026
// Description: Handles target tracking, taking damage from bullets, and adjusting the game manager to alter the countdown

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Variables
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject playerChaserPrefab;
    [SerializeField] private GameObject defenseChaserPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 2f;

    private float nextSpawnTime;
    #endregion

    #region Functions
    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        Transform chosenSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject prefabToSpawn = (Random.value > 0.8f) ? playerChaserPrefab : defenseChaserPrefab;

        Instantiate(prefabToSpawn, chosenSpawn.position, Quaternion.identity);
    }
    #endregion
}