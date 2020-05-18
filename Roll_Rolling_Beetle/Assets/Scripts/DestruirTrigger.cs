using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruirTrigger : MonoBehaviour
{
    Material mat; //Referencia al material
    Texture2D texture;//Textura que modifica para ponerla en la final
    int alcance=2;// Alcanse del cambio en pixeles
    public Vector2 COORD;//Cordenadas en las UV para saber donde esta en el objeto
    public PoopIncrement poop;
    public FrontCollider frontPL, backPL;
    float timeToReactivate = 30.0f;
    private void Awake()
    {
        FindMat();

        texture = (Texture2D)mat.GetTexture("_Mask");// Creca si ya tiene una textura

        if (texture == null)
        {
            texture = new Texture2D(64, 64);
            int size = 64 * 64;
            Color[] whites = new Color[size];
            texture.SetPixels(whites);
            texture.Apply();

            mat.SetTexture("_Mask", texture);
        }
    }
    private void Start()
    {
        GameObject check = GameObject.FindGameObjectWithTag("Poop");
        if(check != null)
        {
            poop = check.GetComponent<PoopIncrement>();
        }

        frontPL = GameObject.FindGameObjectWithTag("Player").GetComponent<Personaje>().frontCollider;
        backPL= GameObject.FindGameObjectWithTag("Player").GetComponent<Personaje>().backCollider;
        //BUSCA LA REFERENCIA AL MATERIAL DEL PISO Y SU POSICION EN EL MESH
        Ray rayo = new Ray(this.transform.position, Vector3.down);
        RaycastHit hit;
        LayerMask mask = LayerMask.NameToLayer("Piso");
        if (Physics.Raycast(rayo, out hit, mask))
        {
            COORD = hit.textureCoord;
        }
    }

    void FindMat()
    {
        Ray rayo = new Ray(this.transform.position, Vector3.down);
        RaycastHit hit;
        LayerMask mask = LayerMask.NameToLayer("Piso");
        if (Physics.Raycast(rayo, out hit, mask))
        {
            mat = hit.collider.GetComponent<MeshRenderer>().material;
        }
    }

    private void Update()
    {
        if(frontPL.isPoop || backPL.isPoop)
        {
            poop = GameObject.FindGameObjectWithTag("Poop").GetComponent<PoopIncrement>();
        }
    }

    //Aumentar puntuación
    private void OnTriggerEnter(Collider other)
    {
        //Destruir al ser tocadas por la bola
        if(other.gameObject.CompareTag("Poop"))
        {
            
            if(poop == null)
            {
                poop = other.GetComponent<PoopIncrement>();

            }
            else
            {
                if (poop.AddScore())
                {
                    FindMat();
                    ChangeColor(Color.white);
                    Invoke("EsperaryActivar", timeToReactivate);
                    //Desactivar trigger
                    gameObject.SetActive(false);
                }
            }
        }
    }

    //Funcion para reacativar triggers
    public void EsperaryActivar()
    {
        gameObject.SetActive(true);
        //FindMat();
        ChangeColor(Color.black);
    }
    void ChangeColor(Color col)
    {
        if(mat.HasProperty("_Mask")){
            texture = (Texture2D)mat.GetTexture("_Mask");// Actualiza la textura

            if (texture == null)
            {
                texture = new Texture2D(64, 64);
                int size = 64 * 64;
                Color[] whites = new Color[size];
                texture.SetPixels(whites);
                texture.Apply();

                mat.SetTexture("_Mask", texture);
            }

            int initX = (int)(COORD.x * texture.width);//Calcula la posicion en pixeles
            int initY = (int)(COORD.y * texture.height);

            for (int y = -alcance / 2; y < alcance / 2; y++)
            {
                for (int x = -alcance / 2; x < alcance / 2; x++)
                {
                    texture.SetPixel(initX + x, initY + y, col);
                }
            }
            texture.Apply();
            mat.SetTexture("_Mask", texture);
        }
        
    }
}
