using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/PlSettings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    public float score;
    public float highscore;
    public bool newScore = false;

    public void LoadSettings()
    {
        highscore = 0f;
        newScore = false;
        if (PlayerPrefs.HasKey("highscore"))
        {
            highscore = PlayerPrefs.GetFloat("highscore");
        }
    }
    
    void SaveScore()
    {
        PlayerPrefs.SetFloat("highscore", highscore);
        PlayerPrefs.Save();
    }

    public void SaveSttings()
    {

    }
    public void UpdateScore(float current)
    {
        score = current;
        newScore = score > highscore;
        if (newScore)
        {
            highscore = score;
            SaveScore();
        }
        
    }


}
