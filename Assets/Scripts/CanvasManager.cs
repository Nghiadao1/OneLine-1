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
    RectTransform completed;

    // Texts 
    public Text titleText;
    public Text coins;
    public Text endText;

    void Awake()
    {
        canvas = this.gameObject.GetComponent<RectTransform>();
        panelUp = this.gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        panelDown = this.gameObject.transform.GetChild(1).GetComponent<RectTransform>();
        if (GameManager.instance.IsInLevel())
        {
            completed = this.gameObject.transform.GetChild(2).GetComponent<RectTransform>();
            completed.gameObject.SetActive(false);
        }

        scaleRef = GetComponent<CanvasScaler>().referenceResolution;

        // Devolvemos el valor del ancho y el alto
        _width = canvas.rect.width - ((canvas.rect.width * 40) / scaleRef.x); // Quitamos 80 porque puntitos

        _height = (canvas.rect.height - (panelUp.rect.height + panelDown.rect.height));

        SetCoinsNum(GameManager.instance.GetCoins());
    }

    public void SetTitleText(string title)
    {
        titleText.text = title;
    }

    public void SetEndText(string end)
    {
        endText.text = end;
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

    public void LevelCompleted()
    {
        completed.gameObject.SetActive(true);
    }

}
