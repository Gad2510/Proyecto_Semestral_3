using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearMapa : MonoBehaviour
{
    public MapaAleatorio terrenoD;
    
    void Start()
    {
        GameObject g1 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 1
        g1.transform.localPosition = new Vector3(0, 0, 0); //Posicion de terreno 1

        GameObject g2 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 2
        g2.transform.localPosition = new Vector3(0, 0, 100); //Posicion de terreno 2

        GameObject g3 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 3
        g3.transform.localPosition = new Vector3(0, 0, 200); //Posicion de terreno 3

        GameObject g4 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 4
        g4.transform.localPosition = new Vector3(100, 0, 0); //Posicion de terreno 4

        GameObject g5 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 5
        g5.transform.localPosition = new Vector3(100, 0, 100); //Posicion de terreno 5

        GameObject g6 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 6
        g6.transform.localPosition = new Vector3(100, 0, 200); //Posicion de terreno 6

        GameObject g7 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 7
        g7.transform.localPosition = new Vector3(200, 0, 0); //Posicion de terreno 7

        GameObject g8 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 8
        g8.transform.localPosition = new Vector3(200, 0, 100); //Posicion de terreno 8

        GameObject g9 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 9
        g9.transform.localPosition = new Vector3(200, 0, 200); //Posicion de terreno 9
    }
}
