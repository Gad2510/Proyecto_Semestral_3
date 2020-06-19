using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodarCaca : MonoBehaviour
{
    //Hace girar la caca, el script esta dentro del modelo y usa de referencia el objeto de centro como pivote de rotacion solo sirve tras ser lanzada la caca
    Personaje escarabajo;
    float CacaRotVel;
    public GameObject centro;
    public ParticleSystem airEffect;

    void Start()
    {
        escarabajo = GameObject.FindGameObjectWithTag("Player").GetComponent<Personaje>();
        airEffect = this.GetComponentInChildren<ParticleSystem>();
        CacaRotVel = 3.0f;
    }

    void Update()
    {

        if(escarabajo.poopshooted)
        {
            airEffect.transform.LookAt(escarabajo.transform);
            transform.RotateAround(centro.transform.position, escarabajo.transform.right, CacaRotVel);
        }
    }
}
