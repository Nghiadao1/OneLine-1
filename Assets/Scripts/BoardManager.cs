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
                // Aquí es donde entraría el cálculo de las posiciones, que es: 
                //      Calcular con el espacio que tenemos dónde colocar los tiles (width / nº de tiles da cuánto ocuparía cada tile) y lo mismo para la altura 
                //      Una vez esto, hay que saber el nº de Unidades de Unity que ocupa el tablero, sabiendo que está en la pos (0, 0)
                //      Aquí hay que calcular de nuevo dónde irían los tiles, ya que empezarían a colocarse en (-X, 0) (Quitando el espacio necesario para que encuadren etc.)
                //      Una vez hecho eso, es sumar la j o la i según sea necesario
                //      Quizá deberíamos hacerlo en un método a parte. 

                Vector3 position = new Vector3(transform.position.x * j, transform.position.y * i, -2);
                Vector3 positionCol = new Vector3(transform.position.x * j, transform.position.y * i, -3);

                // Instantiate GameObjects needed 
                brd.board[i, j] = Instantiate(tilePrefab, position, Quaternion.identity); // Position
                GameObject colorSprite = Instantiate(colour, positionCol, Quaternion.identity);

                // Attacht them to parents
                brd.board[i, j].transform.SetParent(transform);
                colorSprite.transform.SetParent(brd.board[i, j].transform);

                // Set the color sprite to Tile
                brd.board[i, j].GetComponent<Tile>().SetColor(colorSprite);

            }
        }
    }
}
