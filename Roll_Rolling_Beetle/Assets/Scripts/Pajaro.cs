using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pajaro : MonoBehaviour
{
    public float contadorAtaque = 0.0f; //Contador para atacar al jugador si esta dentro se su trigger
    public bool atacando = false; //Saber si esta atacando
    public Transform bird;
    public MeshRenderer matRef;
    public Animator birdAnimation;
    Vector3 originPos;
    float velocityWrning;
    Color colorWarnning;
    Collider selfCollider;

    [SerializeField]
    Collider triggerCollider =null;

    void Start()
    {
        selfCollider = this.GetComponent<Collider>();
        velocityWrning= matRef.material.GetFloat("_xMovement");
        colorWarnning = Color.black;
        matRef.material.SetColor("_Color", colorWarnning);
        originPos = bird.position;
        InvokeRepeating("CambiarPos",6f,6f);
        triggerCollider.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            atacando = true; //Atacar
            
            contadorAtaque += Time.deltaTime;//Sumar

            if (contadorAtaque >= 3.0f)
            {
                contadorAtaque = 0.0f;
                birdAnimation.SetTrigger("down");
                StartCoroutine(Attack(other.transform));
                selfCollider.enabled = false;

            }
            colorWarnning.r = (contadorAtaque / 3);
            matRef.material.SetColor("_Color", colorWarnning);
            matRef.material.SetFloat("_xMovement", velocityWrning * (contadorAtaque/3));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            atacando = false;
            contadorAtaque = 0.0f;
            colorWarnning = Color.black;
            matRef.material.SetColor("_Color", colorWarnning);
            matRef.material.SetFloat("_xMovement", velocityWrning);
        }
    }

    private IEnumerator Attack(Transform player)
    {
        float counter = 0f;
        float timer = 35 / 24;
        Vector3 origin = bird.transform.position;
        while (counter <= 1f)
        {
            counter += Time.deltaTime/timer;
            bird.transform.position = Vector3.Lerp(origin, player.position,counter);
            yield return null;
        }
        triggerCollider.enabled = true;
        StartCoroutine(ReturnSky());
    }

    private IEnumerator ReturnSky()
    {
        float c=0;
        Vector3 origin = bird.transform.position;
        Vector3 end = origin+(bird.forward*10);
        end.y += 10f;
        while (c < 1f)
        {
            bird.transform.position = Vector3.Lerp(origin, end, c);
            c += Time.deltaTime*2;
            yield return null;
        }
        triggerCollider.enabled = false;
        bird.position = originPos;
        selfCollider.enabled = true;
    }

    public void CambiarPos()
    {
        if (atacando)
        {
            return;
        }

        int pos;
        pos = Random.Range(-50,50);

        gameObject.transform.position = new Vector3(pos, gameObject.transform.position.y, gameObject.transform.position.z);
    }
}
