using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruirTrigger : MonoBehaviour
{
    public GameObject poop;
    float maxScale = 2f;
    private void Start()
    {
        poop = GameObject.FindGameObjectWithTag("Poop");
    }

    //Aumentar puntuación
    private void OnTriggerEnter(Collider other)
    {
        //Destruir al ser tocadas por la bola
        if(other.gameObject.CompareTag("Poop"))
        {
            if(poop.GetComponent<Transform>().localScale.y < maxScale)
            {
                PoopIncrement.score += 1.0f;
                Destroy(gameObject);
            }
            
            
            //Debug.Log(PoopIncrement.score);
        }
    }
}
