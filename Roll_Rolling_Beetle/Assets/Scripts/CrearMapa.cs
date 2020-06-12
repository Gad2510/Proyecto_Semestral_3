using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CrearMapa : MonoBehaviour
{
    public static CrearMapa _instance;
    public static UnityEngine.UI.Slider slider;
    public static SpawnPopoManager poopMnager;

    public MapaAleatorio terrenoD;//Scriptableobject que tiene la referencia a todos los prefebs del juego

    public Transform playerObj;//RReferencia al jugador
    public Personaje player;
    GameObject[] mapaRef;//Referencias a todos los tiles

    public GameObject canvas;


    //Los culling son objetos desactivados de los cuales solo se revisa si estan en pantalla
    Renderer[] mapRender; //Referencias a su objetos culling
    void Awake()
    {
        _instance = this;

        slider = canvas.transform.transform.Find("Slider").GetComponent<UnityEngine.UI.Slider>();
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
        canvas.SetActive(false);
        for (int x = 0; x < 3; x++)
        {
            coord.x = x;//actualiza su posicion en x
            for (int y = 0; y < 3; y++)
            {
                coord.z = y;//actualiza su posicion en z

                if (x == 1 && y == 1)//Revisa si no es el centro del juego
                {
                    g = Instantiate(terrenoD.terrenos[17], coord * 100f, Quaternion.identity, this.transform);// Crea el centro
                }
                else
                {
                    g = Instantiate(terrenoD.ObtenerTerreno(), coord * 100f,Quaternion.identity, this.transform);//Crea un tile aleatorio
                }
                mapaRef[x + (3 * y)] = g;//Guarda la referencia en su posicion

                Transform cullingRef = g.transform.Find("Culling"); //Busca el objeto de referencia culling dentro del objeto
                if (cullingRef != null)
                {
                    mapRender[x + (3 * y)] = cullingRef.GetComponent<Renderer>();//Guarda la referencia
                }
                yield return null;   
            }
        }
        
        poopMnager.Active();
        if(player == null)// Se activa cada que se inicia la escena
        {
            Transform pl = Instantiate(playerObj,this.transform);
            player = pl.GetComponentInChildren<Personaje>();
        }
        canvas.SetActive(true);
        canvas.transform.Find("Home").GetComponentInChildren<apuntar>().searchnewpoop();

        if (Scene_Manager_BH._instance.loadScene)//Verifica que haya estado en la pantalla de inicio
        {
            Loading_manager.Loading = 1f;
        }
        
        InvokeRepeating("OutputVisibleRenderers", 1f, 0.2f); //Empieza el culling que activa y desactiva los objetos cada 0.5 seg
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

    public GameObject FindCloseBonus()
    {
        changeColor[] bonus= mapaRef[4].GetComponentsInChildren<changeColor>();
        
        if (bonus.Length == 0)
        {
            bonus = mapaRef[1].GetComponentsInChildren<changeColor>();
        }

        GameObject close =bonus[0].gameObject;
        float distance= Vector3.Distance(player.transform.position,close.transform.position);

        if(bonus.Length > 1)
        {
            for (int i = 1; i < bonus.Length; i++)
            {
                float d = Vector3.Distance(player.transform.position, bonus[i].transform.position);

                if (d < distance)
                {
                    distance = d;
                    close = bonus[i].gameObject;
                }
            }
        }

        return close;
        
    }
}
