using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class apuntar : MonoBehaviour
{
    GameObject objetToPoint;
    public Camera cam;

    public string tagObject;

    private void Start()
    {
        objetToPoint = GameObject.FindGameObjectWithTag(tagObject); //Buscamos la Casa por su etiqueta
    }

    void Update()
    {
        if (objetToPoint != null)
        {
            MovePointerHome();
        }
    }

    void MovePointerHome()
    {
        Vector3 screenObj = cam.WorldToScreenPoint(objetToPoint.transform.position);
        if (Vector3.Distance(transform.position, screenObj) > 15)
        {
            float distance = Vector3.Distance(cam.transform.position, objetToPoint.transform.transform.position);
            Vector3 forward = cam.transform.forward;
            Vector3 objPos = (objetToPoint.transform.position - cam.transform.position).normalized;
            float dot = Vector3.Dot(objPos, forward);

            float zRotation = (Mathf.Atan2(screenObj.y - transform.position.y, screenObj.x - transform.position.x) * Mathf.Rad2Deg);

            if (dot < 0.0f)//Checa si el objeto esta fuera de pantalla y corrige la ubicacion
            {
                transform.eulerAngles = new Vector3(0, 0, zRotation - 90);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, zRotation + 90);
            }
        }
    }

    public void searchnewpoop()
    {
        objetToPoint = GameObject.FindGameObjectWithTag(tagObject);
    }
}
