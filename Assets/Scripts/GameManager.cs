using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Welcome to the GameManager script, enjoy the visit and left some comments below. 

    public BoardManager bm;

    public GameObject background;
    public CanvasManager canvas;

    Vector2 _refResolution;       //Reference resolution
    float _cameraWidth;               // Available size
    float _cameraHeight;              // Available size

    // Start is called before the first frame update
    void Start()
    {
        //Llamar primero a Escalate
        _refResolution = canvas.GetReferenceResolution();

        _cameraHeight = Camera.main.orthographicSize * 2;
        _cameraWidth = _cameraHeight * Screen.width / Screen.height;

        Sprite bgSprite = background.GetComponent<SpriteRenderer>().sprite;

        background.transform.localScale = Escalate(bgSprite.rect.width, bgSprite.rect.height, background.transform.localScale);

        bm.SetBoard(new Vector2(canvas.GetWidth(), canvas.GetHeight()), new Vector2(_cameraWidth, _cameraHeight),  6, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 Escalate (float spriteWidth, float spriteHeight, Vector2 prevScale)
    {
        float actualWidth = spriteWidth;
        float actualHeight = spriteHeight;

        // If the src width is higher than the reference width
        if (actualWidth > _cameraWidth)
        {
            // Set the new width but resized proportionally
            actualWidth = repositionX(actualWidth);
            // Change height keeping proportions
            actualHeight = (actualWidth * spriteHeight) / spriteWidth;
        } // if
       
        // Transform pixels to unity units
        actualHeight = (actualHeight * prevScale.x) / (spriteHeight / 100);
        actualWidth = (actualWidth * prevScale.y)  / (spriteWidth / 100);

        // Save the changes to the new Rectangle
        Vector3 temp = new Vector3(actualWidth, actualHeight, 0);

        // Return result
        return temp;

    }

    public float repositionX(float x)
    {
        return (x * _cameraWidth) / _refResolution.x;
    } // repositionX


    public float repositionY(float y)
    {
        return (y * _cameraHeight) / _refResolution.y;
    } // repositionY


}
