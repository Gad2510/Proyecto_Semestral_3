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
    SceneManage[] managers;
    public bool restart;
    int index = 0;
    string lastLv;

    public string LastLv
    {
        set { lastLv = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        lastLv = "";
        audioSr = GetComponent<AudioSource>();
        SceneManager.LoadScene(niveles[index].nombre, LoadSceneMode.Additive);
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        settings.LoadSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeLevel(bool restart, int level)
    {
        this.restart = restart;
        if (!niveles[level].aditiveToLast)
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

        if(!restart)
            SceneManager.LoadScene(niveles[index].nombre, LoadSceneMode.Additive);

        if (niveles[index].background != null)
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
