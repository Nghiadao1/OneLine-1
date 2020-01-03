using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject tilePrefab;

    // Private atributes
    string path = "Prefabs/Game/Skins/Blocks/block_0";

    // Board 
    struct Board
    {
        public GameObject[,] board;

        public float sizeX, sizeY;

        public Vector2 size;
    }

    Board brd; // GameBoard of tiles


    /**
    * Receives a possition were the screen is clicked.
    * 
    * @param x int X position of the cursor
    * @param y int Y position of the cursor
    */
    public void BoardClicked(float x, float y)
    {
        //Bucle para revisar qué tile ha sido pulsado
        for (int i = 0; i < brd.sizeX; i++)
        {
            for (int j = 0; j < brd.sizeY; j++)
            {
                //if (x < brd.board[i, j])
            }
        }
    }

    public void SetBoard (Vector2 size, Vector2 camSize, int sizeX, int sizeY)
    {

        // Get a random number to decide which block is going to be used
        int color = Random.Range(1, 8);

        GameObject colour = Resources.Load(path + color) as GameObject;

        Vector2 scale = transform.localScale;

        brd.board = new GameObject[sizeX, sizeY];
        brd.size = DefinePlayZone(size, sizeX, sizeY, ref scale);
        brd.sizeX = sizeX;
        brd.sizeY = sizeY;

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {

                Vector3 position = new Vector3(((transform.position.x  - (sizeX/2)) + 0.5f) + i, (transform.position.y  - (sizeY/2))  + j, -1);
                
                // Instantiate GameObjects needed 
                brd.board[i, j] = Instantiate(tilePrefab, position, Quaternion.identity); // Position
                GameObject colorSprite = Instantiate(colour, position, Quaternion.identity);

                // Attacht them to parents
                brd.board[i, j].transform.SetParent(transform);
                colorSprite.transform.SetParent(brd.board[i, j].transform);

                // Set the color sprite to Tile
                brd.board[i, j].GetComponent<Tile>().SetColor(colorSprite);

            }
        }

        Debug.Log(brd.size);

        // Escalate all board
        transform.localScale = BoardEscalate(brd.size, camSize, scale);
    }

    Vector2 DefinePlayZone(Vector2 size, int x, int y, ref Vector2 scale)
    { // TODOS LOS CALCULOS EN PIXELES
        float width = 0.0f;
        float height = 0.0f;

        float defWidth;
        float defHeight;

        if(y <= 5)
        {
            // Definimos espacio para 6x5
            width = 6 * 120;
            height = 5 * 120;
        }
        else if (y > 5)
        {
            width = 6 * 120;
            height = 8 * 120;
        }
        

        defWidth = width;
        defHeight = height;

        // Recolocar
        if (defWidth > size.x)
        {
            defWidth = (size.x * width) / 720f;

            defHeight = (defWidth * height) / width;
        }

        if(defHeight > size.y)
        {
            defHeight = (size.y * height) / 1280f;

            defWidth = (defHeight * width) / height;
        }

        scale.x = (scale.x * defWidth) / width;
        scale.y = (scale.y * defHeight) / height;

        return new Vector2(defWidth, defHeight);
    }

    Vector3 BoardEscalate(Vector2 dim, Vector2 cam, Vector2 prevScale)
    {
        // Transform width and height to unity units
        float unityWidth = dim.x;
        float unityHeight = dim.y;

        float notAvailable = cam.y - unityHeight;

        //float scaleX = prevScale.x;
        //float scaleY = prevScale.y;

        //if (unityWidth > cam.x)
        //{
        //    // Set the new width but resized proportionally
        //    scaleX = (prevScale.x * cam.x) / unityWidth;

        //    // Change height keeping proportions
        //    scaleY = scaleX;
        //}

        //if (unityHeight > cam.y - notAvailable)
        //{
        //    // Set the new width but resized proportionally
        //    scaleY = (prevScale.y * (cam.y - notAvailable)) / unityHeight;

        //    // Change height keeping proportions
        //    scaleX = scaleY;
        //}

        //return new Vector3(scaleX, scaleY, 0);

        if (unityWidth > cam.x)
        {
            // Set the new width but resized proportionally
            unityWidth = (unityWidth * cam.x) / 720;

            // Change height keeping proportions
           unityHeight = (unityWidth * dim.y) / dim.x;
        } // if

        if (unityHeight > cam.y - notAvailable)
        {
            // Set the new width but resized proportionally
            unityHeight = (unityHeight * cam.y) / 1280;

            // Change height keeping proportions
            unityWidth = (unityHeight * dim.x) / dim.y;
        } // if

        // Transform pixels to unity units
        unityHeight = (unityHeight * prevScale.x) / (dim.y / 100);
        unityWidth = (unityWidth * prevScale.y) / (dim.x / 100);

        // Save the changes to the new Rectangle
        Vector3 temp = new Vector3(unityWidth, unityHeight, 0);

        // Return result
        return temp;
    }

    public Vector2 GetSize ()
    {
        return brd.size;
    }
}