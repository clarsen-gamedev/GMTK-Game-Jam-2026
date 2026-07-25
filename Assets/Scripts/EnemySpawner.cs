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

    [Header("Kill Scale Settings")]
    [SerializeField] private int minKillsToIncrease = 10;
    [SerializeField] private int maxKillsToIncrease = 30;

    // Shared accross all spawners
    private static int globalDifficultyTier = 0;
    private static int totalKillsSinceLastIncrease = 0;
    private static int currentKillTarget = -1;

    // Calculate current inverval based on base rate and global difficulty tier
    private float currentInterval => Mathf.Max(minSpawnInterval, baseSpawnInterval - (globalDifficultyTier * 0.15f));

    private float nextSpawnTime;
    #endregion

    #region Functions
    private void Start()
    {
        nextSpawnTime = Time.time + currentInterval;
        if (currentKillTarget == -1)
        {
            RandomizeKillTarget();
        }
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

    // Call this whenever ANY enemy is defeated
    public static void RegisterEnemyKill(int minKills = 10, int maxKills = 30)
    {
        totalKillsSinceLastIncrease++;

        // Ensure we have a target set
        if (currentKillTarget <= 0)
        {
            currentKillTarget = Random.Range(minKills, maxKills + 1);
        }

        // Check if we hit or exceeded the random target
        if (totalKillsSinceLastIncrease >= currentKillTarget)
        {
            totalKillsSinceLastIncrease = 0;
            IncreaseGlobalSpawnRate();
            currentKillTarget = Random.Range(minKills, maxKills + 1);
        }
    }

    private static void RandomizeKillTarget()
    {
        currentKillTarget = Random.Range(10, 31);
    }

    // Call this whenever the spawn rate should increase
    public static void IncreaseGlobalSpawnRate()
    {
        globalDifficultyTier++;
        Debug.Log("Spawn rate increased to {currentInterval}!");

        // Trigger UI Notification
        if (TimerFeedbackUI.Instance != null)
        {
            TimerFeedbackUI.Instance.ShowSpawnRateNotification("You've angered the horde!\nSpawn rates increased!");
        }
    }

    // Call this when restarting the game so static variables reset
    public static void ResetDifficulty()
    {
        globalDifficultyTier = 0;
        totalKillsSinceLastIncrease = 0;
        currentKillTarget = -1;
    }
    #endregion
}