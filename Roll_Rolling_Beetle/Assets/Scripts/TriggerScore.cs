using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScore : MonoBehaviour
{
    //Prefab TriggerPoint
    public BoxCollider puntos;
    //Padre que contiene todos los triggers
    public GameObject padre;

    void Awake()
    {
        //Llenar de triggers el plano donde esta parado el jugador 
        //Colocar el GeneradorTriggers en el centro de cada plano
        for (int i = 0; i <= 100; i++)
        {
            for (int j = 0; j <= 100; j++)
            {
                BoxCollider t = Instantiate(puntos, padre.transform);
                t.transform.localPosition = new Vector3(i, 0.5f, j);
            }
        }
        //Desactivar el padre que contiene todos los trigger del plano
        padre.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        //Activar cuando el jugador entra en un plano
        if(other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Entro el jugador");
            padre.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Desactivar cuando el jugador sale de un plano
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Se fue el jugador");
            padre.SetActive(false);
        }
    }

}
