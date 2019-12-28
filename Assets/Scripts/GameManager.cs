using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Welcome to the GameManager script, enjoy the visit and left some comments below. 

    BoardManager boardManager;

    public GameObject background;
    public CanvasManager canvas;

    Vector2 _refResolution;       //Reference resolution
    float _width;               //Screen width
    float _height;              //Screen height

    // Start is called before the first frame update
    void Start()
    {
        //Llamar primero a Escalate
        boardManager.SetBoundings(0, 0, 0, 0);

        _refResolution = canvas.GetReferenceResolution();

        _width = canvas.GetWidth();
        _height = canvas.GetHeight();

        Sprite bgSprite = background.GetComponent<Sprite>();

        background.transform.localScale = Escalate(bgSprite.rect.width, bgSprite.rect.height);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScreenClicked(float x, float y)
    {
        


    }

    Vector3 Escalate (float spriteWidth, float spriteHeight)
    {
        float actualWidth = spriteWidth;
        float actualHeight = spriteHeight;

        // If the src width is higher than the reference width
        if (actualWidth > _width)
        {
            // Set the new width but resized proportionally
            actualWidth = repositionX(actualWidth);
            // Change height keeping proportions
            actualHeight = (actualWidth * spriteHeight) / spriteWidth;
        } // if

        // If the src height (or the changed height) is bigger than the reference one
        if (actualHeight > _height)
        {
            // Set the new height but resized proportionally
            actualHeight = repositionY(actualHeight);
            // Change width proportionally
            actualWidth = (actualHeight * spriteWidth) / spriteHeight;
        } // if

        // Save the changes to the new Rectangle
        Vector3 temp = new Vector3(actualWidth, actualHeight, 0);

        // Return result
        return temp;

    }


    public float repositionX(float x)
    {
        return (x * _width) / _refResolution.x;
    } // repositionX


    public float repositionY(float y)
    {
        return (y * _height) / _refResolution.y;
    } // repositionY


}
