using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScore : MonoBehaviour
{
    //Prefab TriggerPoint
    public BoxCollider puntos;
    //Padre que contiene todos los triggers
    public GameObject padre;
    public int cantidad;
    public float distancia;

    void Awake()
    {
        //Llenar de triggers el plano donde esta parado el jugador 
        //Colocar el GeneradorTriggers en el centro de cada plano
        for (int i = 0; i <= cantidad; i++)
        {
            for (int j = 0; j <= cantidad; j++)
            {
                BoxCollider t = Instantiate(puntos, padre.transform);
                t.transform.localPosition = new Vector3(i*distancia, 0.5f, j*distancia);
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
            padre.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Desactivar cuando el jugador sale de un plano
        if (other.gameObject.CompareTag("Player"))
        {
            padre.SetActive(false);
        }
    }

}
