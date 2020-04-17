using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearMapa : MonoBehaviour
{
    public MapaAleatorio terrenoD;
    public Object player;

    GameObject[] mapaRef;
    void Awake()
    {
        mapaRef = new GameObject[9];
        StartLevel();
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

    private void StartLevel()
    {
        GameObject g;
        Vector3 coord = Vector4.zero;

        for (int x = 0; x < 3; x++)
        {
            coord.x = x;
            for (int y = 0; y < 3; y++)
            {
                coord.z = y;

                if (x == 1 && y == 1)
                {
                    g = Instantiate(terrenoD.terrenos[17], null);
                }
                else
                {
                    g = Instantiate(terrenoD.ObtenerTerreno(), null);

                }
                mapaRef[x + (3 * y)] = g;
                g.transform.localPosition = coord * 100f;
                SetCoord(coord, g.transform);
            }
        }
    }
    public void RestartLevel()
    {
        for(int i = 0; i < mapaRef.Length; i++)
        {
            Destroy(mapaRef[i]);
        }

        StartLevel();

        Instantiate(player);
    }
}
