using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    public static Vector2 fingerDir; //Vector de movimiento para el jugador
    PauseMenu pause;//Componente de pausa
    Vector2 startTouch,endTouch;//Posiciones del touch

    public Image backtouch,area, fingeratach; //Imagenes del stick

    Color alphaChange;
    bool isTouch, outOfRange;
    // Start is called before the first frame update
    void Start()
    {
        alphaChange = Color.white; //Crea el color base
        alphaChange.a = 0f;
        fingerDir = Vector2.zero;
        backtouch.color=alphaChange;
        area.color = alphaChange;
        fingeratach.color=alphaChange;

        pause=GetComponent<PauseMenu>();//Obtiene referencia a la pausa
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause.GameIsPaused)
        {
            TapCalculation(); // Calcula el touch del jugador

            if (isTouch && alphaChange.a < 1f)//Si esta tocando la pantalla aumenta su alpha 
            {
                alphaChange.a += Time.deltaTime;
            }
            else if (alphaChange.a > 0f) //Mientras el alpha no sea menor a 0
            {
                alphaChange.a -= Time.deltaTime;
            }
        }
        else //Si esta en pausa el juego no se ven los del touch
        {
            alphaChange.a = 0;
        }
        backtouch.color = alphaChange;
        area.color = alphaChange;
        fingeratach.color = alphaChange;

    }

    private void TapCalculation()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)//Cuando empieza el touch
            {
                startTouch = Input.touches[0].position; //Guarda referencia a la posicion tocada
                outOfRange = startTouch.y < Screen.height / 8;
                if (outOfRange)
                {
                    return;
                }
                isTouch = true;
                
                backtouch.transform.position = startTouch; //Coloca la forma en posicion
            }
            else if (Input.touches[0].phase == TouchPhase.Moved)//Cuando mueva el dedo
            {
                if (outOfRange)
                {
                    return;
                }
                endTouch = Input.touches[0].position; // Guarda referencia a la ultima posicion

                Vector2 dir = endTouch - startTouch;//Calcula la direccion
                float x = (dir.x / Screen.width) * 40f; //Lotransforma a terminos de la pantalla
                float y = (dir.y / Screen.height) * 5f;
                fingerDir.x = (Mathf.Abs(x)>1)? Mathf.Abs(x)/x: x * (Mathf.Abs(y) / y);//Verifica que su valor absoluto no sea mayor a 1 si si es igual a 1 en su direccion
                fingerDir.y = (Mathf.Abs(y) > 1) ? Mathf.Abs(y) / y : y;

                

                if(Vector2.Distance(startTouch, endTouch)>=100f) //Distancia del area del stick
                {
                    fingeratach.transform.position=(dir.normalized*100f)+startTouch;//Direccion del touch por distancia maxima mas posicion inical del touch
                }
                else
                {
                    fingeratach.transform.position = endTouch;//Ultima posicion del touch
                }

            }
            else if (Input.touches[0].phase == TouchPhase.Ended) //Cuando deja de tocar la pantalla
            {
                isTouch = false;
                fingerDir = Vector2.zero;
            }
        }
    }
}
