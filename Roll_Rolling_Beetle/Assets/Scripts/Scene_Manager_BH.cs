using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Scene_Manager_BH : MonoBehaviour
{
    public static Scene_Manager_BH _instance;
    public PlayerSettings settings;
    public LevelLogic currentScene;
    [SerializeField]
    LevelLogic[] niveles= null;

    AsyncOperation currentLoad;

    public bool loadScene,restart;
    public int index = 0;
    string lastLv;

    LoadSceneMode mode = LoadSceneMode.Additive;

    public RenderTexture LastFrame { get; set; }

    public AsyncOperation CurrentLoad
    {
        get { return currentLoad; }
    }

    public string LastLv
    {
        set { lastLv = value; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        settings = Resources.Load<PlayerSettings>("PlayerSettings");
        settings.LoadSettings();
        if (loadScene)
        {
            SceneManager.LoadScene(niveles[index].nombre, LoadSceneMode.Additive);
            currentScene = niveles[index];
        }
        else
        {
            mode = LoadSceneMode.Single;

            foreach (LevelLogic lv in niveles)
            {
                if (lv.nombre == SceneManager.GetActiveScene().name)
                {
                    currentScene = lv;
                }
            }
        }
    }

    public void UnloadLoding()
    {
        SceneManager.UnloadSceneAsync("LoadingScene");
    }

    public void ChangeLevel(int level)
    {
        if (AudioManager.GetInstance().isPaused)
        {
            AudioManager.GetInstance().music.Play();
        }
        if (mode!=LoadSceneMode.Single && !niveles[level].aditiveToLast)
        {
            SceneManager.UnloadSceneAsync(niveles[index].nombre);
            if (niveles[index].aditiveToLast && !restart)
            {
                SceneManager.UnloadSceneAsync(lastLv);
            }
        }
        else
        {
            lastLv = niveles[index].nombre;
        }
        index = level;

        if ((mode == LoadSceneMode.Single || !restart) && !niveles[index].loading)
        {
            currentLoad=SceneManager.LoadSceneAsync(niveles[level].nombre, mode);
        }
        else if (niveles[level].loading)
        {
            SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
        }

        //Musica
        if (level == 3)
        {
            AudioManager.GetInstance().PlayAudio(AUDIO_TYPE.GAME_OVER);
            AudioManager.GetInstance().PauseBackground();
        }
        if (level == 0)
        {
            AudioManager.GetInstance().PlayBackground(BACKGROUND_TYPE.LVL1);
        }
    }

    public void loadLevelInLine()
    {
        currentLoad = SceneManager.LoadSceneAsync(niveles[index].nombre, mode);
    }

    public bool RestartLevel()
    {
        return niveles[index].nombre == lastLv && restart;
    }

    
}
[System.Serializable]
public class LevelLogic
{
    public string nombre;
    public bool aditiveToLast, loading;
    public BACKGROUND_TYPE InitMusic;
}
