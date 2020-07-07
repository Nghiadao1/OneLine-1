using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class BoardManager : MonoBehaviour
{
    // Original resolution of the board (CAMBIAR EL NOMBRE DE ESTA SEÑORA)
    Vector2 resolution;

    // Margenes que se van a dejar para hacer el espacio de juego 
    public int margenSuperior = 5;

    public int margenLateral = 45;

    // Simplemente para debugear (LIMPIAR LUEGO)
    public Transform panelDePrueba;

    // Prefab de los tiles
    public GameObject tile;

    // Esto es para luego calcular la posición del tablero
    private float panelSuperior;

    private float panelInferior;

    public Vector2 dimensiones =  new Vector2(); // Cuantos tiles hay a lo alto y a lo ancho

    float PixelToUnityPosition(float pixel)
    {
        return pixel /= GameManager.GetInstance().GetScaling().UnityUds();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Primero creamos el tablero de un tamaño concreto para que entren X tiles a lo largo y ancho
        // Le damos ese valor y luego, calculando el espacio disponible, lo ajustamos
        
        CalculateSpace();

        CalculatePosition();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region CalculateBoard
    /// <summary>
    /// Calcula el espacio disponible para el tablero de juego
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

        Debug.Log("Disponemos de este espacio: " + resolution);

        resolution /= GameManager.GetInstance().GetScaling().UnityUds();

        DefineTileSize();

        
    }

    void DefineTileSize()
    {
        Vector2 resolutionTemp = resolution * GameManager.GetInstance().GetScaling().UnityUds();

        Vector3 medidasTablero = new Vector3 ();

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

        //Escalado del tablero con los tiles una vez que se han instanciado todos estos
        Vector2 nTam = new Vector2(tamFinal, tamFinal);
        panelDePrueba.transform.localScale = 
            GameManager.GetInstance().GetScaling().resizeObjectScaleKeepingAspectRatio(tile.GetComponent<SpriteRenderer>().bounds.size * GameManager.GetInstance().GetScaling().UnityUds(), 
            nTam, tile.transform.localScale);
    }

    void InstantiateTiles(Vector3 medidasTablero, float tamTile)
    {
        Debug.Log(medidasTablero);

        for(int i = 0; i < dimensiones.y; i++)
        {
            for (int j = 0; j < dimensiones.x; j++)
            {
                GameObject nTile = Instantiate(tile, panelDePrueba.position, Quaternion.identity);

                //ConfigTile(nTile, tamTile);

                Vector3 position = new Vector3();

                position.z = -1;

                if(dimensiones.x % 2 == 0)
                {
                    //position.x = ((panelDePrueba.position.x - (medidasTablero.x / 2)) + (margenLateral / GameManager.GetInstance().GetScaling().UnityUds())) + (j * ((tamTile + 40) / GameManager.GetInstance().GetScaling().UnityUds())); 
                    position.x = (panelDePrueba.position.x + (dimensiones.x / 2) - 0.5f) - j;
                }
                else
                {
                    position.x = ((panelDePrueba.position.x - (int)(dimensiones.x / 2))) + j;
                }

                if (dimensiones.y % 2 == 0)
                {
                    //position.y = ((panelDePrueba.position.y - (medidasTablero.y / 2)) + (margenLateral / GameManager.GetInstance().GetScaling().UnityUds())) + (i * ((tamTile + 40) / GameManager.GetInstance().GetScaling().UnityUds()));
                    position.y = (panelDePrueba.position.y + (dimensiones.y / 2) - 0.5f) - i;
                }
                else
                {
                    position.y = (panelDePrueba.position.y + (int)(dimensiones.y / 2)) - i;
                }

                nTile.transform.SetPositionAndRotation(position, nTile.transform.rotation);

                nTile.transform.SetParent(panelDePrueba);
            }
        }
    }

    void ConfigTile(GameObject tile, float tamTile)
    {
        Vector2 nTam = new Vector2(tamTile, tamTile);

        tile.transform.localScale = GameManager.GetInstance().GetScaling().resizeObjectScaleKeepingAspectRatio(tile.GetComponent<SpriteRenderer>().bounds.size * GameManager.GetInstance().GetScaling().UnityUds(), nTam, tile.transform.localScale);
    }

    void CalculatePosition()
    {
        Vector3 position = new Vector3();

        float dispDistance = GameManager.GetInstance().getResolution().y - (panelInferior + panelSuperior);

        dispDistance /= 2; // Calculamos la distancia hasta el punto medio entre los dos paneles

        position.y = (GameManager.GetInstance().getResolution().y - panelSuperior) - dispDistance;
        
        // Ahora calcular la posición en unidades de Unity
        // Si la posición es mayor de la mitad, está en unidades de unity positivas
        if(position.y > (GameManager.GetInstance().getResolution().y / 2))
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


}
