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
    GameObject pathPivot;
    GameObject hintPivot;

    Quaternion origRotPath;
    Quaternion origRotHint;

    // Position in the board
    Vector2 brdPos;

    public void SetTile(GameObject baseSprite, GameObject colorSprite, GameObject plPivot, GameObject hnPivot, Vector2 pos)
    {
        baseSpr = baseSprite;
        colorSpr = colorSprite;
        pathPivot = plPivot;
        hintPivot = hnPivot;

        origRotPath = pathPivot.transform.rotation;
        origRotHint = hintPivot.transform.rotation;

        colorSpr.SetActive(false);
        pathPivot.SetActive(false);
        hintPivot.SetActive(false);

        brdPos = pos;
    }

    public void ActivateColor()
    {
        colorSpr.SetActive(true);
    }

    public void RotatePlayerPath(float rotation)
    {
        pathPivot.transform.Rotate(new Vector3(0, 0, rotation));
        pathPivot.SetActive(true);
    }

    public void RotateHintPath(float rotation)
    {
        hintPivot.SetActive(true);
    }

    public void ResetTile()
    {
        pathPivot.transform.rotation = origRotPath;
        hintPivot.transform.rotation = origRotHint;

        colorSpr.SetActive(false);
        pathPivot.SetActive(false);
        hintPivot.SetActive(false);
    }

    public Vector2 getPositionInBoard()
    {
        return brdPos;
    }
}
