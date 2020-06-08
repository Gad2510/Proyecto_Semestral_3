using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_icon : MonoBehaviour
{
    Slider CacaPorcentage;
    public static bool escalando, startOver;
    Transform parent;
    Vector3 puntoFinal;

    int leanID;

    void Start()
    {
        CacaPorcentage = GameObject.FindGameObjectWithTag("UI").transform.Find("Slider").GetComponent<Slider>();
        leanID = LeanTween.scale(gameObject, new Vector3(0.5f, 0.5f, 0.5f), 0.5f).setLoopPingPong().id;
        parent = GetComponentInParent<Transform>();
        puntoFinal = new Vector3(-320.0f, 680.0f, 0.0f);
        escalando = true;
        startOver = true;
    }

    void Update()
    {
        //Cuando el slider llegue al maximo, escalar el icono
        if (CacaPorcentage.value >= 1.0f && !escalando)
        {
            LeanTween.resume(leanID);
            escalando = true;
        }

        //Si no se esta escalando, dejar su escala en 1
        if (escalando && startOver)
        {
            escalando = false;
            startOver = false;
            LeanTween.pause(leanID);
            transform.localScale = Vector3.one;
        }
    }
}
