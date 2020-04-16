using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gusano : MonoBehaviour
{
    BoxCollider coll;
    public GameObject posicionBala;
    public GameObject posicionRayo;
    GameObject jugador;
    public GameObject bala;
    public float recarga = 0;
    public float cadencia = 3.0f;
    public LayerMask detection;
    Animator anim;
    bool aumento = false;
    void Start()
    {
        coll = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
        jugador = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Aumento de cadencia cuando el jugador llega a 1500 puntos
        if (PoopIncrement.score > 1500 && !aumento)
        {
            cadencia = 2.0f;
            aumento = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Poop"))
        {
            coll.enabled = false;
            anim.SetTrigger("hit");
            Destroy(gameObject,2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetBool("in_out", true);//Elevar torreta
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetBool("in_out", false); //Esconder torreta
            recarga = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            posicionRayo.transform.LookAt(jugador.transform);
            transform.LookAt(jugador.transform);
            transform.rotation = Quaternion.Euler(0.0f, transform.localEulerAngles.y, transform.localEulerAngles.z); //Bloquear rotación X
            posicionRayo.transform.rotation = Quaternion.Euler(0.0f, posicionRayo.transform.localEulerAngles.y, posicionRayo.transform.localEulerAngles.z);

            RaycastHit hit;

            Debug.DrawRay(posicionRayo.transform.position, posicionRayo.transform.forward * 15, Color.red);

            Ray direction = new Ray(posicionRayo.transform.position, posicionRayo.transform.forward);

            if (Physics.Raycast(direction, out hit,15f,detection))
            {
                Debug.Log(hit.collider.gameObject.tag);
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    transform.LookAt(jugador.transform); //Mirar al jugador
                    transform.rotation = Quaternion.Euler(0.0f, transform.localEulerAngles.y, transform.localEulerAngles.z); //Bloquear rotación X

                    recarga += Time.deltaTime;
                    anim.SetBool("shoot", false);
                }
                else
                {
                    recarga = 0;
                }
            }
            if(recarga >= cadencia)
            {
                anim.SetBool("shoot", true);
                Instantiate(bala, posicionBala.transform);
                recarga = 0;
            }
        }
    }
}
