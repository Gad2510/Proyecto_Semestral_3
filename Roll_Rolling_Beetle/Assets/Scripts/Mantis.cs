﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mantis : MonoBehaviour
{
    public Collider selfcoll;
    public SphereCollider rango;
    public GameObject player;
    UnityEngine.UI.Slider CacaPorcentage;
    public float radioDeteccion;
    public Transform poopSize;

    public Transform[] walkPoints;
    public float walkSpeed = 1.0f;
    public bool isIdle = false;

    float aumento1 = 1.3f;
    float aumento2 = 1.6f;

    int walkIndex;
    int walkIndexPrev;

    NavMeshAgent navAgent;
    public Animator anim;

    public bool siguiendoJugador = false;

    private void Awake()
    {
        selfcoll = GetComponent<Collider>();
        CacaPorcentage = GameObject.FindGameObjectWithTag("UI").transform.Find("Slider").GetComponent<UnityEngine.UI.Slider>();// Referencia al slider
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        rango = GetComponent<SphereCollider>();
        rango.radius = radioDeteccion;
    }

    private void Start()
    {
        //Activar aniimaciones en base al bool isIdle
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("walking", true);
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
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Personaje.IsAlive) //Verifica si el jugador sige vivo
            {
                //Detenerlo
                anim.SetTrigger("attack");
                navAgent.isStopped = true;
                isIdle = true;
                anim.SetBool("walking", false);

            }
        }

        else
        {
            if (collision.gameObject.CompareTag("Poop") && Personaje.CanHold == true)
            {
                poopSize= collision.transform;
                if (CacaPorcentage.value >= 0.7f)
                {
                    anim.SetTrigger("dead");
                    selfcoll.enabled = false;
                    Destroy(gameObject, 0.5f);
                    AudioManager.GetInstance().PlayAudio(AUDIO_TYPE.MUERTE_MANTIS);
                }
            }
        }
    }
}
