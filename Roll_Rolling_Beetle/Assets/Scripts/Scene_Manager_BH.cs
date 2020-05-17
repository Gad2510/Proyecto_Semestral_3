using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum SCENE_STATE{ MENU, OPTIONS, JUEGO, GAMEOVER}

public class Scene_Manager_BH : MonoBehaviour
{
    public static Scene_Manager_BH _instance;
    public PlayerSettings settings;
    public SCENE_STATE currentScene;
    [SerializeField]
    LevelLogic[] niveles;

    public bool loadScene,restart;
    public int index = 0;
    string lastLv;

    LoadSceneMode mode = LoadSceneMode.Additive;
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
    }

    // Start is called before the first frame update
    void Start()
    {
        lastLv = "";
        settings = Resources.Load<PlayerSettings>("PlayerSettings");
        settings.LoadSettings();
        if (loadScene)
            SceneManager.LoadScene(niveles[index].nombre, LoadSceneMode.Additive);
        else
        {
            mode = LoadSceneMode.Single;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeLevel(bool restart, int level)
    {
        this.restart = restart;
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

        if (mode == LoadSceneMode.Single || !restart)
        {
            SceneManager.LoadScene(niveles[index].nombre, mode);
        }
        if(level == 3)
        {
            AudioManager.GetInstance().PlayBackground(BACKGROUND_TYPE.GAME_OVER);
        }
        if (level == 0)
        {
            AudioManager.GetInstance().PlayBackground(BACKGROUND_TYPE.LVL1);
        }
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
    public bool aditiveToLast,loopBg;
}
