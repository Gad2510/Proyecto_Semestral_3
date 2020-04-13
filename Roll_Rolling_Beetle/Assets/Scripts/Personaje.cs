﻿using System.Collections;
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
    public bool poopshooted;
    public PlayerState state;
    public GameObject poopPrefab;
    public GameObject actualPoop;


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
            
            transform.Rotate(0, x * Time.deltaTime * rotationSpeed, 0);
            transform.Translate(0, 0,  y* Time.deltaTime * movementSpeed);
            float tringulate = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
            isMoving = (!poopRigid.IsSleeping())&& tringulate>0.1f;
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
        animBeetle.SetTrigger("shoot");
        poopRigid.transform.parent = null;
        poopRigid.velocity = transform.forward * 10;
        canHold = true;
        poopshooted = true;
        timeTouched = 0f;
        actualPoop = null;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Poop")
        {
            if (canHold)
            {
                actualPoop = collision.gameObject;
                poopRigid = actualPoop.GetComponent<Rigidbody>();
                animBeetle.SetTrigger("holding");
                poopRigid.transform.SetParent(transform);
                canHold = false;
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
        if (other.gameObject.tag == "bonus" && canHold)
        {
            Destroy(other.gameObject);
            poopPrefab = GameObject.Instantiate(poopPrefab, transform.position, Quaternion.identity, this.transform);
            poopPrefab.transform.localScale = new Vector3(1, 1, 1);
            poopPrefab.transform.position = new Vector3(poopPrefab.transform.position.x, 1f,poopPrefab.transform.position.z +2f);
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
        //SceneManage._instance.settings.UpdateScore(score);
        //SceneManage._instance.ChangeLevel(3);
    }

    public void SpawnPosition()
    {
        int indexTeleport = Random.Range(0,spawners.Length);
        Debug.Log(indexTeleport);
        gameObject.transform.position = spawners[indexTeleport].transform.position;
    }
}
