using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading_manager : MonoBehaviour
{
    static float loading = 0f;
    static int leanId;

    public Shader camShader;
    public Texture2D mask;

    bool isUnloading = false;
    Material camMat;

    public static float Loading
    {
        set
        {
            loading = value;
            if(loading>=0.95f)
            {
                LeanTween.resume(leanId);
            }
        }
    }

    private void Awake()
    {
        camMat = new Material(camShader);
        camMat.SetFloat("_Alpha", 0);
        LeanTween.value(gameObject,CameraTransition, 0f, 1f, 1f).setOnComplete(LoadNextLevel);
    }

    void CameraTransition(float _value)
    {
        camMat.SetFloat("_Alpha", _value);
    }

    void CameraT(float _value)
    {
        camMat.SetFloat("_Alpha", _value);
    }

    private void LoadNextLevel()
    {
        Scene_Manager_BH._instance.loadLevelInLine();
        leanId = LeanTween.value(gameObject, CameraT, 1f, 0f, 1f).setOnComplete(UnloadScene).id;
        LeanTween.pause(leanId);
    }

    private static void UnloadScene()
    {
        Scene_Manager_BH._instance.UnloadLoding();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, camMat);
    }
}
