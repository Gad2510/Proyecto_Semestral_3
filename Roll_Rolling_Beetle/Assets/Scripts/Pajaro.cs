using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pajaro : MonoBehaviour
{
    public float contador = 0.0f; //Contador para cambiar de posicion si no entra el jugador en si trigger
    public float contadorAtaque = 0.0f; //Contador para atacar al jugador si esta dentro se su trigger
    bool atacando = false; //Saber si esta atacando
    void Start()
    {
        
    }

    void Update()
    {
        if (!atacando)
        {
            contador += Time.deltaTime;//Sumar

            if (contador >= 6.0f)
            {
                CambiarPos();
                contador = 0.0f;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            atacando = true; //Atacar

            contadorAtaque += Time.deltaTime;//Sumar

            if (contadorAtaque >= 3.0f)
            {
                contadorAtaque = 0.0f;
                Destroy(other.gameObject); //Condicion de derrota
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            atacando = false;
            contadorAtaque = 0.0f;
        }
    }

    public void CambiarPos()
    {
        int pos;
        pos = Random.Range(-50,50);

        gameObject.transform.position = new Vector3(pos, gameObject.transform.position.y, gameObject.transform.position.z);
    }
}
