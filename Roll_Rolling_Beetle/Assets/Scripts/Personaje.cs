using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { WALKING, THROWING, DEAD}

public class Personaje : MonoBehaviour
{
    Animator animBeetle;
    Rigidbody rigi;
    public Rigidbody poopRigid;
    public bool isAlive;
    public float maxMovementSpeed;
    public float minMovementSpeed;
    public float maxRotationSpeed;
    public float minRotationSpeed;
    public float timeSinceShot;
    public bool poopshooted;
    public PlayerState state;



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
        animBeetle = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody>();
        canHold = false;
        isMoving = false;
        poopshooted = false;
        isAlive = true;
        maxMovementSpeed = 10;
        maxRotationSpeed = 50;
    }
    void Update()
    {
        if (isAlive && state == PlayerState.WALKING)
        {
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
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
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
    public void ShootPoop()
    {
        poopRigid.transform.parent = null;
        poopRigid.velocity = transform.forward * 10;
        canHold = true;
        poopshooted = true;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Poop")
        {
            if (canHold)
            {
                animBeetle.SetTrigger("holding");
                poopRigid.transform.SetParent(transform);
                canHold = false;
            }
        }
        if (collision.gameObject.tag == "Enemigo")
        {
            animBeetle.SetTrigger("dead");
            isAlive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Baba"))
        {
            maxMovementSpeed = maxMovementSpeed / 2;
            movementSpeed = movementSpeed / 2;
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
}
