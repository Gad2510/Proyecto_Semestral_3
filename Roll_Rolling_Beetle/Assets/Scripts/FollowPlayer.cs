using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    bool inverseRot;// Bool para saber si se rota la camara
    public float offsetDistance;// Ajustador de la posicion cuando esta rotada la camara seleccionar posicion
    float rotation;// Modifica la rotacion de la camara
    float offset;//Modifica la posicion de la camara
    public float transitionDuration = 0.5f; // Duracion del cambio de camara
    GameObject refToPlayer;//Referencia al jugador

    public GameObject PlayerPos {
        set { refToPlayer = value; }
    }

    public bool InverseRot
    {
        set
        {
            if (value != inverseRot)
            {
                StartCoroutine(RotationTransition());
            }
            inverseRot = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (refToPlayer != null)
        {
            //CALCULO DE POSICION
            Vector3 pos;
            pos.y = this.transform.position.y;
            pos.x = refToPlayer.transform.position.x;
            pos.z = refToPlayer.transform.position.z;
            Vector3 rot = refToPlayer.transform.eulerAngles;
            //MODIFICADORES PARA LA ANIMACION
            rot += Vector3.up * rotation;
            Vector3 frwd = refToPlayer.transform.forward;
            frwd.y = 0.0f;
            pos += frwd * offset;
            //SET DE POSICIONES
            this.transform.position = pos;
            this.transform.eulerAngles = rot;
        }
    }

    IEnumerator RotationTransition()
    {
        float offsetPos = offset;
        float rot = rotation;
        float counter = 0f;
        while (counter < 1f)
        {
            float dir = (inverseRot) ? counter : 1-counter;//Selecciona una manera de mover las variables
            offsetPos = (dir * offsetDistance);//Multiplica el valor total por las fracciones del resultante
            rot = (dir * 180);
            offset = offsetPos;//Pone los valores en su respectivo lugar
            rotation = rot;
            counter += Time.deltaTime/transitionDuration;
            yield return null;
        }

        rotation = (inverseRot) ? 180f:0f;//Set para que queden los valores en posiciones exactas
        offset = (inverseRot) ? offsetDistance:0f;
    }
}
