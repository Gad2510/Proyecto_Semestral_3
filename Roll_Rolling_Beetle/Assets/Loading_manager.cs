using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading_manager : MonoBehaviour
{
    public Transform pivotMovement;
    public RectTransform endPoint;

    public UnityEngine.UI.Slider loadBar;

    
    Material camMat;
    public Shader camShader;
    public Texture2D mask;
    [Range(0,1)]
    public float color;
    private void Awake()
    {

        camMat = new Material(camShader);
        camMat.SetTexture("_Mask", mask);
        camMat.SetFloat("_Alpha", 0);
        camMat.SetTexture("_SecTex", Scene_Manager_BH._instance.LastFrame);
        LeanTween.value(gameObject,CameraTransition, 0f, 1f, 1f);
    }

    void CameraTransition(float _value)
    {
        camMat.SetFloat("_Alpha", _value);
    }

    // Update is called once per frame
    void Update()
    {
        if(Scene_Manager_BH._instance.CurrentLoad != null)
            loadBar.value = Scene_Manager_BH._instance.CurrentLoad.progress;

        Vector3 worldPos = endPoint.position;
        worldPos.z = pivotMovement.position.z;
        worldPos.y = pivotMovement.position.y;

        pivotMovement.position = worldPos;

        if ( Scene_Manager_BH._instance.CurrentLoad == null || loadBar.value > 0.9)
        {
            Scene_Manager_BH._instance.UnloadLoding();
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, camMat);
    }
}
