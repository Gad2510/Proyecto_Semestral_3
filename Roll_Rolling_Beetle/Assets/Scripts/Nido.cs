using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nido : MonoBehaviour
{
    PoopIncrement poop;
    Personaje per;

    void Start()
    {
        poop = GameObject.FindGameObjectWithTag("Poop").GetComponent<PoopIncrement>();
        per = GameObject.FindGameObjectWithTag("Player").GetComponent<Personaje>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Poop") && poop.CacaPorcentage.value >= 0.9f)
        {
            per.IfNoPoop();
            poop.CacaPorcentage.value = 0.0f;
            PoopIncrement.score += 500.0f;
            poop.screenPoints.text = "SCORE: " + Mathf.Round(PoopIncrement.score).ToString();

            
            Destroy(other.gameObject);
        }
    }
}
