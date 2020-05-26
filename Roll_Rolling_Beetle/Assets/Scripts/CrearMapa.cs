using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearMapa : MonoBehaviour
{
    public static SpawnPopoManager poopMnager;

    public MapaAleatorio terrenoD;//Scriptableobject que tiene la referencia a todos los prefebs del juego

    public Transform playerObj;//RReferencia al jugador
    Personaje player;
    GameObject[] mapaRef;//Referencias a todos los tiles
    
    //Los culling son objetos desactivados de los cuales solo se revisa si estan en pantalla
    Renderer[] mapRender; //Referencias a su objetos culling
    void Awake()
    {
        terrenoD = Resources.Load<MapaAleatorio>("MapaAleatorio"); //Carga automaticamente de la carpeta resources el scriptable object
        mapRender = new Renderer[9]; //Inicializamos variables
        mapaRef = new GameObject[9];
        poopMnager = GetComponent<SpawnPopoManager>();
        StartCoroutine(StartLevel());//Empezar el juego
    }

    private IEnumerator StartLevel()
    {
        GameObject g; //Crea un gameobject para tener donde depositar las referencias de los prefabs
        Vector3 coord = Vector4.zero;// Empieza la cordenada de conde se crea

        for (int x = 0; x < 3; x++)
        {
            coord.x = x;//actualiza su posicion en x
            for (int y = 0; y < 3; y++)
            {
                coord.z = y;//actualiza su posicion en z

                if (x == 1 && y == 1)//Revisa si no es el centro del juego
                {
                    g = Instantiate(terrenoD.terrenos[17],this.transform);// Crea el centro
                }
                else
                {
                    g = Instantiate(terrenoD.ObtenerTerreno(), this.transform);//Crea un tile aleatorio

                }
                mapaRef[x + (3 * y)] = g;//Guarda la referencia en su posicion
                g.transform.localPosition = coord * 100f; //La coloca en su posicion

                Transform cullingRef = g.transform.Find("Culling"); //Busca el objeto de referencia culling dentro del objeto
                if (cullingRef != null)
                {
                    mapRender[x + (3 * y)] = cullingRef.GetComponent<Renderer>();//Guarda la referencia
                }
                yield return null;   
            }
        }
        GameObject.FindGameObjectWithTag("UI").transform.Find("Home").GetComponentInChildren<apuntar>().searchnewpoop();
        poopMnager.Active();
        if(player == null)// Se activa cada que se inicia la escena
        {
            Transform pl = Instantiate(playerObj);
            player = pl.GetComponentInChildren<Personaje>();
        }

        Loading_manager.Loading = 1f;
        InvokeRepeating("OutputVisibleRenderers", 1f, 0.5f); //Empieza el culling que activa y desactiva los objetos cada 0.5 seg
    }

    void OutputVisibleRenderers()
    {
        foreach (var renderer in mapRender) 
        {
            renderer.transform.parent.gameObject.SetActive(IsVisible(renderer));//Revisa si es visible en camara
        }
    }

    private bool IsVisible(Renderer renderer)// Regresa si el objeto esta en la camara
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main); 

        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds); 
    }
    public void RestartLevel() //Reinicai el nivel si es necesario
    {
        CancelInvoke();
        GameObject[] popos = GameObject.FindGameObjectsWithTag("Poop");

        for(int i = 1; i < popos.Length; i++)
        {
            Destroy(popos[i]);
        }
        if (player != null)
        {
            player.gameObject.SetActive(true);
            player.Revive();
        }
        

        for(int i = 0; i < mapaRef.Length; i++)
        {
            Destroy(mapaRef[i]);
        }

        Invoke("StartLevel",0.1f);
    }

    public void BeetleShoot()
    {
        if(player!=null)
            player.PrepareToShot();
    }
}
