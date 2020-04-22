using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearMapa : MonoBehaviour
{
    public MapaAleatorio terrenoD;

    public Personaje playerRef;

    GameObject[] mapaRef;
    Renderer[] mapRender;
    void Awake()
    {
        mapRender = new Renderer[9];
        mapaRef = new GameObject[9];

        StartLevel();
    }

    private void Update()
    {
        
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
                    g = Instantiate(terrenoD.terrenos[17],this.transform);
                }
                else
                {
                    g = Instantiate(terrenoD.ObtenerTerreno(), this.transform);

                }
                mapaRef[x + (3 * y)] = g;
                g.transform.localPosition = coord * 100f;
                SetCoord(coord, g.transform);
            }
        }

        GameObject[] inScene = GameObject.FindGameObjectsWithTag("Ground");
        
        for(int i=0;i<inScene.Length;i++)
        {
            mapRender[i]=inScene[i].GetComponent<Renderer>();
        }

        InvokeRepeating("OutputVisibleRenderers", 1f, 1f);
    }

    void OutputVisibleRenderers()
    {
        foreach (var renderer in mapRender)
        {
            renderer.transform.parent.gameObject.SetActive(IsVisible(renderer));
        }
    }

    private bool IsVisible(Renderer renderer)// Regresa si el objeto esta en la camara
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }
    public void RestartLevel()
    {
        CancelInvoke();
        playerRef.gameObject.SetActive(true);
        playerRef.Revive();

        for(int i = 0; i < mapaRef.Length; i++)
        {
            Destroy(mapaRef[i]);
        }

        StartLevel();
    }
}
