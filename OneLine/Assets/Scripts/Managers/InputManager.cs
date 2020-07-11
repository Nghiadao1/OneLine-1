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


        if (Input.GetMouseButton(0))
        {
            GameManager.GetInstance().ScreenTouched(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            GameManager.GetInstance().ScreenReleased();
        }
    }
}
