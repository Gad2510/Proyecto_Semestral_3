using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodarCaca : MonoBehaviour //Hace girar la caca, el script esta dentro del modelo y usa de referencia el objeto de centro como pivote de rotacion solo sirve tras ser lanzada la caca
{
    Personaje escarabajo;
    float CacaRotVel;
    public GameObject centro;


    void Start()
    {
        escarabajo = GameObject.FindGameObjectWithTag("Player").GetComponent<Personaje>();
        CacaRotVel = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(escarabajo.poopshooted)
        {
            //Pero por favor no comenten mis cosas, pero oigame no me ahorco
           transform.RotateAround(centro.transform.position, centro.transform.right, CacaRotVel );
        }
    }
}
