using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColor : MonoBehaviour
{
    MeshRenderer mesh;
    Color whiteRef, initCol;
    ParticleSystem particulas;
    bool isChanging = false , playPart=true;
   
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        whiteRef = Color.white;
        initCol = mesh.material.color;

        particulas = transform.GetChild(0).GetComponent<ParticleSystem>();
        //particulas.main
        particulas.Pause(true);
        
    }

    void Update()
    {
        if(!Personaje.IsPoopInGame)
        {
            isChanging=true;
            mesh.material.color = Color.Lerp(initCol, whiteRef, Mathf.Sin(Time.time*2));
            particulas.transform.Rotate(Vector3.up);
            if (playPart)
            {
                particulas.Play(true);
                playPart = false;
            }
            
        }
        else if (isChanging)
        {
            isChanging = false;
            playPart = true;
            mesh.material.color = initCol;
            particulas.Pause(true);
        }
    }
}
