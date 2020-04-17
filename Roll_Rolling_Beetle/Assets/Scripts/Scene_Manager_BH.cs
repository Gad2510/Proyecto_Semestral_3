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

    SceneManage[] managers;
    public bool restart;
    int index = 0;



    // Start is called before the first frame update
    void Start()
    {
        index = 0;
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
        
        if (!niveles[index].aditiveToLast)
        {
            SceneManager.UnloadSceneAsync(niveles[index].nombre);
        }
        index = level;
        SceneManager.LoadScene(niveles[index].nombre, LoadSceneMode.Additive);

        managers = GameObject.FindObjectsOfType<SceneManage>();

        for(int i = 0; i < managers.Length; i++)
        {
            Debug.Log(managers[i].name);
        }

    }

    public bool RestartLevel(string levelName)
    {
        return niveles[index].nombre == levelName && restart;
    }
}
[System.Serializable]
public class LevelLogic
{
    public string nombre;
    public bool aditiveToLast;
}
