using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdate : MonoBehaviour
{
    public Text score;

    // Start is called before the first frame update
    void Start()
    {
        score.text = "Puntuacion: \n" + Scene_Manager_BH._instance.settings.score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
