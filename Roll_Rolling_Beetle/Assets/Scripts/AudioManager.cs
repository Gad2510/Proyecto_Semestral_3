﻿using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;

    public AudioSource sfx;
    public AudioSource music;
    public AudioByType[] audios;
    public BackgrounByType[] backgroundSounds;
    public AudioSource[] g_audio;

    public bool sfxEnable;
    public bool musicEnable;

    public void Start()
    {
        sfx.enabled = true;
    }

    public static AudioManager GetInstance()
    {
        return _instance;
    }

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void PlayAudio(AUDIO_TYPE _t) //2d
    {
        for(int i = 0; i < audios.Length; i++)
        {
            if(audios[i].type == _t)
            {
                sfx.PlayOneShot(audios[i].clip);
                break;
            }
        }
    }
    public void PlayBackground(BACKGROUND_TYPE _t) //2d
    {
        for (int i = 0; i < backgroundSounds.Length; i++)
        {
            if (backgroundSounds[i].type == _t)
            {
                music.clip = backgroundSounds[i].clip;
                music.Play();
                break;
            }
        }
    }
    public void PlayAudio(AUDIO_TYPE _t, Vector3 pos) //3d
    {
        for (int i = 0; i < audios.Length; i++)
        {
            if (audios[i].type == _t)
            {
                g_audio[i].PlayOneShot(audios[i].clip);
                g_audio[i].transform.position = pos;
                break;
            }
        }
    }
}

public enum AUDIO_TYPE
{
    BUTTON,APLASTADO_CACA,MUERTE_MANTIS,MUERTE_GUSANO,MUERTE_JUGADOR
}
public enum BACKGROUND_TYPE
{
    GAME_OVER, LVL1, LVL2, LVL3, LVL4, INTER
}
[Serializable]
public class AudioByType
{
    public AudioClip clip;
    public AUDIO_TYPE type;
}

[Serializable]
public class BackgrounByType
{
    public AudioClip clip;
    public BACKGROUND_TYPE type;
}
