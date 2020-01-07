using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject pathPrefab;

    // Private atributes
    string path = "Prefabs/Game/Skins";

    Stack<TilePosition> playerPath;
    List<TilePosition> hintPath;

    // Board 
    struct Board
    {
        public GameObject[,] board;

        public float sizeX, sizeY;

        public Vector2 size;
    }

    void Awake()
    {
        playerPath = new Stack<TilePosition>();
        hintPath = new List<TilePosition>();
    }

    Board brd; // GameBoard of tiles

    public void SavePath(TilePosition[] path)
    {
        for (int i = 0; i < path.Length; i++)
        {
            hintPath.Add(path[i]);
        }    
    }

    public void SetBoard (Vector2 size, Vector2 camSize)
    {
        Levels level = GameManager.instance.InitActualLevel();

        SavePath(level.path);

        int sizeX, sizeY;

        sizeX = level.layout[0].Length;
        sizeY = level.layout.Length;

        // Get a random number to decide which block is going to be used
        int color = Random.Range(1, 8);

        GameObject colour = Resources.Load(path + "/Blocks/block_0" + color) as GameObject;
        GameObject pathColor = Resources.Load(path + "/Hints/block_0" + color + "_hint") as GameObject;

        Vector2 scale = transform.localScale;

        brd.board = new GameObject[sizeY, sizeX];
        brd.size = DefinePlayZone(size, sizeX, sizeY, ref scale);
        brd.sizeX = sizeX;
        brd.sizeY = sizeY;
        int identification = 0;


        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {

                Vector3 position = new Vector3(((transform.position.x  - (sizeX/2)) + 0.5f) + j, (transform.position.y  - (sizeY/2))  + i, -1);

                if(level.layout[i][j] != '0')
                {
                    // Instantiate GameObjects needed 
                    brd.board[i, j] = Instantiate(tilePrefab, position, Quaternion.identity); // Position
                    GameObject colorSprite = Instantiate(colour, position, Quaternion.identity);
                    GameObject camino = Instantiate(pathPrefab, position, Quaternion.identity);
                    GameObject hintPivot = new GameObject("HintPivot");
                    GameObject pathSpr = Instantiate(pathColor, position, Quaternion.identity);

                    // Attacht them to parents
                    brd.board[i, j].transform.SetParent(transform);
                    colorSprite.transform.SetParent(brd.board[i, j].transform);
                    camino.transform.SetParent(brd.board[i, j].transform);
                    hintPivot.transform.SetParent(brd.board[i, j].transform);
                    pathSpr.transform.SetParent(hintPivot.transform);
                    pathSpr.transform.SetPositionAndRotation(camino.transform.GetChild(0).transform.position, camino.transform.GetChild(0).transform.rotation);

                    // Set the color sprite to Tile
                    brd.board[i, j].GetComponent<Tile>().SetColor(colorSprite);
                    brd.board[i, j].GetComponent<Tile>().SetPathSpr(camino);
                    brd.board[i, j].GetComponent<Tile>().SetHintSpr(hintPivot);
                    brd.board[i, j].GetComponent<Tile>().SetPressed(false, 0.0f);
                    brd.board[i, j].GetComponent<Tile>().SetPosBoard(i, j);
                    brd.board[i, j].GetComponent<Tile>().SetID(identification);
                    identification++;

                    Debug.Log(level.layout[i][j]);

                    if (level.layout[i][j] == '2')
                    { 
                        brd.board[i, j].GetComponent<Tile>().SetPressed(true, 0.0f);
                        brd.board[i, j].GetComponent<Tile>().CreatePath(0.0f, false);
                        playerPath.Push(brd.board[i, j].GetComponent<Tile>().GetPosition());
                    }
                }
            }
        }

        // Escalate all board
        transform.localScale = BoardEscalate(brd.size, camSize, scale);
    }

    #region Escalado y cosos
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
    #endregion

    #region Gestion de Tiles
    bool CheckTile(TilePosition pos, int x, int y)
    {
        GameObject t = brd.board[y, x];

        if (t.GetComponent<Tile>().GetPressed() && (playerPath.Peek() == t.GetComponent<Tile>().GetPosition()))
        {
            playerPath.Push(pos);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TileClicked(int id, TilePosition pos, bool isClicked, ref float degrees)
    {
        if (!isClicked)
        {
            bool activate = false;

            if(!activate && pos.x - 1 >= 0)
            {
                activate = CheckTile(pos, (int)pos.x - 1, (int)pos.y);
                if (activate)
                {
                    degrees = 180.0f;
                }
            }

            if (!activate && pos.x + 1 < brd.sizeX)
            {
                activate = CheckTile(pos, (int)pos.x + 1, (int)pos.y);
                if (activate)
                {
                    degrees = 0.0f;
                }
            }

            if(!activate && pos.y - 1 >= 0)
            {
                activate = CheckTile(pos, (int)pos.x, (int)pos.y - 1);
                if (activate)
                {
                    degrees = -90.0f;
                }
            }

            if(!activate && pos.y + 1 < brd.sizeY)
            {
                activate = CheckTile(pos, (int)pos.x, (int)pos.y + 1);
                if (activate)
                {
                    degrees = 90.0f;
                }
            }
            return activate;
        }
        else
        {
            while (playerPath.Peek() != pos)
            {
                brd.board[pos.x, pos.y].GetComponent<Tile>().SetPressed(false, 0.0f);
                playerPath.Pop();
            }
            return false; 
        }
    }

    public bool Ended()
    {
        return true;
    }

    public Vector2 GetSize ()
    {
        return brd.size;
    }
    #endregion
}