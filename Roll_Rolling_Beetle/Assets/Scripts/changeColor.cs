using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColor : MonoBehaviour
{
    Personaje player;
    MeshRenderer mesh;
    Color whiteRef, initCol;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Personaje>();
        mesh = GetComponent<MeshRenderer>();
        whiteRef = Color.white;
        initCol = mesh.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.canHold==true && player.poopRigid == null)
        {
            mesh.material.color = Color.Lerp(initCol, whiteRef, Mathf.Sin(Time.time*2));
        }
    }
}
