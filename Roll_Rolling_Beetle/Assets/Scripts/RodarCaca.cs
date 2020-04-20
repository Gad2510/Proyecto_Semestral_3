using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodarCaca : MonoBehaviour
{
    GameObject escarabajo;
    public float CacaRotVel;
    public GameObject centro;


    void Start()
    {
        escarabajo = GameObject.FindGameObjectWithTag("Player");
        CacaRotVel = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(escarabajo.GetComponent<Personaje>().poopshooted)
        {
           transform.RotateAround(centro.transform.position, centro.transform.right, CacaRotVel /** Time.deltaTime*/);
        }
    }
}
