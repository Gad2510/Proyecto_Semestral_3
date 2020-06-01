using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsToggle : MonoBehaviour
{
    public Toggle sfx;
    public Toggle music;
    void Start()
    {
        sfx.isOn = AudioManager.GetInstance().sfxEnable;
        music.isOn = AudioManager.GetInstance().musicEnable;
    }
}
