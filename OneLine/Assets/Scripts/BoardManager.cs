using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    // Original resolution of the board
    public Vector2 resolution = new Vector2 (630, 780);
    
    // Start is called before the first frame update
    void Start()
    {
        // Clacular el tamaño disponible para el juego y luego ya si eso tirar 
        // Hay dejar definido el espacio que queremos ocupar de píxeles por un lado 
        // y el que queremos ocupar por arriba y por abajo (márgenes)

        Vector2 temp = GameManager.GetInstance().GetScaling().ScaleKeepingAspectRatio(resolution);

        Debug.Log(temp + " Esto es lo que da de prueba");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
