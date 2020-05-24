using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public UnityEvent restartEvent;

    Scene_Manager_BH refToManager;
    private void Start()
    {
        refToManager = Scene_Manager_BH._instance;
        refToManager.LastFrame = Camera.main.activeTexture;
    }

    private void Update()
    {
        if (refToManager.RestartLevel())
        {
            restartEvent.Invoke();
            refToManager.LastLv = "";
        }
    }

    public void ChangeLevel(int index)
    {
        AudioManager.GetInstance().PlayAudio(AUDIO_TYPE.BUTTON);
        RestartScore();
        refToManager.ChangeLevel(index);
    }

    public void RestartScore()
    {
        PoopIncrement.score = 0;
    }
}
