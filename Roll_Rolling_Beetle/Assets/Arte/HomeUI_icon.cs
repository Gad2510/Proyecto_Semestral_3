using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_icon : MonoBehaviour
{
    Slider CacaPorcentage;
    public static bool escalando = false;

    void Start()
    {
        CacaPorcentage = GameObject.FindGameObjectWithTag("UI").transform.Find("Slider").GetComponent<Slider>();
    }

    void Update()
    {
        //Cuando el slider llegue al maximo, escalar el icono
        if(CacaPorcentage.value >= 0.9f && !escalando)
        {
            escalando = true;
            LeanTween.scale(gameObject, new Vector3(0.5f, 0.5f, 0.5f), 0.3f).setLoopPingPong();
        }

        //Si no se esta escalando, dejar su escala en 1
        if(!escalando)
        {
            LeanTween.scale(gameObject, new Vector3(1.0f, 1.0f, 1.0f), 1.0f);
        }
    }
}
