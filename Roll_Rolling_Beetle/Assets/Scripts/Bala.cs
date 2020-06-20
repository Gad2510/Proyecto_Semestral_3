using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public float vel;
    float velAumento1;
    float velAumento2;
    public Transform rastro;
    public GameObject particulas;

    void Start()
    {
        Invoke("Destructor", 10);//Funcion para destruir el objeto si no choca
        transform.parent = null;
        velAumento1 = vel * 1.3f;
        velAumento2 = vel * 1.5f;
    }

    void Update()
    {
        //Aumento de velocidad cuando el jugador llega a 1500 puntos
        if (PoopIncrement.score > 1500)
        {
            vel = velAumento1;
        }
        //Aumento de velocidad cuando el jugador llega a 1500 puntos
        if (PoopIncrement.score > 2000)
        {
            vel = velAumento2;
        }

        transform.Translate(Vector3.forward * vel * Time.deltaTime);//Mover hacia adelante
        transform.Translate(Vector3.down * Time.deltaTime * (vel / 15));//Mover hacia abajo
        rastro.Translate(Vector3.up * Time.deltaTime * (vel / 15));//Mover el pivot del rastro mientras de escala
        rastro.localScale = new Vector3(rastro.localScale.x, rastro.localScale.y, rastro.localScale.z + Time.deltaTime * (vel * 3.5f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Poop") || collision.gameObject.CompareTag("Enemigo"))
        {
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            vel = 0;
            Destroy(particulas);
            Invoke("Destructor", 1);
        }

        
    }

    void Destructor()
    {
        Destroy(gameObject);
    }
}
