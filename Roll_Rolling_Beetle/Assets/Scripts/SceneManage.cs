using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public UnityEvent restartEvent;
    public bool restart;

    public bool Restart
    {
        set { if (value) { PoopIncrement.score = 0; } restart = value; }
    }

    Scene_Manager_BH refToManager;
    private void Start()
    {
        refToManager = Scene_Manager_BH._instance;
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
        refToManager.ChangeLevel(restart,index);
    }

    public void RestartScore()
    {
        PoopIncrement.score = 0;
    }
}
