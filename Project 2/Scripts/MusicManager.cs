using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;
    public AudioMixerGroup mixerGroup;

    public AudioClip clip;
    public string name;
    [Range(0f, 1f)]
    public float volume;
    public bool loop = true;
    [HideInInspector]
    public AudioSource source;

    public bool isPlaying = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = loop;
        source.volume = volume;
        source.outputAudioMixerGroup = mixerGroup;
    }

    void Start()
    {
        StartMusic();
    }

    public void StartMusic()
    {
        if (!isPlaying)
        {
            source.Play();
            isPlaying = true;
        }
    }
}
