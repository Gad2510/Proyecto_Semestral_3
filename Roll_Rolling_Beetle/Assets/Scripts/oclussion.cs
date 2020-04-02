using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oclussion : MonoBehaviour
{
    Renderer[] renderers;
    bool Fisrt = true;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateMeshes",0.1f,0.1f);
        
    }

    void OutputVisibleRenderers(Renderer[] renderers)
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = IsVisible(renderer);
        }
    }

    private bool IsVisible(Renderer renderer)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    private void UpdateMeshes()
    {
        if (Fisrt)
        {
            renderers = FindObjectsOfType<Renderer>();
            Fisrt = false;
        }

        OutputVisibleRenderers(renderers);
    }

    
}
