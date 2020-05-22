using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class ScoreUpdate : MonoBehaviour
{
    public TextMeshProUGUI score, highscore;

    void Start()
    {
        score.text = "Puntuacion: \n" + Scene_Manager_BH._instance.settings.score;
        highscore.text = "Highscore: \n" + Scene_Manager_BH._instance.settings.highscore;
    }

}
