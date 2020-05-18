using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { WALKING, THROWING, DEAD}

public class Personaje : MonoBehaviour
{
    Animator animBeetle; //Referencia al animador
    public GameObject[] spawners; //Lista de puntos a los que puede spawnear el personaje
    public Rigidbody poopRigid; //Referencia al rigidbody de la popo
    public float maxMovementSpeed;// Velocidad sin caca
    public float maxRotationSpeed;// Rotacion sin caca
    public float timeSinceShot; //Contador para evitar que el jugador se mueve despues de disparar
    public bool isAlive;// Bool para saber si esta vivo el jugador
    public bool isPoopInGame;
    public bool poopshooted;//Bool para la rotacion de la caca cuando pierde su referencia con la misma
    public PlayerState state;//Estado del juegador
    public GameObject poopPrefab; //Prefab de la caca
    public FrontCollider frontCollider, backCollider;//Referencia a sus colliders para cuando tiene que agarrarla
    public float CacaRotVel; //Velocidad de rotacion
    float dir=1f; //Float para saber la direccion en la que tiene que moverse el jugador, cambia al agarrar la popo por detras

    bool lvl2music;
    bool lvl3music;

    //Flecha de direccion de popo
    public apuntar Arrow;

    FollowPlayer camPos;

    Vector3 fingerDir;

    float startTouch, endTouch, timeTouched;

    public Vector2 move;

    [SerializeField]
    float movementSpeed, rotationSpeed;
    

    public bool canHold, isMoving;
    public float t;
    public bool IsMoving
    {
        get { return isMoving; }
    }

    public bool CanHold
    {
        get { return canHold; }
    }

    void Start()
    {
        
        camPos = Camera.main.GetComponent<FollowPlayer>();
        state = PlayerState.WALKING;
        spawners = GameObject.FindGameObjectsWithTag("SpawnerPlayer");
        animBeetle = GetComponent<Animator>();
        canHold = false;
        isMoving = false;
        poopshooted = false;
        isAlive = true;
        maxMovementSpeed = 10;
        maxRotationSpeed = 50;
        startTouch = 0f;
        endTouch = 0f;
        timeTouched = 0f;
        SpawnPosition();
        CacaRotVel = 30.0f;
        Arrow.gameObject.SetActive(false);
    }
    void Update()
    {
        TapCalculation();

        if (isAlive && state == PlayerState.WALKING)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            //float x = fingerDir.x;
            //float y = fingerDir.y;
            y *= dir;//Cambio de dirreccion cuando se agarra por atras
            
            if (canHold)
            {
                movementSpeed = maxMovementSpeed;
                rotationSpeed = maxRotationSpeed;
            }
            else
            {
                movementSpeed = 5;
                rotationSpeed = 25;
                RotationPoop(x,y);
            }

            isPoopInGame = GameObject.FindGameObjectsWithTag("Poop").Length > 0;

            transform.Rotate(0, x * Time.deltaTime * rotationSpeed, 0);
            transform.Translate(0, 0,  y* Time.deltaTime * movementSpeed);
            float tringulate = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));//Para saber si el jugador se esta moviendo
            if (poopRigid!= null)
            {
                isMoving = (!poopRigid.IsSleeping()) || tringulate > 0.1f;
            }
            animBeetle.SetBool("isWalking", tringulate > 0.1f);


            if (Input.GetKeyDown(KeyCode.Space) && !canHold)
            {
                animBeetle.SetTrigger("shoot");
                poopshooted = true;
                poopRigid.isKinematic = false;
                state = PlayerState.THROWING;
            }
        }
        else if (state == PlayerState.THROWING)
        {
            timeSinceShot += Time.deltaTime;
            if(timeSinceShot >= 1.5f)
            {
                state = PlayerState.WALKING;
                timeSinceShot = 0;
                poopshooted = false;
            }
        }

        //////////////// Musica ////////////////
        if (PoopIncrement.score > 50 && !lvl2music)
        {
            lvl2music = true;
            AudioManager.GetInstance().PlayBackground(BACKGROUND_TYPE.LVL2);
        }
        else if (PoopIncrement.score > 100 && !lvl3music)
        {
            lvl3music = true;
            AudioManager.GetInstance().PlayBackground(BACKGROUND_TYPE.LVL3);
        }
        ////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.H))
        {
            KillPlayer();
        }
    }

    public void KillPlayer()//funcion para borrar
    {
        animBeetle.SetTrigger("dead");
        isAlive = false;
    }
    private void TapCalculation()
    {
        if (state == PlayerState.THROWING)
            return;

        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                startTouch = Time.realtimeSinceStartup;
            }
            else if (Input.touches[0].phase == TouchPhase.Moved)
            {
                fingerDir = Input.touches[0].deltaPosition;
                fingerDir.x = fingerDir.x / (Screen.width/50);
                fingerDir.y = fingerDir.y / (Screen.height/50);
                
            }
            else if(Input.touches[0].phase==TouchPhase.Ended)
            {
                fingerDir = Vector3.zero;
                endTouch = Time.realtimeSinceStartup;
                timeTouched = endTouch-startTouch;

                if (!canHold && timeTouched < 0.3f)
                {
                    animBeetle.SetTrigger("shoot");
                    poopshooted = true;
                    poopRigid.isKinematic = false;
                    state = PlayerState.THROWING;
                }
            }

            

        }
    }
    public void ShootPoop()
    {
        Arrow.gameObject.SetActive(true); //Activa la flecha al apuntar
        animBeetle.SetBool("holding", true);
        if(poopRigid != null)
        {
            poopRigid.transform.parent = null;
            poopRigid.velocity = transform.forward * 10 * dir;
        }
        poopRigid = null;
        canHold = true;
        
    }
    public void FrontColliderAction()
    {
        Arrow.gameObject.SetActive(false); //Si agarra la popo desactiva la flecha
        if (frontCollider.isPoop )// Un if para saber con cual collider fue agarrado
        {
            ChooseCollider(frontCollider);
            SetDirection(frontCollider.gameObject);
        }
        else if (backCollider.isPoop)
        {
            ChooseCollider(backCollider);
            SetDirection(backCollider.gameObject);
        }
    }

    private void SetDirection(GameObject collider)
    {
        if (collider == backCollider.gameObject)
        {
            animBeetle.SetLayerWeight(0, 0f);
            animBeetle.SetLayerWeight(1, 1f);
            camPos.InverseRot = true;
            dir = -1f;
        }
        else
        {
            animBeetle.SetLayerWeight(0, 1f);
            animBeetle.SetLayerWeight(1, 0f);
            camPos.InverseRot = false;
            dir = 1f;
        }
    }

    void ChooseCollider(FrontCollider currentColl, bool animState=false)
    {
        poopRigid = currentColl.collide.attachedRigidbody;
        animBeetle.SetBool("holding", false);
        poopRigid.transform.SetParent(transform);
        poopRigid.isKinematic = true;
        canHold = false;
        poopRigid.transform.position = currentColl.transform.position; //Pone en posicion el objeto
    }
    void OnCollisionEnter(Collision collision)
    {
        if (isAlive && collision.gameObject.tag == "Enemigo")
        {
            animBeetle.SetTrigger("dead");
            isAlive = false;
            AudioManager.GetInstance().PlayAudio(AUDIO_TYPE.MUERTE_JUGADOR);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TriggerPoop"))
        {
            if (canHold)
            {
                FrontColliderAction();
            }
        }

        if (other.gameObject.CompareTag("Baba"))
        {
            maxMovementSpeed = maxMovementSpeed / 2;
            movementSpeed = movementSpeed / 2;
        }
        if (other.gameObject.CompareTag("Bird"))
        {
            ChangeScene();
            isAlive = false;
            this.gameObject.SetActive(false);
        }
       
    }

    public void CreateNewPoop(GameObject coll)
    {
        GameObject poop = Instantiate(poopPrefab, coll.transform.position, Quaternion.identity, this.transform) ;
        SetDirection(coll);
        poopRigid = poop.GetComponent<Rigidbody>();
        poopRigid.isKinematic = true;
        animBeetle.SetBool("holding", false);
        canHold = false;
        Arrow.searchnewpoop();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Baba"))
        {
            maxMovementSpeed = maxMovementSpeed * 2;
            movementSpeed = movementSpeed * 2;
        }
    }
    public void ChangeScene() {
        float score = Mathf.Round(PoopIncrement.score);
        GameObject canvas = GameObject.FindGameObjectWithTag("UI");

        if(canvas != null)
            canvas.SetActive(false);

        Scene_Manager_BH._instance.settings.UpdateScore(score);
        Scene_Manager_BH._instance.ChangeLevel(false,3);
    }
    public void SpawnPosition()
    {
        int indexTeleport = Random.Range(0,spawners.Length);
        gameObject.transform.position = spawners[indexTeleport].transform.position;
    }
    public void Revive()
    {   
        //Restart player
        isAlive = true;
        animBeetle.SetTrigger("revive");
        animBeetle.SetLayerWeight(0, 1f);
        animBeetle.SetLayerWeight(1, 0f);
        camPos.InverseRot = false;
        dir = 1f;
        //RestartPoop
        GameObject poop = GameObject.FindGameObjectWithTag("Poop");
        if (poop != null)
        {
            poop.transform.position = frontCollider.transform.position;
            poop.transform.localScale = Vector3.one;
            poop.transform.parent = this.transform;
            canHold = false;
        }
        else
        {
            CreateNewPoop(frontCollider.gameObject);
        }
        //Restart Spawners
        spawners = GameObject.FindGameObjectsWithTag("SpawnerPlayer");
        SpawnPosition();

    }
    public void RotationPoop(float x, float y)
    {
        if (poopRigid != null)
        {
            
            //Si se mueve hacia adelante
            if (y > 0)
            {
                poopRigid.transform.GetChild(0).RotateAround(poopRigid.transform.GetChild(1).position, poopRigid.transform.right, CacaRotVel * Time.deltaTime);
            }
            //Si se mueve hacia atras
            if (y < 0)
            {
                poopRigid.transform.GetChild(0).RotateAround(poopRigid.transform.GetChild(1).position, poopRigid.transform.right, -CacaRotVel * Time.deltaTime);
            }
            //Si se mueve a la derecha
            if (x > 0)
            {
                poopRigid.transform.GetChild(0).RotateAround(poopRigid.transform.GetChild(1).position, poopRigid.transform.forward, -CacaRotVel * Time.deltaTime);
            }
            //Si se mueve hacia la izquierda
            if (x < 0)
            {
                poopRigid.transform.GetChild(0).RotateAround(poopRigid.transform.GetChild(1).position, poopRigid.transform.forward, CacaRotVel * Time.deltaTime);
            }
        }
    }

    public void IfNoPoop()
    {
        frontCollider.isPoop = false;
        backCollider.isPoop = false;
        animBeetle.SetBool("holding", true);
        Arrow.gameObject.SetActive(false); 
        if(poopRigid!= null)
            poopRigid.transform.parent = null;

        poopRigid = null;
        canHold = true;
    }
}
