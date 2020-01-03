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
    GameObject pathSpr;

    // Calculos 
    Vector2 rt;
    Vector2 posInBoard;
    int id; // Stores position of this tile in the board

    // Start is called before the first frame update
    void Start()
    {
        baseSpr = transform.GetChild(0).gameObject;

        rt = baseSpr.transform.localScale;
    }

    public void SetPressed(bool b, float degrees)
    {
        _pressed = b;
        colorSpr.SetActive(_pressed);
        CreatePath(degrees, _pressed);
    }

    public void CreatePath(float degrees, bool activate)
    {
        pathSpr.transform.Rotate(new Vector3(0, 0, degrees));
        pathSpr.SetActive(activate);
    }

    public bool GetPressed()
    {
        return _pressed;
    }

    public int GetID()
    {
        return id;
    }

    public void SetColor(GameObject c)
    {
        colorSpr = c;

        colorSpr.SetActive(false);
    }

    public void SetPosBoard(int i, int j)
    {
        posInBoard = new Vector2(i, j);
    }

    public void SetID(int i)
    {
        id = i;
    }

    public void SetPathSpr(GameObject p)
    {
        pathSpr = p;
        p.SetActive(false);
    }

    public void OnClick()
    {
        if (transform.parent.GetComponent<BoardManager>())
        {
            float degrees = 0.0f;
            if (transform.parent.GetComponent<BoardManager>().TileClicked(id, posInBoard, _pressed, ref degrees) && !_pressed)
            {
                SetPressed(true, degrees);
            }
        }
    }
}
