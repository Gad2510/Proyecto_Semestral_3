using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gusano : MonoBehaviour
{
    public GameObject posicionBala;
    public GameObject posicionRayo;
    GameObject jugador;
    public GameObject bala;
    public float recarga = 0;
    public float cadencia = 3.0f;
    public LayerMask detection;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        jugador = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (PoopIncrement.score > 100)
        {
            cadencia = 2.0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Poop"))
        {
            Destroy(gameObject);
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

            Debug.DrawRay(posicionRayo.transform.position, posicionRayo.transform.forward * 10, Color.red);

            Ray direction = new Ray(posicionRayo.transform.position, posicionRayo.transform.forward);

            if (Physics.Raycast(direction, out hit,10f,detection))
            {

                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    transform.LookAt(jugador.transform); //Mirar al jugador
                    transform.rotation = Quaternion.Euler(0.0f, transform.localEulerAngles.y, transform.localEulerAngles.z); //Bloquear rotación X

                    recarga += Time.deltaTime;
                }
                else
                {
                    recarga = 0;
                }
            }
            if(recarga >= cadencia)
            {
                Instantiate(bala, posicionBala.transform);
                recarga = 0;
            }
        }
    }
}
