using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;


/// <summary>
/// This class is the one that will control the board of the game and 
/// manage all the changes in the tiles. Calculates the space between 
/// the panels to locate the game. Calculates how many tiles are needed
/// for the current level. Sets the space for the current configuration
/// and scales everything keeping aspect ratio. 
/// </summary>
public class BoardManager : MonoBehaviour
{
    // Space that the board will take
    Vector2 resolution;

    // Margins
    public int margenSuperior = 5;
    public int margenLateral = 45;

    // Object that will contain the Board
    public Transform panelDePrueba;
    GameObject[,] board;

    Stack<Vector2> path;

    // Tile's prefab
    GameObject tile;
    GameObject playerPath;
    GameObject colorTile;
    GameObject pathColor;

    // Tile Origen del nivel
    GameObject origin;

    // Panels to calculate the space for the board
    private float panelSuperior;
    private float panelInferior;

    // Variables del tablero
    Levels _level;

    // How many tiles will be in the current level (WidthxHeight)
    Vector2 dimensiones; // Cuantos tiles hay a lo alto y a lo ancho

    // Hint management
    int lastHint = 1;


    /// <summary>
    /// Converts a pixel meassure to Unity Units.
    /// </summary>
    /// <param name="pixel">Pixels</param>
    /// <returns>Unity Units</returns>
    float PixelToUnityPosition(float pixel)
    {
        return pixel /= GameManager.GetInstance().GetScaling().UnityUds();
    }

    private void Awake()
    {
        path = new Stack<Vector2>();
    }

    // Start is called before the first frame update
    public void Init(Levels level, int c)
    {
        _level = level;

        dimensiones = new Vector2(_level.layout[0].Length, _level.layout.Length);

        // Primero creamos el tablero de un tamaño concreto para que entren X tiles a lo largo y ancho
        // Le damos ese valor y luego, calculando el espacio disponible, lo ajustamos
        board = new GameObject[(int)dimensiones.y, (int)dimensiones.x];

        margenSuperior = (int)(7.5 * dimensiones.y);
        margenLateral = (int)(11.7 * dimensiones.x);

        InitGameObjects(c);

        CalculateSpace();

        CalculatePosition();
    }

    #region Calculate and create Board
    void InitGameObjects(int color)
    {
        tile = GameManager.GetInstance().getPrefabFromTileAssetBundle("block_00");
        playerPath = GameManager.GetInstance().getPrefabFromPathAssetBundle("block_00_hint");
        colorTile = GameManager.GetInstance().getPrefabFromTileAssetBundle("block_0" + color);
        pathColor = GameManager.GetInstance().getPrefabFromPathAssetBundle("block_0" + color + "_hint");
    }

    /// <summary>
    /// Function that calculates the space available for the board and the game.  Uses the height 
    /// of the different panels to calculate this space in relation with the height of the screen.
    /// 
    /// </summary>
    void CalculateSpace()
    {
        // Calculamos el espacio ocupado por los paneles superior e inferior en píxeles
        panelSuperior = GameManager.GetInstance().panelSuperiorHeight() * GameManager.GetInstance().GetCanvas().scaleFactor;
        panelInferior = GameManager.GetInstance().panelInferiorHeight() * GameManager.GetInstance().GetCanvas().scaleFactor;

        Vector2 actRes = GameManager.GetInstance().getResolution();

        // Calculamos el espacio disponible en la resolución actual 
        float dispY = (actRes.y - (panelInferior + panelSuperior)) - (2 * GameManager.GetInstance().GetScaling().ResizeY(margenSuperior));
        float dipsX = actRes.x - (2 * GameManager.GetInstance().GetScaling().ResizeX(margenLateral));

        // Creamos el espacio disponible en pantalla (en píxeles) para el juego
        resolution = new Vector2(dipsX, dispY);

        resolution /= GameManager.GetInstance().GetScaling().UnityUds();

        DefineTileSize();
    }

    void DefineTileSize()
    {
        Vector2 resolutionTemp = resolution * GameManager.GetInstance().GetScaling().UnityUds();

        Vector3 medidasTablero = new Vector3();

        float tamFinal;
        float tamTilesX;
        float tamTilesY;

        if (dimensiones.y > 5)
        {
            tamTilesX = resolutionTemp.x / 6;
            tamTilesY = resolutionTemp.y / 8;
        }
        else
        {
            tamTilesX = resolutionTemp.x / 6;
            tamTilesY = resolutionTemp.y / 5;

        }

        if (tamTilesY < tamTilesX)
        {
            tamFinal = tamTilesY;
        }
        else
        {
            tamFinal = tamTilesX;
        }

        medidasTablero.x = tamFinal * dimensiones.x;
        medidasTablero.y = tamFinal * dimensiones.y;

        medidasTablero /= GameManager.GetInstance().GetScaling().UnityUds();

        InstantiateTiles(medidasTablero, tamFinal);

        //Escalado del tablero con los tiles una vez que se han instanciado todos
        Vector2 nTam = new Vector2(tamFinal, tamFinal);
        Vector2 nScale = GameManager.GetInstance().GetScaling().resizeObjectScaleKeepingAspectRatio(tile.GetComponent<SpriteRenderer>().bounds.size * GameManager.GetInstance().GetScaling().UnityUds(),
            nTam, tile.transform.localScale);
        panelDePrueba.transform.localScale = new Vector3(nScale.x, nScale.y, 1);
    }

    void InstantiateTiles(Vector3 medidasTablero, float tamTile)
    {
        for (int i = 0; i < dimensiones.y; i++)
        {
            for (int j = 0; j < dimensiones.x; j++)
            {
                if (_level.layout[i][j] != '0')
                {
                    Vector3 position = new Vector3();

                    position.z = -1;

                    if (dimensiones.x % 2 == 0)
                    {
                        position.x = (panelDePrueba.position.x - (dimensiones.x / 2) + 0.5f) + j;
                    }
                    else
                    {
                        position.x = ((panelDePrueba.position.x - (int)(dimensiones.x / 2))) + j;
                    }

                    if (dimensiones.y % 2 == 0)
                    {
                        position.y = (panelDePrueba.position.y + (dimensiones.y / 2) - 0.5f) - i;
                    }
                    else
                    {
                        position.y = (panelDePrueba.position.y + (int)(dimensiones.y / 2)) - i;
                    }

                    ConfigTile(position, j, i);

                    if (_level.layout[i][j] == '2')
                    {
                        path.Push(new Vector2(j, i));

                        board[i, j].GetComponent<Tile>().ActivateColor();

                        origin = board[i, j];
                    }
                }
            }
        }
    }

    void ConfigTile(Vector3 pos, int posX, int posY)
    {
        // Instantiate GameObjects needed
        GameObject nTile = Instantiate(tile, pos, Quaternion.identity);
        GameObject clTile = Instantiate(colorTile, pos, Quaternion.identity);
        GameObject pathPivot = new GameObject("PathPivot");
        pathPivot.transform.SetPositionAndRotation(pos, Quaternion.identity);
        GameObject plPath = Instantiate(playerPath, pos, Quaternion.identity);
        GameObject hintPivot = new GameObject("HintPivot");
        hintPivot.transform.SetPositionAndRotation(pos, Quaternion.identity);
        GameObject hnPath = Instantiate(pathColor, pos, Quaternion.identity);

        // Attacht them to parents
        nTile.transform.SetParent(panelDePrueba);
        clTile.transform.SetParent(nTile.transform);
        hintPivot.transform.SetParent(nTile.transform);
        pathPivot.transform.SetParent(nTile.transform);

        // Configure paths to rotate correctly
        plPath.transform.SetParent(pathPivot.transform);
        plPath.transform.SetPositionAndRotation(pathPivot.transform.position + new Vector3(0.5f, 0, 0), pathPivot.transform.rotation);

        hnPath.transform.SetParent(hintPivot.transform);
        hnPath.transform.SetPositionAndRotation(hintPivot.transform.position + new Vector3(0.5f, 0, 0), hintPivot.transform.rotation);

        // Configure Tile info for later use
        nTile.transform.GetComponent<Tile>().SetTile(nTile, clTile, pathPivot, hintPivot, new Vector2(posX, posY));

        // Save the tile in the array
        board[posY, posX] = nTile;
    }

    void CalculatePosition()
    {
        Vector3 position = new Vector3();

        float dispDistance = GameManager.GetInstance().getResolution().y - (panelInferior + panelSuperior);

        dispDistance /= 2; // Calculamos la distancia hasta el punto medio entre los dos paneles

        position.y = (GameManager.GetInstance().getResolution().y - panelSuperior) - dispDistance;

        // Ahora calcular la posición en unidades de Unity
        // Si la posición es mayor de la mitad, está en unidades de unity positivas
        if (position.y > (GameManager.GetInstance().getResolution().y / 2))
        {
            position.y -= (GameManager.GetInstance().getResolution().y / 2);

            position.y = PixelToUnityPosition(position.y);
        }
        // Si no, está en unidades negativas
        else if (position.y < (GameManager.GetInstance().getResolution().y / 2))
        {
            position.y = (GameManager.GetInstance().getResolution().y / 2) - position.y;

            position.y = (PixelToUnityPosition(position.y) * (-1));
        }
        // Por último, la posición 0, 0, 0
        else
        {
            position.y = 0;
        }

        panelDePrueba.SetPositionAndRotation(position, panelDePrueba.rotation);
    }
    #endregion

    #region GamePlay
    public void Touched(Vector2 position)
    {
        float degrees;
        RaycastHit2D ray = Physics2D.Raycast(position, Vector2.zero);
        if (ray)
        {
            if (ray.collider.gameObject.GetComponent<Tile>())
            {
                Tile tile = ray.collider.gameObject.GetComponent<Tile>();

                if (path.Contains(tile.getPositionInBoard()))
                {
                    while (path.Peek() != tile.getPositionInBoard())
                    {
                        board[(int)path.Peek().y, (int)path.Peek().x].GetComponent<Tile>().ResetTile();
                        path.Pop();
                    }
                }
                else
                {
                    // Si no, comprobamos si alrededor tiene tiles activados y el que está activo es el último
                    if (CheckTile(new Vector2(tile.getPositionInBoard().x - 1, tile.getPositionInBoard().y)))
                    {
                        degrees = 180.0f;
                        TileAdded(tile, degrees);
                    }
                    else if (CheckTile(new Vector2(tile.getPositionInBoard().x + 1, tile.getPositionInBoard().y)))
                    {
                        degrees = 0.0f;
                        TileAdded(tile, degrees);
                    }
                    else if (CheckTile(new Vector2(tile.getPositionInBoard().x, tile.getPositionInBoard().y - 1)))
                    {
                        degrees = 90.0f;
                        TileAdded(tile, degrees);
                    }
                    else if (CheckTile(new Vector2(tile.getPositionInBoard().x, tile.getPositionInBoard().y + 1)))
                    {
                        degrees = -90.0f;
                        TileAdded(tile, degrees);
                    }
                }
            }
        }
    }

    public void TileAdded(Tile t, float degrees)
    {
        path.Push(t.getPositionInBoard());

        t.ActivateColor();
        t.RotatePlayerPath(degrees);
    }

    public bool CheckTile(Vector2 position)
    {
        if (position.x >= dimensiones.x || position.x < 0 || position.y >= dimensiones.y || position.y < 0)
        {
            return false;
        }
        else if (board[(int)position.y, (int)position.x] != null)
        {
            if (path.Peek() == position)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void HintGiven()
    {
        float degrees = 0.0f;
        int i;

        for (i = lastHint; i < (lastHint + 4) && i < _level.path.Length; i++)
        {
            if ((int)_level.path[i].x == (int)_level.path[i - 1].x - 1)
            {
                degrees = -90.0f;
            }
            else if ((int)_level.path[i].x == (int)_level.path[i - 1].x + 1)
            {
                degrees = 90.0f;
            }
            else if ((int)_level.path[i].y == (int)_level.path[i - 1].y - 1)
            {
                degrees = 0.0f;
            }
            else if ((int)_level.path[i].y == (int)_level.path[i - 1].y + 1)
            {
                degrees = 180.0f;
            }

            board[(int)_level.path[i].x, (int)_level.path[i].y].GetComponent<Tile>().RotateHintPath(degrees);
        }

        lastHint = i;
    }

    public void ResetLevel()
    {
        while (path.Peek() != origin.GetComponent<Tile>().getPositionInBoard())
        {
            board[(int)path.Peek().y, (int)path.Peek().x].GetComponent<Tile>().ResetTile();
            path.Pop();
        }
    }

    public bool Ended()
    {
        if (path.ToArray().Length == _level.path.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanBuyHint()
    {
        return lastHint == (int)_level.path.Length;
    }
    #endregion
}
