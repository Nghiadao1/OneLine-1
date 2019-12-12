using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // Private atributes

    // Board 

    struct Board
    {
        public Tile[,] board;

        public int width, height;
    }

    Board board; // GameBoard of tiles

    // Board information
    float avX; // This atributes will store the available size in pixels
    float avY; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * Receives a possition were the screen is clicked.
     * 
     * @param x int X position of the cursor
     * @param y int Y position of the cursor
     */
    public void BoardClicked(float x, float y)
    {
        
    }

    /**
     * Initialize the board with the values. 
     */
    public void SetBoundings(int x, int y)
    {
        board = new Board();

        board.board = new Tile[x, y];
        board.width = x;
        board.height = y; 
    }

    void SetBoard()
    {
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {

            }
        }
    }


    /**
     * Sets the size of the board. This will be used to 
     * calculate and scalate all tiles in the board. 
     */
    public void SetAvailableSize(float x, float y)
    {
        avX = x;
        avY = y;
    }
}
