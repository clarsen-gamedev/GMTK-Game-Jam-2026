// Name: DefensePointManager.cs
// Author: Connor Larsen
// Date: 07/23/2026
// Description: Manages an array of defense points, cycling through them on a timer

using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using UnityEngine;

public class DefensePointManager : MonoBehaviour
{
    #region Variables
    public static DefensePointManager Instance { get; private set; }

    [Header("Defense Points Array")]
    [SerializeField] private Transform[] defensePoints;

    [Header("Switch Settings")]
    [SerializeField] private float switchInterval = 30f;

    [Header("Debug Color Settings")]
    [SerializeField] private bool enableDebugColors = true;
    [SerializeField] private Color activeColor = new Color(0.3f, 0.7f, 1f, 1f);
    [SerializeField] private Color inactiveColor = Color.gray;

    private int currentPointIndex = 0;
    private float nextSwitchTime;

    // Track active defense point enemies so they can update their target in real time
    private List<Enemy> activeDefenseEnemies = new List<Enemy>();
    #endregion

    #region Functions
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        nextSwitchTime = Time.time + switchInterval;

        // Select a random defense point at the start of the game
        if (defensePoints != null && defensePoints.Length > 0)
        {
            currentPointIndex = Random.Range(0, defensePoints.Length);
        }

        UpdateDefensePointColors();
    }

    private void Update()
    {
        if (defensePoints == null || defensePoints.Length <= 1) return;

        if (Time.time >= nextSwitchTime)
        {
            SwitchToNextDefensePoint();
            nextSwitchTime = Time.time + switchInterval;
        }
    }

    private void SwitchToNextDefensePoint()
    {
        // Randomly select a new point index (excluding the current point)
        int newIndex = currentPointIndex;
        while (newIndex == currentPointIndex)
        {
            newIndex = Random.Range(0, defensePoints.Length);
        }
        currentPointIndex = newIndex;
        Transform newTarget = GetCurrentDefensePoint();

        // Update all currently living defense enemies
        activeDefenseEnemies.RemoveAll(enemy => enemy == null);
        foreach (Enemy enemy in activeDefenseEnemies)
        {
            enemy.SetNewTarget(newTarget);
        }

        // Refresh sprite colors to reflect the new active target
        UpdateDefensePointColors();

        // Show Pop-Up Notification on Canvas
        if (TimerFeedbackUI.Instance != null && newTarget != null)
        {
            TimerFeedbackUI.Instance.ShowDefensePointNotification($"DEFENSE POINT MOVED!");
        }

        Debug.Log($"Defense Point Swapped! New Target: {newTarget.gameObject.name}");
    }

    private void UpdateDefensePointColors()
    {
        if (!enableDebugColors || defensePoints == null) return;

        for (int i = 0; i < defensePoints.Length; i++)
        {
            if (defensePoints[i] == null) continue;

            // Search the point or it's child objects for a SpriteRenderer
            SpriteRenderer sr = defensePoints[i].GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = (i == currentPointIndex) ? activeColor : inactiveColor;
            }
        }
    }
    
    public Transform GetCurrentDefensePoint()
    {
        if (defensePoints == null || defensePoints.Length == 0) return null;
        return defensePoints[currentPointIndex];
    }

    public void RegisterDefenseEnemy(Enemy enemy)
    {
        if (!activeDefenseEnemies.Contains(enemy))
        {
            activeDefenseEnemies.Add(enemy);
        }
    }

    public void UnregisterDefenseEnemy(Enemy enemy)
    {
        if (activeDefenseEnemies.Contains(enemy))
        {
            activeDefenseEnemies.Remove(enemy);
        }
    }
    #endregion
}