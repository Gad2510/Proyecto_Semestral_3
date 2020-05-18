using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    bool inverseRot;// Bool para saber si se rota la camara
    public float transitionDuration = 0.5f; // Duracion del cambio de camara

    Transform pivot;

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

    private void Start()
    {
        pivot = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator RotationTransition()
    {
        float counterRot = 0f;
        Vector3 init = pivot.eulerAngles;
        Vector3 change = init;
        while (counterRot < 1f)
        {
            counterRot += Time.deltaTime / transitionDuration;
            change.y += (Time.deltaTime / transitionDuration)*180;
            pivot.eulerAngles = change;

            yield return null;
        }
        init.y += 180f;
        pivot.eulerAngles = init;
    }
}
