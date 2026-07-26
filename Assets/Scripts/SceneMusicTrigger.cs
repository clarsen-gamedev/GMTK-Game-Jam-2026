

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicTrigger : MonoBehaviour
{
    #region Variables
    public enum TrackType { TITLE, MAIN_GAME, GAME_OVER, NONE }

    [SerializeField] private TrackType trackToPlay;
    [SerializeField] private float fadeDuration = 0.5f;
    #endregion

    #region Functions
    private void Start()
    {
        if (AudioManager.Instance == null) return;

        switch (trackToPlay)
        {
            case TrackType.TITLE:
                AudioManager.Instance.PlayTitleTheme();
                break;
            case TrackType.MAIN_GAME:
                AudioManager.Instance.PlayMainGameTheme();
                break;
            case TrackType.GAME_OVER:
                AudioManager.Instance.PlayGameOverTheme();
                break;
        }
    }
    #endregion
}