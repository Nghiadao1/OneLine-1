using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    public void CheckInput()
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            // Finger down
            if(t.phase == TouchPhase.Began || t.phase == TouchPhase.Moved)
            {
                GameManager.GetInstance().ScreenTouched(t.position);
            }
            else if(t.phase == TouchPhase.Ended)
            {
                GameManager.GetInstance().ScreenReleased();
            }
        }     
#else
        if (Input.GetMouseButton(0))
        {
            GameManager.GetInstance().ScreenTouched(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            GameManager.GetInstance().ScreenReleased();
        }
#endif
    }
}
