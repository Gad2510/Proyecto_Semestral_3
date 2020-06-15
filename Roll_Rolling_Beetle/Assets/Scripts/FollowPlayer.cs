using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class FollowPlayer : MonoBehaviour
{
    public LayerMask layer;
    MeshRenderer visual=null;
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
    private void Update()
    {
        Ray rayo = new Ray(this.transform.position, this.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(rayo,out hit ,10f,layer))
        {
            visual = hit.transform.GetComponent<MeshRenderer>();
            if (visual != null)
                visual.enabled = false;
        }
        else if (visual != null)
        {
            visual.enabled = true;
            visual = null;
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

    private void OnDrawGizmos()
    {
        Ray rayo = new Ray(this.transform.position, this.transform.forward);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rayo);
    }
}
