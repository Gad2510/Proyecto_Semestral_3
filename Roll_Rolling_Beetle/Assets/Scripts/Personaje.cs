using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { WALKING, THROWING, DEAD}

public class Personaje : MonoBehaviour
{
    Animator animBeetle;
    public GameObject[] spawners;
    public Rigidbody poopRigid;
    public float maxMovementSpeed;
    public float minMovementSpeed;
    public float maxRotationSpeed;
    public float minRotationSpeed;
    public float timeSinceShot;
    public float spawnIndex;
    public bool isAlive;
    public bool isPoopInGame;
    public bool poopshooted;
    public PlayerState state;
    public GameObject poopPrefab;
    public GameObject actualPoop;
    public FrontCollider frontCollider, backCollider;
    public float CacaRotVel;
    float dir=1f;

    //Flecha de direccion de popo
    public apuntar Arrow;
    public GameObject ArrowHome;

    FollowPlayer camPos;

    Vector3 fingerDir;

    float startTouch, endTouch, timeTouched;

    [SerializeField]
    float movementSpeed, rotationSpeed;
    

    public bool canHold, isMoving;

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
        camPos.PlayerPos = this.transform.Find("CameraPoint").gameObject;
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
        ArrowHome.SetActive(true);
    }
    void Update()
    {
        if (isAlive && state == PlayerState.WALKING)
        {
            TapCalculation();
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            //float x = fingerDir.x;
            //float y = fingerDir.y;
            y *= dir;
            timeSinceShot = 0;
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
            if (GameObject.FindGameObjectsWithTag("Poop").Length == 0)
            {
                isPoopInGame = false;
            }
            else
            {
                isPoopInGame = true;
            }

            transform.Rotate(0, x * Time.deltaTime * rotationSpeed, 0);
            transform.Translate(0, 0,  y* Time.deltaTime * movementSpeed);
            float tringulate = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
            if (isPoopInGame)
            {
                isMoving = (!poopRigid.IsSleeping()) && tringulate > 0.1f;
            }
            animBeetle.SetBool("isWalking", tringulate > 0.1f);


            if (Input.GetKeyDown(KeyCode.Space) && !canHold)
            {
                animBeetle.SetTrigger("shoot");
                actualPoop.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        else if (state == PlayerState.THROWING)
        {
            timeSinceShot += Time.deltaTime;
            if(timeSinceShot >= 1.5f)
            {
                state = PlayerState.WALKING;
                poopshooted = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            animBeetle.SetTrigger("dead");
            isAlive = false;
        }
    }

    private void TapCalculation()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                fingerDir = Input.touches[0].deltaPosition;
                fingerDir.x = fingerDir.x / (Screen.width/100);
                fingerDir.y = fingerDir.y / (Screen.height/100);
                startTouch = Time.realtimeSinceStartup;
            }
            else if(Input.touches[0].phase==TouchPhase.Ended)
            {
                fingerDir = Vector3.zero;
                endTouch = Time.realtimeSinceStartup;
                timeTouched = endTouch-startTouch;
            }

            if (!canHold && timeTouched > 1f)
            {
                ShootPoop();
            }

        }
    }
    public void ShootPoop()
    {
        Arrow.gameObject.SetActive(true); //Activa la flecha al apuntar
        ArrowHome.SetActive(false); //Desactivar la de la casa
        state = PlayerState.THROWING;
        animBeetle.SetBool("holding", true);
        poopRigid.transform.parent = null;
        poopRigid.velocity = transform.forward * 10* dir;
        canHold = true;
        poopshooted = true;
        timeTouched = 0f;
        actualPoop = null;
    }
    public void FrontColliderAction()
    {
        Arrow.gameObject.SetActive(false); //Si agarra la popo desactiva la flecha
        ArrowHome.SetActive(true);
        if (frontCollider.isPoop )
        {
            ChooseCollider(frontCollider);
            animBeetle.SetLayerWeight(0, 1f);
            animBeetle.SetLayerWeight(1, 0f);
            camPos.InverseRot=false;
            dir = 1f;
        }
        else if (backCollider.isPoop)
        {
            ChooseCollider(backCollider);
            animBeetle.SetLayerWeight(0, 0f);
            animBeetle.SetLayerWeight(1, 1f);
            camPos.InverseRot = true;
            dir = -1f;
        }
    }

    void ChooseCollider(FrontCollider currentColl, bool animState=false)
    {
        actualPoop = currentColl.collide.gameObject;
        poopRigid = actualPoop.GetComponent<Rigidbody>();
        animBeetle.SetBool("holding", false);
        poopRigid.transform.SetParent(transform);
        poopRigid.isKinematic = true;
        canHold = false;
        currentColl.collide.gameObject.transform.position = currentColl.transform.position; //Girar al agarrar. por favor no lo borren
        actualPoop.transform.rotation = this.transform.rotation;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (isAlive && collision.gameObject.tag == "Enemigo")
        {
            animBeetle.SetTrigger("dead");
            isAlive = false;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TriggerPoop"))
        {
            //print("hola");
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
            FollowPlayer cam = Camera.main.GetComponent<FollowPlayer>();
            cam.PlayerPos = null;
            isAlive = false;
            this.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("bonus") && canHold && !isPoopInGame)
        {
            Destroy(other.gameObject);
            actualPoop = Instantiate(poopPrefab, frontCollider.transform.position, Quaternion.identity, this.transform);
            frontCollider.isPoop = true;
            actualPoop.transform.rotation = this.transform.rotation;
            poopRigid = actualPoop.GetComponent<Rigidbody>();
            poopRigid.isKinematic = true;
            animBeetle.SetBool("holding", false);
            canHold = false;
            Arrow.searchnewpoop();

        }
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
        isAlive = true;
        animBeetle.SetTrigger("revive");
        GameObject poop = GameObject.FindGameObjectWithTag("Poop");
        poop.transform.position = frontCollider.transform.position;
        poop.transform.parent = this.transform;
        canHold = false;
        spawners = GameObject.FindGameObjectsWithTag("SpawnerPlayer");
        SpawnPosition();

    }
    public void RotationPoop(float x, float y)
    {
        if (isPoopInGame && actualPoop != null)
        {
            
            //Si se mueve hacia adelante
            if (y > 0)
            {
                actualPoop.transform.GetChild(0).RotateAround(actualPoop.transform.GetChild(1).position, actualPoop.transform.right, CacaRotVel * Time.deltaTime);
            }
            //Si se mueve hacia atras
            if (y < 0)
            {
                actualPoop.transform.GetChild(0).RotateAround(actualPoop.transform.GetChild(1).position, actualPoop.transform.right, -CacaRotVel * Time.deltaTime);
            }
            //Si se mueve a la derecha
            if (x > 0)
            {
                actualPoop.transform.GetChild(0).RotateAround(actualPoop.transform.GetChild(1).position, actualPoop.transform.forward, -CacaRotVel * Time.deltaTime);
            }
            //Si se mueve hacia la izquierda
            if (x < 0)
            {
                actualPoop.transform.GetChild(0).RotateAround(actualPoop.transform.GetChild(1).position, actualPoop.transform.forward, CacaRotVel * Time.deltaTime);
            }
        }
    }

    public void IfNoPoop()
    {
        frontCollider.isPoop = false;
        animBeetle.SetBool("holding", true);
        poopRigid.transform.parent = null;
        poopRigid.velocity = transform.forward * 10;
        canHold = true;
        timeTouched = 0f;
        actualPoop = null;
    }
}
