using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverCuboLerp : MonoBehaviour
{

    public Vector3 A;
    public Transform B;

    Vector3 ACopy;
    Transform BCopy;

    float t;

    public float tiempoEnCompletar;
    private float factorMovimiento;

    // Start is called before the first frame update
    void Start()
    {
        factorMovimiento = 1f / tiempoEnCompletar;
        A = transform.position;
        t = 0f;
        ACopy = A;
        BCopy = B;
    }

    // Update is called once per frame
    void Update()
    {
        if (B == BCopy)
        {
            if (t < 1f)
            {
                t += factorMovimiento * Time.deltaTime;
                if (t > 1f)
                {
                    t = 1f;
                }
                transform.position = Vector3.Lerp(A, B.position, t);
            }
        }
        else
        {
            A = transform.position;
            ACopy = A;
            BCopy = B;
            t = 0;
        }
    }
}
