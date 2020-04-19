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
        set { restart = value; }
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
        refToManager.ChangeLevel(restart,index);
    }

}
