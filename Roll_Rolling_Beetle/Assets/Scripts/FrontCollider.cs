using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCollider : MonoBehaviour
{
    public bool isPoop;
    public Collider collide;

    Personaje player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Personaje>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Poop"))
        {
            isPoop = true;
            collide = other;
        }
        if (other.gameObject.CompareTag("bonus") && player.CanHold() && !Personaje.IsPoopInGame)
        {
            Destroy(other.gameObject);
            player.CreateNewPoop(this.gameObject);
        }

    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Poop"))
        {
            isPoop = false;
            collide = null;
        }
    }
}
