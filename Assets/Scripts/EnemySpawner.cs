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
    [SerializeField] private float baseSpawnInterval = 2f;
    [SerializeField] private float minSpawnInterval = 0.1f;

    // Shared accross all spawners (how many times spawning has been scaled)
    private static int globalDifficultyTier = 0;

    // Calculate current inverval based on base rate and global difficulty tier
    private float currentInterval => Mathf.Max(minSpawnInterval, baseSpawnInterval - (globalDifficultyTier * 0.15f));

    private float nextSpawnTime;
    #endregion

    #region Functions
    private void Start()
    {
        nextSpawnTime = Time.time + currentInterval;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + currentInterval;
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        Transform chosenSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject prefabToSpawn = (Random.value > 0.75f) ? playerChaserPrefab : defenseChaserPrefab;

        Instantiate(prefabToSpawn, chosenSpawn.position, Quaternion.identity);
    }

    // Call this whenever the spawn rate should increase
    public static void IncreaseGlobalSpawnRate()
    {
        globalDifficultyTier++;
        Debug.Log("Spawn rate increased to {currentInterval}!");
    }

    // Call this when restarting the game so static variables reset
    public static void ResetDifficulty()
    {
        globalDifficultyTier = 0;
    }
    #endregion
}