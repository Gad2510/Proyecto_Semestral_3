using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearMapa : MonoBehaviour
{
    public MapaAleatorio terrenoD;
    
    void Start()
    {
        GameObject g;
        Vector3 coord=Vector4.zero;

        for(int x = 0; x < 3; x++)
        {
            coord.x = x;
            for(int y = 0; y < 3; y++)
            {
                coord.z = y;

                if(x==1 && y == 1)
                {
                    g = Instantiate(terrenoD.terrenos[17], null); //Crear terreno 5
                }
                else
                {
                    g = Instantiate(terrenoD.ObtenerTerreno(), null);
                    
                }
                g.transform.localPosition = coord * 100f;
                SetCoord(coord, g.transform);
            }
        }
        /*g = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 1
        g.transform.localPosition = new Vector3(0, 0, 0); //Posicion de terreno 1
        
        g = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 2
        g.transform.localPosition = new Vector3(0, 0, 100); //Posicion de terreno 2

        g = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 3
        g.transform.localPosition = new Vector3(0, 0, 200); //Posicion de terreno 3

        g = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 4
        g.transform.localPosition = new Vector3(100, 0, 0); //Posicion de terreno 4

        g = Instantiate(terrenoD.terrenos[17], null); //Crear terreno 5
        g.transform.localPosition = new Vector3(100, 0, 100); //Posicion de terreno 5

        g = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 6
        g.transform.localPosition = new Vector3(100, 0, 200); //Posicion de terreno 6

        g = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 7
        g.transform.localPosition = new Vector3(200, 0, 0); //Posicion de terreno 7

        g = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 8
        g.transform.localPosition = new Vector3(200, 0, 100); //Posicion de terreno 8

        g = Instantiate(terrenoD.ObtenerTerreno(), null); //Crear terreno 9
        g.transform.localPosition = new Vector3(200, 0, 200); //Posicion de terreno 9*/
    }

    private void SetCoord(Vector3 coord, Transform g)
    {
        Vector4 loc = new Vector4(2-coord.x, 2-coord.z, 3f, 0f);
        Material r;
        bool isGround = false;
        for(int i=0;i<g.childCount && !isGround; i++)
        {
            Transform child = g.GetChild(i);
            isGround = child.CompareTag("Ground");
            if(isGround){
                r = child.GetComponent<MeshRenderer>().material;
                r.SetVector("_Cord", loc);
            }
        }
    }
}
