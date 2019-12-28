using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    Vector2 scaleRef;       //Medidas del canvas de referencia

    //Actual canvas size (The screen width and height)
    float _width;
    float _height;

    void Awake()
    {
        scaleRef = GetComponent<CanvasScaler>().referenceResolution;
        _width = Screen.width;
        _height = Screen.height;
        
    }

  //Getter of the screen width
  public float GetWidth()
    {
        return _width;
    }

    //Getter of the screen height
    public float GetHeight()
    {
        return _height;
    }

    //Getter of reference resolution
    public Vector2 GetReferenceResolution()
    {
        return scaleRef;
    }

}
