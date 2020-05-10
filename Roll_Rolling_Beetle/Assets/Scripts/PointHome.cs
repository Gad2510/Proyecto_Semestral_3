using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointHome : MonoBehaviour
{
    public GameObject Home;
    public Camera cam;

    private void Start()
    {
        Home = GameObject.FindGameObjectWithTag("home"); //Buscamos la Casa por su etiqueta
    }

    void Update()
    {
        MovePointerHome();
    }

    void MovePointerHome()
    {
        Vector3 screenObj = cam.WorldToScreenPoint(Home.transform.position);
        if (Vector3.Distance(transform.position, screenObj) > 15)
        {
            
            float distance = Vector3.Distance(cam.transform.position, Home.transform.transform.position);
            Vector2 forward = new Vector2(cam.transform.forward.x, cam.transform.forward.z);
            Vector2 objPos = new Vector2(Home.transform.transform.position.x - cam.transform.position.x, Home.transform.transform.position.z - cam.transform.position.z) / distance;
            float dot = Vector2.Dot(objPos, forward);
            float anglepos = Mathf.Acos(dot) * Mathf.Rad2Deg;


            float zRotation = (Mathf.Atan2(screenObj.y - transform.position.y, screenObj.x - transform.position.x) * Mathf.Rad2Deg);

            if (anglepos < 90.0f)
            {
                transform.eulerAngles = new Vector3(0, 0, zRotation + 90);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, zRotation - 90);
            }
        }
    }

}
