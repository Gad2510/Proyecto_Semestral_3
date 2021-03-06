﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Nido : MonoBehaviour
{
    [SerializeField]
    float centerrTrans = 1f;
    Personaje per;// referencia al personaje
    Slider CacaPorcentage;//Referencia al slider de crecimiento
    //public apuntar a;

    ParticleSystem [] effect;
    void Start()
    {
        effect = GetComponentsInChildren<ParticleSystem>();
        CacaPorcentage = CrearMapa.slider;// Referencia al slider
    }

    private void OnTriggerEnter(Collider other)
    {
        //Comprobar si entra la bola y esta a su tamaño maximo
        if(other.gameObject.CompareTag("Poop") && CacaPorcentage.value >= 0.9f)
        {
            if (per == null)
            {
                per = GameObject.FindGameObjectWithTag("Player").GetComponent<Personaje>();
            }
            per.IfNoPoop();//Funcion del jugador para resetear sus valores
            other.enabled = false;
            PoopIncrement poop = other.GetComponent<PoopIncrement>(); // Busco la referencia aqui encaso que se destruya la original
            poop.CacaPorcentage.value = 0.0f;//El slider regresa a cero
            PoopIncrement.score += 500.0f;//Da un bonus de puntuación por entregarla
            poop.screenPoints.text = "SCORE: " + Mathf.Round(PoopIncrement.score).ToString();//Se imprime en la pantalla
            StartCoroutine(Entrega(poop.transform));
            HomeUI_icon.startOver = true; //Dejar de escalar el icono de casa en UI
            AudioManager.GetInstance().PlayAudio(AUDIO_TYPE.POPO_ENTREGADA);
        }
    }

    //Animacion de entrega
    private IEnumerator Entrega(Transform obj)
    {
        Vector3 initpos = obj.position;
        float counter = 0f;
        while (counter < 1f)//Se pone en el centro del objeto
        {
            counter += Time.deltaTime/centerrTrans;
            obj.position = Vector3.Lerp(initpos, this.transform.position, counter);
            yield return null;
        }
        counter = 0f;

        initpos = obj.position;//Setea la posicion inicial
        Vector3 finalpos = this.transform.position; //Crea una ubicacion en la parte inferior
        finalpos.y -= obj.localScale.y*2f; // agraga un offset para que se oculte dependiendo de la escala

        foreach(ParticleSystem p in effect)
        {
            p.Play();
        }

        while (counter < 1f) //Se desliza hacia abajo
        {
            counter += Time.deltaTime / centerrTrans;
            obj.position = Vector3.Lerp(initpos, finalpos, counter);
            yield return null;
        }

        foreach (ParticleSystem p in effect)
        {
            p.Stop();
        }
        Destroy(obj.gameObject);
        CrearMapa._instance.player.SearchNewPoop();
    }
}
