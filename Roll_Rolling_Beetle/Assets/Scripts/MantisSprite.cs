using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MantisSprite : MonoBehaviour
{
    public GameObject mantisRed;
    public GameObject mantisGreen;
    UnityEngine.UI.Slider sliderCaca;

    void Start()
    {
        
        sliderCaca = GameObject.FindGameObjectWithTag("UI").transform.Find("Slider").GetComponent<UnityEngine.UI.Slider>();// Referencia al slider

    }

    // Update is called once per frame
    void Update()
    {
        if (sliderCaca.value >= 0.7f)
        {
            mantisGreen.SetActive(true);//Activar icono rojo
            mantisRed.SetActive(false);
        }
        else if(sliderCaca.value <= 0.69f)
        {
            mantisGreen.SetActive(false);//Activar icono verde
            mantisRed.SetActive(true);
        }
    }
}
