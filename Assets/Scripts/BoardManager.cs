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

        public float x, y;

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

    }

    public void SetBoard (Vector2 size, int sizeX, int sizeY, float posX, float posY)
    {
        // Get a random number to decide which block is going to be used
        int color = Random.Range(1, 8);

        GameObject colour = Resources.Load(path + color) as GameObject;

        brd.board = new GameObject[sizeX, sizeY];
        brd.size = size;
        brd.x = posX;
        brd.y = posY;

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {

                Vector3 position = new Vector3(((transform.position.x  - (sizeX/2)) + 0.5f) + i, (transform.position.y  - (sizeY/2)) + j, -1);

                Debug.Log(position);

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
    }

    void BoardEscalate()
    {

    }
}
