﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gusano : MonoBehaviour
{
    BoxCollider coll;
    SphereCollider trig;
    public GameObject posicionBala;
    public GameObject posicionRayo;
    UnityEngine.UI.Slider CacaPorcentage;
    public GameObject bala;
    public float recarga = 0;
    public float cadencia = 3.0f;
    public LayerMask detection;
    Animator anim;
    bool aumento = false,dead= false;
    public GameObject particles;
    //bool reload;
   

    void Start()
    {
        coll = GetComponent<BoxCollider>();
        trig = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        CacaPorcentage = CrearMapa.slider;// Referencia al slider
        particles = GameObject.FindWithTag("Gusano_CFX");
        particles.SetActive(false);
    }

    void Update()
    {
        if (!dead)
        {
            //Aumento de cadencia cuando el jugador llega a 1500 puntos
            if (PoopIncrement.score > 1500 && !aumento)
            {
                cadencia = 2.0f;
                aumento = true;
            }
            
        }
       
 
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Poop") && CacaPorcentage.value >= 0.4f) //Comparar si la popo esta al porcentaje adecuado para que te mate
        {
            coll.enabled = false;
            trig.enabled = false;
            anim.SetTrigger("hit");
            Invoke("Restart", 10f);
            AudioManager.GetInstance().PlayAudio(AUDIO_TYPE.MUERTE_GUSANO);
            particles.SetActive(true);
           // reload = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            coll.enabled = true;
            anim.SetBool("in_out", true);//Elevar torreta
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            coll.enabled = false;
            anim.SetBool("in_out", false); //Esconder torreta
            recarga = 0;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            posicionRayo.transform.LookAt(other.transform);
            transform.LookAt(other.transform);
            transform.rotation = Quaternion.Euler(0.0f, transform.localEulerAngles.y, transform.localEulerAngles.z); //Bloquear rotación X
            posicionRayo.transform.rotation = Quaternion.Euler(0.0f, posicionRayo.transform.localEulerAngles.y, posicionRayo.transform.localEulerAngles.z);
            Detection();
        }
    }

    private void Detection()
    {
        RaycastHit hit;

        Ray direction = new Ray(posicionRayo.transform.position, posicionRayo.transform.forward);

        if (Physics.Raycast(direction, out hit, 20f, detection))
        {
            
            if (hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("Poop"))
            {
                transform.LookAt(hit.transform); //Mirar al jugador
                transform.rotation = Quaternion.Euler(0.0f, transform.localEulerAngles.y, transform.localEulerAngles.z); //Bloquear rotación X

                recarga += Time.deltaTime;
                if (recarga >= cadencia)
                {
                    anim.SetTrigger("shoot");
                    recarga = 0;
                }
            }
            else
            {
                recarga = 0;
            }
        }
    }


    private void OnGUI()
    {
        Debug.DrawRay(posicionRayo.transform.position, posicionRayo.transform.forward * 15, Color.red);
    }

    public void Shoot()
    {
        Instantiate(bala, posicionBala.transform);
        AudioManager.GetInstance().PlayAudio(AUDIO_TYPE.ATAQUE_GUSANO);
    }

    private void Restart()
    {
        dead = false;
        trig.enabled = true;
    }
}
