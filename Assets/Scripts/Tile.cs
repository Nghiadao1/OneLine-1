using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    bool _pressed;

    GameObject colorSpr;

 

    // Start is called before the first frame update
    void Start()
    {
        _pressed = false;

        colorSpr.SetActive(false);
    }

    public void setPressed(bool b)
    {
        _pressed = b;
        colorSpr.SetActive(_pressed);
    }

    public bool getPressed()
    {
        return _pressed;
    }
}
