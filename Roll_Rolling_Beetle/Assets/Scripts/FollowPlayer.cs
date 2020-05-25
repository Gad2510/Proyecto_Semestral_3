using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Animations;

public class FollowPlayer : MonoBehaviour
{
    bool inverseRot;// Bool para saber si se rota la camara
    public float transitionDuration = 0.5f; // Duracion del cambio de camara

    Transform pivot;
    ParentConstraint constrain;
    public Transform Pivot
    {
        set { pivot = value;
            ConstraintSource sr = new ConstraintSource();
            sr.sourceTransform = pivot;
            sr.weight = 1;
            constrain.SetSource(0, sr);
        }
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

    private void Start()
    {
        constrain = GetComponent<ParentConstraint>();
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
