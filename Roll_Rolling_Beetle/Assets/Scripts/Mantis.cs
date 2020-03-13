using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mantis : MonoBehaviour
{
    public SphereCollider rango;
    public Personaje player;
    public float radioDeteccion;
    public PoopIncrement poopSize;

    public Transform[] walkPoints;
    public float walkSpeed = 1.0f;
    public bool isIdle;

    int walkIndex;
    int walkIndexPrev;

    NavMeshAgent navAgent;
    Animator anim;

    public bool siguiendoJugador = false;

    private void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        rango = GetComponent<SphereCollider>();
        rango.radius = radioDeteccion;
    }

    private void Start()
    {
        /*//Activar aniimaciones en base al bool isIdle
        if (isIdle)
        {
            anim.Play("Idle");
        }
        else
        {
            anim.Play("Walk");
        }*/
    }

    private void Update()
    {
        if (!isIdle)
        {
            ChooseWalkPoint();
        }
    }

    void ChooseWalkPoint()
    {
        //Seguir su camino
        if (navAgent.remainingDistance <= 0.0f && !siguiendoJugador)
        {
            navAgent.SetDestination(walkPoints[walkIndex].position);
            walkIndexPrev = walkIndex;

            if (walkIndex == walkPoints.Length - 1)
            {
                walkIndex = 0;
            }
            else
            {
                walkIndex++;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Seguir al jugador
        if (other.CompareTag("Player"))
        {
            siguiendoJugador = true;
            transform.LookAt(player.transform);
            navAgent.SetDestination(player.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Seguir su camino
        if (other.CompareTag("Player"))
        {
            siguiendoJugador = false;
            navAgent.SetDestination(walkPoints[walkIndexPrev].position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //Detenerlo
            Debug.Log("Golpe");
            navAgent.isStopped = true;
            navAgent.SetDestination(transform.position);
        }
        if (collision.gameObject.CompareTag("Poop") && poopSize.transform.localScale.y > 16.0f && player.poopshooted == true)
        {
            Debug.Log("muerto");
            Debug.Log(poopSize.transform.localScale.y);
            Destroy(gameObject);
        }
    }
}
