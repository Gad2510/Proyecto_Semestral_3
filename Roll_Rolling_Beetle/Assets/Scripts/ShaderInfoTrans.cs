using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderInfoTrans : MonoBehaviour
{
    public Material grassEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        grassEffect.SetVector("_PlayerPosition", this.transform.position);
    }
}
