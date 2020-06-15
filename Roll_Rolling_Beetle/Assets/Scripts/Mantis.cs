using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Mantis : MonoBehaviour
{
    public Collider selfcoll;
    public SphereCollider rango;
    UnityEngine.UI.Slider CacaPorcentage;
    public float radioDeteccion;
    public Transform poopSize;
    public GameObject particlesM;

    public Transform[] walkPoints;
    public float walkSpeed = 1.0f;
    public bool isIdle = false;

    float aumento1 = 1.3f;
    float aumento2 = 1.6f;

    int walkIndex;
    int walkIndexPrev;

    NavMeshAgent navAgent;
    public Animator anim;
    Transform sign;
    Camera main;
    public bool siguiendoJugador = false;

    private void Awake()
    {
        selfcoll = GetComponent<Collider>();
        CacaPorcentage = CrearMapa.slider;// Referencia al slider
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        rango = GetComponent<SphereCollider>();
        rango.radius = radioDeteccion;
    }

    private void Start()
    {
        sign=CrearMapa._instance.canvas.transform.Find("Sign");
        main = Camera.main;
        //Activar aniimaciones en base al bool isIdle
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("walking", true);
        particlesM = GameObject.FindWithTag("Mantis_CFX");
        particlesM.SetActive(false);
    }

    private void Update()
    {
        if (!isIdle)
        {
            anim.SetBool("walking", true);
            ChooseWalkPoint();
        }
        if(PoopIncrement.score > 1500 && siguiendoJugador)//Primer aumento
        {
            walkSpeed = aumento1;
            anim.speed = walkSpeed;
        }
        if (PoopIncrement.score > 3000 && siguiendoJugador)//Segundo aumento
        {
            walkSpeed = aumento2;
            anim.speed = walkSpeed;
        }
        if(!siguiendoJugador)
        {
            walkSpeed = 1.0f;
            anim.speed = walkSpeed;
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
            sign.position = main.WorldToScreenPoint(this.transform.position);
            siguiendoJugador = true;
            transform.LookAt(other.transform);
            navAgent.SetDestination(other.transform.position);
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
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Personaje.IsAlive) //Verifica si el jugador sige vivo
            {
                
                //Detenerlo
                anim.SetTrigger("attack");
                AudioManager.GetInstance().PlayAudio(AUDIO_TYPE.ATAQUE_MANTIS);
                navAgent.isStopped = true;
                isIdle = true;
                anim.SetBool("walking", false);

            }
        }

        else
        {
            if (collision.gameObject.CompareTag("Poop") && !Personaje.CanHold)
            {
                poopSize= collision.transform;
                if (CacaPorcentage.value >= 0.7f)
                {
                    anim.SetTrigger("dead");
                    selfcoll.enabled = false;
                    Destroy(gameObject, 0.5f);
                    AudioManager.GetInstance().PlayAudio(AUDIO_TYPE.MUERTE_MANTIS);
                    particlesM.SetActive(true);
                }
            }
        }
    }
}
