using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public GameObject tilePrefab;

    // Private atributes

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
        brd.board = new GameObject[sizeX, sizeY];
        brd.size = size;
        brd.x = posX;
        brd.y = posY;

        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                Debug.Log(tilePrefab);
                brd.board[i, j] = Instantiate(tilePrefab);
                brd.board[i, j].transform.SetParent(transform);
                brd.board[i, j].transform.position = new Vector3(transform.position.x * j, transform.position.y * i);

            }
        }
    }
}
