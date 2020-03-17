using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapaAleatorio", menuName = "Datos/MapaAleatorio", order = 1)]

public class MapaAleatorio : ScriptableObject
{
    public GameObject[] terrenos;

    public GameObject ObtenerTerreno()
    {
        return terrenos[Random.Range(0, terrenos.Length)];
    }
}
