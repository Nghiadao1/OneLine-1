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

    float PixelToUnityPosition(float pixel)
    {
        return pixel /= GameManager.GetInstance().GetScaling().UnityUds();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        CalculateSpace();

        SetSpaceWithConfig();

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
        
        panelDePrueba.localScale = GameManager.GetInstance().GetScaling().resizeObjectScale(panelDePrueba.GetComponent<SpriteRenderer>().bounds.size, resolution, panelDePrueba.localScale);
    }

    void SetSpaceWithConfig()
    {
        Debug.Log(tile.transform.GetComponent<SpriteRenderer>().bounds.size * GameManager.GetInstance().GetScaling().UnityUds());
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
