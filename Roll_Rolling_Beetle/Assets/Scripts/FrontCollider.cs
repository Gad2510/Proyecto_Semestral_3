using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCollider : MonoBehaviour
{
    public bool isPoop;
    public Collider collide;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Poop"))
        {
            Debug.Log("Toco");
            isPoop = true;
            collide = other;
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
