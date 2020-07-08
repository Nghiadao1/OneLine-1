using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //
    private bool pressed = false;

    // Sprites needed for changing and showing information
    GameObject baseSpr;
    GameObject colorSpr;
    GameObject pathSpr;
    GameObject hintSpr;

    // Position in the board
    Vector2 brdPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTile(GameObject baseSprite, GameObject colorSprite, GameObject pathSprite, GameObject hintSprite, Vector2 pos)
    {
        baseSpr = baseSprite;
        colorSpr = colorSprite;
        pathSpr = pathSprite;
        hintSpr = hintSprite;

        colorSpr.SetActive(false);
        pathSpr.SetActive(false);
        hintSpr.SetActive(false);

        brdPos = pos;
    }
}
