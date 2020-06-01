using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    public static Vector2 fingerDir;

    Vector2 startTouch,endTouch;

    public Image backtouch, fingeratach;

    Color alphaChange;
    bool isTouch;
    // Start is called before the first frame update
    void Start()
    {
        alphaChange = Color.white;
        alphaChange.a = 0f;
        fingerDir = Vector2.zero;
        backtouch.color=alphaChange;
        fingeratach.color=alphaChange;
    }

    // Update is called once per frame
    void Update()
    {
        TapCalculation();

        if (isTouch && alphaChange.a<1f)
        {
            alphaChange.a += Time.deltaTime;
            backtouch.color = alphaChange;
            fingeratach.color = alphaChange;
        }
        else if(alphaChange.a>0f)
        {
            alphaChange.a -= Time.deltaTime;
            backtouch.color = alphaChange;
            fingeratach.color = alphaChange;
        }
    }

    private void TapCalculation()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                isTouch = true;
                startTouch = Input.touches[0].position;
                backtouch.transform.position = startTouch;
            }
            else if (Input.touches[0].phase == TouchPhase.Moved)
            {
                endTouch = Input.touches[0].position;
                Vector2 dir = endTouch - startTouch;
                float x = (dir.x / Screen.width) * 2f;
                float y = (dir.y / Screen.height) * 5f;
                fingerDir.x = (Mathf.Abs(x)>1)? Mathf.Abs(x)/x: x;
                fingerDir.y = (Mathf.Abs(y) > 1) ? Mathf.Abs(y) / y : y;

                fingeratach.transform.position = endTouch;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended)
            {
                isTouch = false;
                fingerDir = Vector2.zero;
            }
        }
    }
}
