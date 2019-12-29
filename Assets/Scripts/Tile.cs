using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    // Private atributes
    bool _pressed;

    public GameObject colorSpr;
    RectTransform rt;

    float width;
    float height; 

 

    // Start is called before the first frame update
    void Start()
    {
        rt = (RectTransform)colorSpr.transform;

        width = rt.rect.width;
        height = rt.rect.height;

        _pressed = false;

        colorSpr.SetActive(false);
    }

    public void SetPressed(bool b)
    {
        _pressed = b;
        colorSpr.SetActive(_pressed);
    }

    public bool GetPressed()
    {
        return _pressed;
    }

    public float GetWidth()
    {
        return width;
    }

    public float GetHeight()
    {
        return height;
    }

    public void SetColor()
    {

    }


}
