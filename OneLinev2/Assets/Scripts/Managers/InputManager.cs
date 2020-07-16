using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class cheks if the game receives some kind of input
/// Differentiates between a mouse input when is load in the editor
/// and touch input when is load in an android or ios system
/// </summary>
public class InputManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    /// <summary>
    /// Checks if the game receives an input information and notify the game manager with it
    /// </summary>
    public void CheckInput()
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        // If there are one or more touch inputs in the screen
        if (Input.touchCount > 0)
        {
            // Keeps the touch input
            Touch t = Input.GetTouch(0);

            // Finger down
            if(t.phase == TouchPhase.Began || t.phase == TouchPhase.Moved)
            {
                // Notify the game manager with the finger down input
                GameManager.GetInstance().ScreenTouched(t.position);
            }
            // Finger up
            else if(t.phase == TouchPhase.Ended)
            {
                // Notify the game manager with the end of the finger down input
                GameManager.GetInstance().ScreenReleased();
            }
        }     
#else
        // If there is a left click input
        if (Input.GetMouseButton(0))
        {
            // Notify the game manager with the click input
            GameManager.GetInstance().ScreenTouched(Input.mousePosition);
        }
        // Or if there is a left click up input
        else if (Input.GetMouseButtonUp(0))
        {
            // Notify the game manager with the end of the click input
            GameManager.GetInstance().ScreenReleased();
        }
#endif
    }
}
