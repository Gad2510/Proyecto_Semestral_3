﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruirTrigger : MonoBehaviour
{
    public GameObject poop;

    private void Start()
    {
        poop = GameObject.FindGameObjectWithTag("Poop");
    }

    //Aumentar puntuación
    private void OnTriggerEnter(Collider other)
    {
        //Destruir al ser tocadas por la bola
        if(other.gameObject.CompareTag("Poop"))
        {
            PoopIncrement.score += 1.0f;
            Destroy(gameObject);
            Debug.Log(PoopIncrement.score);
        }
    }
}
