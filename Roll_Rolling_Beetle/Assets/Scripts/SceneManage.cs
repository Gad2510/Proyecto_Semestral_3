using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public static SceneManage _instance;

    public PlayerSettings settings;

    public UnityEvent myEvent;

    string loadScene;

    bool restart = false;

    // Start is called before the first frame update
    void Start()
    {
        if(_instance!= this)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
        settings.LoadSettings();
    }

    public void ChangeLevel(string index, LoadSceneMode mode=LoadSceneMode.Single)
    {
        loadScene = index;
        SceneManager.LoadScene(index,mode);
    }
    public void UnloadScene()
    {
        Debug.Log(loadScene);
        SceneManager.UnloadSceneAsync("GameOver");
        myEvent.Invoke();
    }
    public void ChangeLevel(string index)
    {
        SceneManager.LoadScene(index);
    }


}
