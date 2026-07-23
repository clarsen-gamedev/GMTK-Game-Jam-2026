// Name: CameraFollow.cs
// Author: Connor Larsen
// Date: 07/22/2026
// Description: Controls the camera which follows the player as they move

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Variables
    [Header("Target Settings")]
    [SerializeField] private Transform target;  // Drag player here

    [Header("Follow Settings")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -15f);
    #endregion

    #region Functions
    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed + Time.deltaTime);
        transform.position = smoothedPosition;
    }
    #endregion
}
