using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearMapa : MonoBehaviour
{
    public MapaAleatorio terrenoD;
    public float[][] posiciones;
    
    void Start()
    {
        GameObject g1 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 1
        g1.transform.position = new Vector3(posiciones[0][0], posiciones[0][1], 0); //Posicion de terreno 1

        GameObject g2 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 2
        g2.transform.position = new Vector3(posiciones[1][0], posiciones[1][1], 0); //Posicion de terreno 2

        GameObject g3 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 3
        g3.transform.position = new Vector3(posiciones[2][0], posiciones[2][1], 0); //Posicion de terreno 3

        GameObject g4 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 4
        g4.transform.position = new Vector3(posiciones[3][0], posiciones[3][1], 0); //Posicion de terreno 4

        GameObject g5 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 5
        g5.transform.position = new Vector3(posiciones[4][0], posiciones[4][1], 0); //Posicion de terreno 5

        GameObject g6 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 6
        g6.transform.position = new Vector3(posiciones[5][0], posiciones[5][1], 0); //Posicion de terreno 6

        GameObject g7 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 7
        g7.transform.position = new Vector3(posiciones[6][0], posiciones[6][1], 0); //Posicion de terreno 7

        GameObject g8 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 8
        g8.transform.position = new Vector3(posiciones[7][0], posiciones[7][1], 0); //Posicion de terreno 8

        GameObject g9 = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 9
        g9.transform.position = new Vector3(posiciones[8][0], posiciones[8][1], 0); //Posicion de terreno 9
    }

    
    void Update()
    {
        
    }
}
