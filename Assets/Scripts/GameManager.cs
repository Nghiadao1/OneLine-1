using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Welcome to the GameManager script, enjoy the visit and left some comments below. 

    BoardManager boardManager;

    // Start is called before the first frame update
    void Start()
    {
        //Llamar primero a Escalate
        boardManager.SetBoundings(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScreenClicked(float x, float y)
    {
        


    }

    void Escalate ()
    {
        //Params -> Rect src, Rect dim
        
            //Rect temp; // Temporal rectangle for calculations

            //int width = src.getWidth(); // Save the src width
            //int height = src.getHeight(); // Save the src height

            //// If the src width is higher than the reference width
            //if (width > dim.getWidth())
            //{
            //    // Set the new width but resized proportionally
            //    width = repositionX(width);
            //    // Change height keeping proportions
            //    height = (width * src.getHeight()) / src.getWidth();
            //} // if

            //// If the src height (or the changed height) is bigger than the reference one
            //if (height > dim.getHeight())
            //{
            //    // Set the new height but resized proportionally
            //    height = repositionY(height);
            //    // Change width proportionally
            //    width = (height * src.getWidth()) / src.getHeight();
            //} // if

            //// Save the changes to the new Rectangle
            //temp = new Rect(width, 0, 0, height);

            //// Set the original position in canvas of the source Rectangle
            //temp.setPosition(src.getX(), src.getY());

            //// Return result
            //return temp;

    }

   
    //public int repositionX(int x)
    //{
    //    return (x * _can.getWidth()) / _refCan.getWidth();
    //} // repositionX


    //public int repositionY(int y)
    //{
    //    return (y * _can.getHeight()) / _refCan.getHeight();
    //} // repositionY


}
