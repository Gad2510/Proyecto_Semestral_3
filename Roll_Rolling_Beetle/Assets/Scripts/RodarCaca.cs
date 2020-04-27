using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodarCaca : MonoBehaviour
{
    Personaje escarabajo;
    public float CacaRotVel;
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
           transform.RotateAround(centro.transform.position, centro.transform.right, CacaRotVel /** Time.deltaTime*/);
        }
    }
}
