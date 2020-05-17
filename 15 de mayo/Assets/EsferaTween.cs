using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EsferaTween : MonoBehaviour
{
    public Vector3 destino;
    public float tiempoEnCompletar;
    public GameObject cubo;
    public GameObject esfera2;
    void Start()
    {
        LeanTween.move(gameObject, destino, tiempoEnCompletar).setLoopClamp();
        LeanTween.rotate(gameObject, new Vector3(0f,180f,0f), tiempoEnCompletar);
        LeanTween.color(gameObject, Color.red, tiempoEnCompletar);
        LeanTween.scale(gameObject, new Vector3(2f,2f,2f), tiempoEnCompletar);

        LeanTween.color(cubo, Color.red, tiempoEnCompletar).setOnComplete(CambiarColorAzul);

        LeanTween.value(gameObject, ActualizarFloat, 0f, 4.0f, 6.0f);

    }
    void CambiarColorAzul()
    {
        LeanTween.color(esfera2, Color.blue, .5f);
    }
    void ActualizarFloat(float _valor)
    {
        print(_valor);
    }
}
