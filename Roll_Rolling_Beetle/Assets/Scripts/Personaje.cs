using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { WALKING, THROWING, DEAD}

public class Personaje : MonoBehaviour
{
    Animator animBeetle;
    Rigidbody rigi;
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
    public FrontCollider frontCollider;

    Vector3 fingerDir;

    float startTouch, endTouch, timeTouched;

    [SerializeField]
    float movementSpeed, rotationSpeed;
    

    bool canHold, isMoving;

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
        
        FollowPlayer cam = Camera.main.GetComponent<FollowPlayer>();
        cam.PlayerPos = this.transform.Find("CameraPoint").gameObject;
        state = PlayerState.WALKING;
        spawners = GameObject.FindGameObjectsWithTag("SpawnerPlayer");
        animBeetle = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody>();
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
                state = PlayerState.THROWING;
                animBeetle.SetTrigger("shoot");
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
        state = PlayerState.THROWING;
        poopRigid.transform.parent = null;
        poopRigid.velocity = transform.forward * 10;
        canHold = true;
        poopshooted = true;
        timeTouched = 0f;
        actualPoop = null;
    }

    public void FrontColliderAction()
    {
        if (frontCollider.isPoop)
        {
            actualPoop = frontCollider.collide.gameObject;
            poopRigid = actualPoop.GetComponent<Rigidbody>();
            animBeetle.SetTrigger("holding");
            poopRigid.transform.SetParent(transform);
            canHold = false;
            Debug.Log(actualPoop.transform.localScale.x * .1f);
            frontCollider.collide.gameObject.transform.localPosition = new Vector3(0, 0, .183f + (actualPoop.transform.localScale.x * .03f));
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Poop")
        {
            if (canHold)
            {
                FrontColliderAction();
            }
        }
        if (isAlive && collision.gameObject.tag == "Enemigo")
        {
            animBeetle.SetTrigger("dead");
            isAlive = false;
            ChangeScene();//Change Escene
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Baba"))
        {
            maxMovementSpeed = maxMovementSpeed / 2;
            movementSpeed = movementSpeed / 2;
        }
        if (other.gameObject.CompareTag("Bird"))
        {
            //ChangeScene();
            FollowPlayer cam = Camera.main.GetComponent<FollowPlayer>();
            cam.PlayerPos = null;
            Destroy(this.transform.parent.gameObject);
        }
        if (other.gameObject.tag == "bonus" && canHold && !isPoopInGame)
        {
            Destroy(other.gameObject);
            poopPrefab = GameObject.Instantiate(poopPrefab, transform.position, Quaternion.identity, this.transform);
            actualPoop = poopPrefab;
            poopPrefab.transform.localScale = new Vector3(1, 1, 1);
            poopPrefab.transform.localPosition = frontCollider.transform.localScale;
            Debug.Log(actualPoop.transform.localScale.x * .1f);
            poopPrefab.transform.localPosition += new Vector3(0,0,.183f + (actualPoop.transform.localScale.x * .03f));
            frontCollider.isPoop = true;
            poopRigid = poopPrefab.GetComponent<Rigidbody>();
            animBeetle.SetTrigger("holding");
            canHold = false;
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
        SceneManage._instance.settings.UpdateScore(score);
        SceneManage._instance.ChangeLevel("GameOver", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public void SpawnPosition()
    {
        int indexTeleport = Random.Range(0,spawners.Length);
        Debug.Log(indexTeleport);
        gameObject.transform.position = spawners[indexTeleport].transform.position;
    }
}
