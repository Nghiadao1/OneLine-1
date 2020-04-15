using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escalate
{
    Vector2 _refResolution;
    float _cameraWidth;
    float _cameraHeight;

    public Escalate(float camWidth, float camHeight, Vector2 refRes)
    {
        _refResolution = refRes;
        _cameraHeight = camHeight;
        _cameraWidth = camWidth;
    }

    public Vector3 EscaleToCamWidth(float spriteWidth, float spriteHeight, Vector2 prevScale)
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
        actualWidth = (actualWidth * prevScale.y) / (spriteWidth / 100);

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
