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
    
    // Start is called before the first frame update
    void Start()
    {
        // Clacular el tamaño disponible para el juego y luego ya si eso tirar 
        // Hay dejar definido el espacio que queremos ocupar de píxeles por un lado 
        // y el que queremos ocupar por arriba y por abajo (márgenes)
        CalculateSpace();

        Debug.Log(GameManager.GetInstance().panelSuperiorHeight());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Calcula el espacio disponible para el tablero de juego
    /// </summary>
    void CalculateSpace()
    {
        float panelSuperior = GameManager.GetInstance().panelSuperiorHeight();
        float panelInferior = GameManager.GetInstance().panelInferiorHeight();

        Vector2 refRes = GameManager.GetInstance().getReferenceResolution();
        Vector2 actRes = GameManager.GetInstance().getResolution();

        // Ahora calculamos el espacio disponible para el juego y restamos los márgenes pero en la resolución de referencia
        float dispTamY = (refRes.y - (panelInferior + panelSuperior)) - (2 * margenSuperior);
        float dipsTamX = refRes.x - (2 * margenLateral);

        // Calculamos los valores actuales en píxeles del espacio ocupado por los paneles EN LA RESOLUCIÓN ACTUAL 
        panelSuperior *= GameManager.GetInstance().GetCanvas().scaleFactor;
        panelInferior *= GameManager.GetInstance().GetCanvas().scaleFactor;

        // AHORA SE VIENE LA WEA
        // Calculamos el espacio disponible en la resolución actual 
        float dispTamActY = (actRes.y - (panelInferior + panelSuperior)) - (2 * GameManager.GetInstance().GetScaling().ResizeY(margenSuperior));
        float dipsTamActX = actRes.x - (2 * GameManager.GetInstance().GetScaling().ResizeX(margenLateral));

        // Creamos los Vectores para hacer los cálculos
        Vector2 tamTeoricoTabla = new Vector2 (dipsTamX, dispTamY);
        Vector2 espDisp = new Vector2(dipsTamActX, dispTamActY);

        // Escalamos el espacio de juego
        resolution = GameManager.GetInstance().GetScaling().ScaleToFitKeepingAspectRatio(tamTeoricoTabla, espDisp);

        Debug.Log(espDisp);
        Debug.Log(resolution);

        resolution /= GameManager.GetInstance().GetScaling().UnityUds();

        Debug.Log(tamTeoricoTabla);
        Debug.Log(resolution);

        panelDePrueba.localScale = GameManager.GetInstance().GetScaling().resizeObjectScale(panelDePrueba.GetComponent<SpriteRenderer>().bounds.size, resolution, panelDePrueba.localScale);
    }
}
