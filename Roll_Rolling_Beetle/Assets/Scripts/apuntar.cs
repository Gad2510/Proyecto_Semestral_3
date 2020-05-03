using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class apuntar : MonoBehaviour
{
    public bool si;
    public GameObject jugador;

    // Update is called once per frame
    void Update()
    {
        LookPlayer();
    }
    
    private void LookPlayer()
    {
        if(si==true)
        {
            transform.LookAt(jugador.transform);
           transform.rotation = Quaternion.Euler(118.258f, 0.0f, transform.localEulerAngles.z);
           // transform.LookAt(jugador , Vector3.left);
            print("siveo");
        }
    }
}
