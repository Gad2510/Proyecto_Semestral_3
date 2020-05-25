using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;
    public GameObject PauseUI;
    // Update is called once per frame
    void Update()
    {

    }
    public void Pause()
    {
        if (!GameIsPaused)
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
            AudioManager.GetInstance().music.pitch *= .5f;
            AudioManager.GetInstance().music.volume *= .5f;
        }
    }
    public void Resume()
    {
        if (GameIsPaused)
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;
            AudioManager.GetInstance().music.pitch = 1;
            AudioManager.GetInstance().music.volume = 1;
        }
    }
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        AudioManager.GetInstance().music.pitch = 1;
        AudioManager.GetInstance().music.volume = 1;
        SceneManager.LoadScene(1);
    }
}
