using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cordanates : MonoBehaviour
{
    Material groundMaterial;
    public Vector4 cordenadas;

    // Start is called before the first frame update
    void Start()
    {
        groundMaterial = GetComponent<MeshRenderer>().material;
        groundMaterial.SetVector("_Cord", cordenadas);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
