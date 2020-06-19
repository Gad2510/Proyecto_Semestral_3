using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Mantis : MonoBehaviour
{
    Collider selfcoll;
    SphereCollider rango;
    NavMeshAgent navAgent;
    Animator anim;
    GameObject sign;
    Camera main;
    UnityEngine.UI.Slider CacaPorcentage;
    float aumento1 = 1.3f;
    float aumento2 = 1.6f;
    int walkIndex;
    int walkIndexPrev;

    public float radioDeteccion;
    public GameObject particlesM;
    public Transform[] walkPoints;
    public float walkSpeed = 0.8f;
    public bool isIdle = false;
    public Object signPrefab;
    public bool siguiendoJugador = false;
    public Transform headpos;

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
        main = Camera.main;
        //Activar aniimaciones en base al bool isIdle
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("walking", true);
        particlesM.SetActive(false);
        
    }

    private void Update()
    {
        if (!isIdle)
        {
            anim.SetBool("walking", true);
            ChooseWalkPoint();
        }
        if (PoopIncrement.score > 1500 && siguiendoJugador)//Primer aumento
        {
            walkSpeed = aumento1;
            anim.speed = walkSpeed;
        }
        if (PoopIncrement.score > 3000 && siguiendoJugador)//Segundo aumento
        {
            walkSpeed = aumento2;
            anim.speed = walkSpeed;
        }
        if (!siguiendoJugador)
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
            if (!siguiendoJugador)
            {
                sign = Instantiate(signPrefab, CrearMapa._instance.canvas.transform) as GameObject;
                Destroy(sign,3f);
            }
            if (sign != null)
            {
                sign.transform.position = main.WorldToScreenPoint(headpos.position);
            }
            
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
        if (collision.gameObject.CompareTag("Poop"))
        {
            if (CacaPorcentage.value >= 0.1f && collision.transform.parent==null)
            {
                anim.SetTrigger("dead");
                particlesM.SetActive(true);
                selfcoll.enabled = false;
                navAgent.isStopped = true;
                Destroy(gameObject, 0.5f);
                AudioManager.GetInstance().PlayAudio(AUDIO_TYPE.MUERTE_MANTIS);

            }
        }
        
    }

    private void OnDisable()
    {
        //Regrasar a la posicion del ultimo walkpoint al desactivar
        gameObject.transform.position = walkPoints[walkIndex].position;
    }
}
