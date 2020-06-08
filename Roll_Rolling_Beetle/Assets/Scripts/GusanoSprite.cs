using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GusanoSprite : MonoBehaviour
{
    public GameObject gusanoRed;
    public GameObject gusanoGreen;
    UnityEngine.UI.Slider sliderCaca;

    void Start()
    {
        sliderCaca = GameObject.FindGameObjectWithTag("UI").transform.Find("Slider").GetComponent<UnityEngine.UI.Slider>();// Referencia al slider

    }

    // Update is called once per frame
    void Update()
    {
        if(sliderCaca.value <= 0.4f) //checar si es menor i dejar icono rojo
        {
            gusanoGreen.SetActive(false); //Activar icono rojo
            gusanoRed.SetActive(true);
        }
        if(sliderCaca.value >= 0.4f)
        {
            gusanoGreen.SetActive(true); //Activar icono verde
            gusanoRed.SetActive(false);

        }
    }
}
