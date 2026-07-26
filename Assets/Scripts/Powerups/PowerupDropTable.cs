using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDropTable : MonoBehaviour
{
    #region Variables
    [System.Serializable]
    public struct DropItem
    {
        public PowerupData powerup;
        [Range(0f, 100f)] public float dropChance; // Chance out of 100%
    }

    [Header("Drop Settings")]
    [SerializeField] private List<DropItem> dropList;
    #endregion

    #region Functions
    public void TryDropPowerup(Vector3 spawnPosition)
    {
        if (dropList == null || dropList.Count == 0) return;

        foreach (var item in dropList)
        {
            if (item.powerup == null || item.powerup.worldPrefab == null) continue;

            float roll = Random.Range(0f, 100f);
            if (roll <= item.dropChance)
            {
                // Instantiate the powerup prefab at enemy death position
                GameObject spawnedObj = Instantiate(item.powerup.worldPrefab, spawnPosition, Quaternion.identity);

                if (spawnedObj.TryGetComponent<PowerupItem>(out var itemScript))
                {
                    itemScript.Initialize(item.powerup);
                }

                break; // Stop after dropping one item
            }
        }
    }
    #endregion
}