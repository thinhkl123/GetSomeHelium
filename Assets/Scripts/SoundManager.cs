using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum AudioType { Music, Sound }
public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    public bool soundMute = false, musicMute = false;
    public float musicVolume = 1f, sfxVolume = 1f;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        foreach (Sound s in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = s.Clip;
            source.volume = s.volume;
            source.pitch = s.pitch;
            source.loop = s.loop;

            s.Source = source;

            if (s.playOnStart)
                s.Source.Play();
        }

    }

    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, so => so.name == soundName);

        if (s == null)
        {
            // Debug.LogError("Sound with name " + soundName + " doesn't exist!");
            return;
        }

        if (s.audioType == AudioType.Music && !musicMute)
        {
            s.Source.Play();
        }
        else if (s.audioType == AudioType.Sound && !soundMute)
        {
            s.Source.Play();
        }

    }

    public void Stop(string soundName)
    {

        Sound s = Array.Find(sounds, so => so.name == soundName);

        if (s == null)
        {
            Debug.LogError("Sound with name " + soundName + " doesn't exist!");
            return;
        }

        s.Source.Stop();
    }

    public void SetVolumeMusic(float value)
    {
        musicVolume = value;
        foreach (Sound s in sounds)
        {
            if (s.audioType == AudioType.Music)
            {
                s.volume = value;
                s.Source.volume = value;
            }
        }
    }

    public void SetVolumeSFX(float value)
    {
        sfxVolume = value;
        foreach (Sound s in sounds)
        {
            if (s.audioType == AudioType.Sound)
            {
                s.volume = value;
                s.Source.volume = value;
            }
        }
    }
}

[Serializable]
public class Sound
{
    public string name;
    public AudioType audioType;
    public AudioClip Clip;


    //[HideInInspector]
    public AudioSource Source;

    public float volume = 1;
    public float pitch = 1;

    public bool loop = false;
    public bool playOnStart = false;

}
