using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;

    public AudioSource sfx;
    public AudioSource music;
    public AudioByType[] audios;
    public BackgrounByType[] backgroundSounds;
    public AudioSource[] g_audio;

    public bool sfxEnable = true;
    public bool musicEnable = true;
    public bool isPaused = false;

    public void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            PlayBackground(Scene_Manager_BH._instance.currentScene.InitMusic);
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static AudioManager GetInstance()
    {
        return _instance;
    }
    public void PlayAudio(AUDIO_TYPE _t) //2d
    {
        if (sfxEnable)
        {
            for (int i = 0; i < audios.Length; i++)
            {
                if (audios[i].type == _t)
                {
                    sfx.PlayOneShot(audios[i].clip);
                    break;
                }
            }
        }
    }
    public void PlayBackground(BACKGROUND_TYPE _t) //2d
    {
        if (musicEnable)
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
    }
    public void PauseBackground()
    {
        music.Pause();
        isPaused = true;
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
    public void SfxEnableConfiguration()
    {
        sfxEnable = !sfxEnable;
    }
    public void MusicEnableConfiguration()
    {
        musicEnable = !musicEnable;
    }
}

public enum AUDIO_TYPE
{
    BUTTON, APLASTADO_CACA, MUERTE_MANTIS, MUERTE_GUSANO, MUERTE_JUGADOR, GAME_OVER
}
public enum BACKGROUND_TYPE
{
    LVL1, LVL2, LVL3, LVL4, INTER
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
