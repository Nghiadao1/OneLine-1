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
    RectTransform canvas; 
    RectTransform panelUp;
    RectTransform panelDown;

    // Texts 
    public Text titleText;
    public Text coins;

    void Awake()
    {
        canvas = this.gameObject.GetComponent<RectTransform>();
        panelUp = this.gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        panelDown = this.gameObject.transform.GetChild(1).GetComponent<RectTransform>();


        scaleRef = GetComponent<CanvasScaler>().referenceResolution;

        // Devolvemos el valor del ancho y el alto
        _width = canvas.rect.width - ((canvas.rect.width * 40) / scaleRef.x); // Quitamos 80 porque puntitos

        _height = (canvas.rect.height - (panelUp.rect.height + panelDown.rect.height));        
    }

    public void SetTitleText(string title)
    {
        titleText.text = title;
    }

    public void SetCoinsNum(int coinsNum)
    {
        
        coins.text = coinsNum.ToString();
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

    public float GetPanelUpHeight ()
    {
        return panelUp.rect.height;
    }

    public float GetPanelDownHeight()
    {
        return panelDown.rect.height;
    }

}
