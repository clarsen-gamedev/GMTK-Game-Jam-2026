// Name: RadarCamera.cs
// Author: Connor Larsen
// Date: 07/23/2026
// Description: Keeps the radar camera centered directly over the player at all times

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RadarCamera : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float height = 20f;
    #endregion

    #region Functions
    private void Start()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }
    }

    private void LateUpdate()
    {
        if (playerTransform == null) return;

        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -height);
    }
    #endregion
}