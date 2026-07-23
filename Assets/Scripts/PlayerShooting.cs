// Name: PlayerShooting.cs
// Author: Connor Larsen
// Date: 07/22/2026
// Description: Controls the player's shooting from their gun

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerShooting : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;

    private float nextFireTime;
    #endregion

    #region Functions
    private void Update()
    {
        // Ignore shooting inputs when game is paused
        if (PauseManager.IsPaused) return;

        // Read arrow inputs
        float shootX = 0f;
        float shootY = 0f;

        if (Input.GetKey(KeyCode.UpArrow)) shootY = 1f;
        if (Input.GetKey(KeyCode.DownArrow)) shootY = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) shootX = 1f;
        if (Input.GetKey(KeyCode.LeftArrow)) shootX = -1f;

        Vector2 shootDirection = new Vector2(shootX, shootY);

        // Fire if any arrow key is pressed and fire cooldown has passed
        if (shootDirection.sqrMagnitude > 0 && Time.time >= nextFireTime)
        {
            Shoot(shootDirection.normalized);
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot(Vector2 direction)
    {
        // Calculate angle to rotate bullet towards firing direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Create bullet, it will now begin it's mission to slay it's enemies
        Instantiate(bulletPrefab, firePoint.position, rotation);
    }
    #endregion
}