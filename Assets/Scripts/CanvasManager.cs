using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    Vector2 scaleRef;       //Medidas del canvas de referencia

    //Actual canvas size (The screen width and height)
    // This will save the free size for the game
    float _width;
    float _height;

    // Paneles
    public RectTransform canvas; 
    public RectTransform panelUp;
    public RectTransform panelDown; 

    void Awake()
    {
        scaleRef = GetComponent<CanvasScaler>().referenceResolution;

        Debug.Log(scaleRef);

        // Devolvemos el valor del ancho y el alto
        _width = canvas.rect.width;

        _height = (canvas.rect.height - (panelUp.rect.height + panelDown.rect.height));

        
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
