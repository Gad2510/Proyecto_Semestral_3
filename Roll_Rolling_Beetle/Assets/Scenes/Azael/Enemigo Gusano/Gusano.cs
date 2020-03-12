using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gusano : MonoBehaviour
{
    public GameObject posicionBala;
    public GameObject posicionRayo;
    public GameObject jugador;
    public GameObject bala;
    public float recarga = 0;
    public float cadencia = 3.0f;

    


    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Entra");
            transform.position = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z); //Elevar torreta
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Sale");
            transform.position = new Vector3(transform.position.x, transform.position.y - 1.0f, transform.position.z); //Esconder torreta
            recarga = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            posicionRayo.transform.LookAt(jugador.transform);
            posicionRayo.transform.rotation = Quaternion.Euler(0.0f, posicionRayo.transform.localEulerAngles.y, posicionRayo.transform.localEulerAngles.z);

            RaycastHit hit;

            Debug.DrawRay(posicionRayo.transform.position, posicionRayo.transform.forward * 10, Color.red);

            if (Physics.Raycast(posicionRayo.transform.position, posicionRayo.transform.forward, out hit, 10))
            {
                Debug.Log(hit.collider);

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
