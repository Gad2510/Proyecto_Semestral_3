using UnityEngine;

public class DestruirTrigger : MonoBehaviour
{
    static MeshRenderer mat; //Referencia al material
    static Texture2D texture;//Textura que modifica para ponerla en la final
    int initX, initY,fX=2,fY=2;//Cordenadas en las UV para saber donde esta en el objeto
    PoopIncrement poop;
    FrontCollider frontPL, backPL;
    float timeToReactivate = 30.0f;
    public Transform init, final;
    private void Awake()
    {
        FindMat();

        texture = (Texture2D)mat.material.GetTexture("_Mask");// Creca si ya tiene una textura

        if (texture == null)
        {
            texture = new Texture2D(256, 256);
            int size = 256 * 256;
            Color[] whites = new Color[size];
            texture.SetPixels(whites);
            texture.Apply();

            mat.material.SetTexture("_Mask", texture);
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
        Vector2 pos1=Vector2.zero,pos2=Vector2.zero;
        Ray rayo = new Ray(this.init.position, Vector3.down);
        RaycastHit hit;
        LayerMask mask = LayerMask.NameToLayer("Piso");
        if (Physics.Raycast(rayo, out hit, mask))
        {
            pos1 = hit.textureCoord;
        }
        rayo= new Ray(this.final.position, Vector3.down);
        if (Physics.Raycast(rayo, out hit, mask))
        {
            pos2 = hit.textureCoord;
        }
        GetInfo(pos1, pos2);
    }

    private void GetInfo(Vector2 pos1,Vector2 pos2)
    {
        Vector2Int UVposX=new Vector2Int((int)(pos1.x * texture.width), (int)(pos2.x * texture.width));

        initX = (UVposX.x < UVposX.y) ? UVposX.x : UVposX.y;

        Vector2Int UVposY = new Vector2Int((int)(pos1.y * texture.height), (int)(pos2.y * texture.height));

        initY=(UVposY.x < UVposY.y)? UVposY.x:UVposY.y;
    }

    void FindMat()
    {
        if (mat == null)
        {
            mat = GameObject.Find("Grass").GetComponent<MeshRenderer>();
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
        ChangeColor(Color.black);
    }
    void ChangeColor(Color col)
    {
        if(mat.material.HasProperty("_Mask")){
            texture = (Texture2D)mat.material.GetTexture("_Mask");// Actualiza la textura

            if (texture == null)
            {
                texture = new Texture2D(256, 256);
                int size = 256 * 256;
                Color[] whites = new Color[size];
                texture.SetPixels(whites);
                texture.Apply();

                mat.material.SetTexture("_Mask", texture);
            }

            for (int y = initY; y <=initY+fY; y++)
            {
                for (int x = initX; x <=initX+fX; x++)
                {
                    texture.SetPixel(x, y, col);
                }
            }
            texture.Apply();
            mat.material.SetTexture("_Mask", texture);
        }
        
    }
}
