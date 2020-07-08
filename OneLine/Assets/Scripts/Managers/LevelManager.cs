using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public BoardManager bm;

    public SpriteRenderer fondo;

    /// <summary>
    /// Flag que controla si el nivel que va a jugar es un challenge o no
    /// </summary>
    bool challenge = false; 

    private void Start()
    {
        // Establecerse en el GameManager para la comunicación
        GameManager.GetInstance().setLevelManager(this);

        // Establecer el tamaño del fondo
        
        // Comprobar si la partida es un Challenge o no

        // Activar el canvas correspondiente

        // Consultar el nivel que hay que poner al GM

        // Cargar ese nivel 

        // Inicializar el BM con ese nivel 
    }

    private void Update()
    {
        // Comprobar si ha finalizado el nivel 

        // Si sí, avisar al GM para gestionar el fin de nivel 

        // Si no, no hace nada
    }


}
