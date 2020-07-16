using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    // Sprites needed for changing and showing information
    GameObject _colorSpr;
    GameObject _pathPivot;
    GameObject _hintPivot;

    // Rotations needed to set each player path direction 
    Quaternion _origRotPath;

    // Position in the board
    Vector2 _brdPos;

    /// <summary>
    /// Sets all the values of the tile and deactivates the hyde objects until is pressed
    /// </summary>
    /// <param name="colorSprite">Random color of the tile when is pressed</param>
    /// <param name="plPivot">Object used like a pivot to rotate the player path</param>
    /// <param name="hnPivot">Object used like a pivot to rotate the hints path</param>
    /// <param name="pos">Board position of the tile</param>
    public void SetTile(GameObject colorSprite, GameObject plPivot, GameObject hnPivot, Vector2 pos)
    {
        _colorSpr = colorSprite;
        _pathPivot = plPivot;
        _hintPivot = hnPivot;

        _origRotPath = _pathPivot.transform.rotation;

        _colorSpr.SetActive(false);
        _pathPivot.SetActive(false);
        _hintPivot.SetActive(false);

        _brdPos = pos;
    }

    /// <summary>
    /// Set active the color of the tile when is pressed
    /// </summary>
    public void ActivateColor()
    {
        _colorSpr.SetActive(true);
    }

    /// <summary>
    /// Rotates the player path when the tile is pressed to the previous tile and set it active
    /// </summary>
    /// <param name="rotation">Degrees needed to rotate the path</param>
    public void RotatePlayerPath(float rotation)
    {
        _pathPivot.transform.Rotate(new Vector3(0, 0, rotation));
        _pathPivot.SetActive(true);
    }

    /// <summary>
    /// Rotates the hint path to the previous tile of the right path and set it active
    /// </summary>
    /// <param name="rotation">Degrees needed to rotate the path</param>
    public void RotateHintPath(float rotation)
    {
        _hintPivot.transform.Rotate(new Vector3(0, 0, rotation));
        _hintPivot.SetActive(true);
    }

    /// <summary>
    /// Set the tile to its original situation (not pressed): without color and path
    /// </summary>
    public void ResetTile()
    {
        // The path pivot is set to the original rotation for simplify the next rotation operations
        _pathPivot.transform.rotation = _origRotPath;

        _colorSpr.SetActive(false);
        _pathPivot.SetActive(false);
    }

    /// <summary>
    /// Get the tile position in the board 
    /// </summary>
    /// <returns></returns>
    public Vector2 getPositionInBoard()
    {
        return _brdPos;
    }
}
