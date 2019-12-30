using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    // Private atributes
    bool _pressed;

    // Sprites needed for representation
    GameObject colorSpr;
    GameObject baseSpr; 

    // Calculos 
    Vector2 rt;

    // Start is called before the first frame update
    void Start()
    {
        baseSpr = transform.GetChild(0).gameObject;

        rt = baseSpr.transform.localScale;
        
        _pressed = false;
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

    public void SetColor(GameObject c)
    {
        colorSpr = c;

        colorSpr.SetActive(false);
    }
}
