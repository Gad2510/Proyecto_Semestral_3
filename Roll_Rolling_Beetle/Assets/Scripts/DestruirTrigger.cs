using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruirTrigger : MonoBehaviour
{
    //Aumentar puntuación
    private void OnTriggerEnter(Collider other)
    {
        //Destruir al ser tocadas por la bola
        if(other.gameObject.CompareTag("Poop"))
        {
            PoopIncrement.score += 1;
            Destroy(gameObject);
        }
    }
}
