using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Scene_Manager_BH : MonoBehaviour
{
    public static Scene_Manager_BH _instance;
    public PlayerSettings settings;
    [SerializeField]
    LevelLogic[] niveles;


    AudioSource audioSr;
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
        audioSr = GetComponent<AudioSource>();
        settings = Resources.Load<PlayerSettings>("PlayerSettings");
        settings.LoadSettings();
        if (loadScene)
            SceneManager.LoadScene(niveles[index].nombre, LoadSceneMode.Additive);
        else
        {
            audioSr.enabled = false;
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
            

        if (mode != LoadSceneMode.Single && niveles[index].background != null)
        {
            audioSr.clip = niveles[index].background;
            audioSr.Play();
            audioSr.loop = niveles[index].loopBg;
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
    public AudioClip background;
}
