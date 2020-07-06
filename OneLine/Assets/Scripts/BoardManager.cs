using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class BoardManager : MonoBehaviour
{
    // Original resolution of the board (CAMBIAR EL NOMBRE DE ESTA SEÑORA)
    public Vector2 resolution = new Vector2 (630, 780);

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
        panelSuperior = GameManager.GetInstance().panelSuperiorHeight();
        panelInferior = GameManager.GetInstance().panelInferiorHeight();

        Vector2 refRes = GameManager.GetInstance().getReferenceResolution();
        Vector2 actRes = GameManager.GetInstance().getResolution();

        // Ahora calculamos el espacio disponible para el juego y restamos los márgenes pero en la resolución de referencia
        float dispTamY = (refRes.y - (panelInferior + panelSuperior)) - (2 * margenSuperior);
        float dipsTamX = refRes.x - (2 * margenLateral);

        // Calculamos los valores actuales en píxeles del espacio ocupado por los paneles EN LA RESOLUCIÓN ACTUAL 
        panelSuperior *= GameManager.GetInstance().GetCanvas().scaleFactor;
        panelInferior *= GameManager.GetInstance().GetCanvas().scaleFactor;
        
        // Calculamos el espacio disponible en la resolución actual 
        float dispTamActY = (actRes.y - (panelInferior + panelSuperior)) - (2 * GameManager.GetInstance().GetScaling().ResizeY(margenSuperior));
        float dipsTamActX = actRes.x - (2 * GameManager.GetInstance().GetScaling().ResizeX(margenLateral));

        // Creamos los Vectores para hacer los cálculos
        Vector2 tamTeoricoTabla = new Vector2 (dipsTamX, dispTamY);
        Vector2 espDisp = new Vector2(dipsTamActX, dispTamActY);
        

        // Escalamos el espacio de juego
        resolution = GameManager.GetInstance().GetScaling().ScaleToFitKeepingAspectRatio(tamTeoricoTabla, espDisp);

        resolution /= GameManager.GetInstance().GetScaling().UnityUds();

        CreateBoardWithTiles();
    }

    void CreateBoardWithTiles()
    {
        Vector2 resolutionTemp = resolution * GameManager.GetInstance().GetScaling().UnityUds();

        Vector3 medidasTablero = new Vector3 ();

        medidasTablero.x = (tile.transform.GetComponent<SpriteRenderer>().bounds.size.x * GameManager.GetInstance().GetScaling().UnityUds()) * 6;

        if (dimensiones.y > 5)
        {
            medidasTablero.y = (tile.transform.GetComponent<SpriteRenderer>().bounds.size.x * GameManager.GetInstance().GetScaling().UnityUds()) * 8;
        }
        else
        {
            medidasTablero.y = (tile.transform.GetComponent<SpriteRenderer>().bounds.size.x * GameManager.GetInstance().GetScaling().UnityUds()) * 5;
        }

        medidasTablero /= GameManager.GetInstance().GetScaling().UnityUds();

        InstantiateTiles(medidasTablero);
        
        panelDePrueba.localScale = GameManager.GetInstance().GetScaling().resizeObjectScaleKeepingAspectRatio(medidasTablero * GameManager.GetInstance().GetScaling().UnityUds(), resolutionTemp, panelDePrueba.localScale);

    }

    void InstantiateTiles(Vector3 medidasTablero)
    {
        Debug.Log(medidasTablero);

        for(int i = 0; i < dimensiones.y; i++)
        {
            for (int j = 0; j < dimensiones.x; j++)
            {
                Vector3 position = new Vector3();

                position.z = -1;

                if(dimensiones.x % 2 == 0)
                {
                    position.x = ((panelDePrueba.position.x - (dimensiones.x / 2)) + 0.5f) + j;                   
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

                Debug.Log(position);

                GameObject nTile = Instantiate(tile, position, Quaternion.identity);

                nTile.transform.SetParent(panelDePrueba);

                ConfigTile();
            }
        }
    }

    void ConfigTile()
    {

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
