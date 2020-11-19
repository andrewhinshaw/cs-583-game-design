using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    /*
     *   The purpose of this class is to manage all game assets. This includes
     *   environment sprites, powerups, and sounds. Use this class to Instantiate
     *   these various types of assets.
     */

    // Level element prefabs
    public Transform linePointPrefab;
    public Transform gridBackgroundPrefab;

    // Powerup prefabs
    public Transform plusOnePowerupPrefab;
    public Transform futureSightPowerupPrefab;
    public Transform gravToolPowerupPrefab;

    public Transform futureSightMarkerPrefab;

    private static GameAssets instance;

    public static GameAssets GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    public SoundAudioClip[] soundAudioClipArray;

    [Serializable]
    public class SoundAudioClip
    {
        public SoundsManager.Sound sound;
        public AudioClip audioClip;
    }
}
