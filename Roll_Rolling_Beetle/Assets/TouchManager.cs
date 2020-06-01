using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    public static Vector2 fingerDir;

    Vector2 startTouch,endTouch;

    public Image backtouch, fingeratach;

    // Start is called before the first frame update
    void Start()
    {
        fingerDir = Vector2.zero;
        backtouch.SetActive(false);
        fingeratach.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        TapCalculation();
    }

    private void TapCalculation()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Moved)
            {
                endTouch = Input.touches[0].position;
                Vector2 dir = endTouch - startTouch;
                float x = (dir.x / Screen.width) * 2f;
                float y = (dir.y / Screen.height) * 5f;
                fingerDir.x = (Mathf.Abs(x)>1)? Mathf.Abs(x)/x: x;
                fingerDir.y = (Mathf.Abs(y) > 1) ? Mathf.Abs(y) / y : y;

            }
            else if (Input.touches[0].phase == TouchPhase.Ended)
            {
                fingerDir = Vector2.zero;
            }
        }
    }
}
