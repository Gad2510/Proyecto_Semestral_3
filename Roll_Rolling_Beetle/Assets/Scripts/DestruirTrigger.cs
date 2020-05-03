using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruirTrigger : MonoBehaviour
{
    public PoopIncrement poop;

    private void Start()
    {
        poop = GameObject.FindGameObjectWithTag("Poop").GetComponent<PoopIncrement>();
    }

    //Aumentar puntuación
    private void OnTriggerEnter(Collider other)
    {
        //Destruir al ser tocadas por la bola
        if(other.gameObject.CompareTag("Poop"))
        {
            if(poop == null)
            {
                poop = GameObject.FindGameObjectWithTag("Poop").GetComponent<PoopIncrement>();

            }
            else
            {
                if (poop.AddScore())
                {
                    Destroy(gameObject);
                }
            }
            
            
            
            //Debug.Log(PoopIncrement.score);
        }
    }
}
