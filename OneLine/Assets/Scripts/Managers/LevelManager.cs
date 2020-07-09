using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public BoardManager bm;

    public SpriteRenderer fondo;

    public int level = 0;

    public int difficulty = 0;
    /// <summary>
    /// Flag que controla si el nivel que va a jugar es un challenge o no
    /// </summary>
    bool challenge = false;

    LevelReader lr;

    private void Start()
    {
        // Establecerse en el GameManager para la comunicación
        GameManager.GetInstance().setLevelManager(this);

        level = GameManager.GetInstance().getLevel();
        difficulty = GameManager.GetInstance().getDifficulty();

        // Establecer el tamaño del fondo
        Vector3 result = GameManager.GetInstance().GetScaling().ScaleToFitScreen(fondo.sprite.bounds.size, fondo.transform.localScale);
        fondo.transform.localScale = result;

        // Comprobar si la partida es un Challenge o no
        LoadLevels(difficulty);

        // Activar el canvas correspondiente

        // Consultar el nivel que hay que poner al GM

        // Cargar ese nivel 
        bm.Init(lr.GetLevel(level));

        // Inicializar el BM con ese nivel 
    }

    public void LoadLevels(int difficulty)
    {
        lr = new LevelReader(Application.dataPath + "/Levels/" + difficulty + ".json");
    }

    public void EndGame()
    {
        GameManager.GetInstance().LevelCompleted();
    }

    public void ScreenReleased()
    {
        if (bm.Ended())
        {
            EndGame();
        }

        // HAcemos desaparecer al coso del touch
    }

    public void ScreenTouched(Vector2 position)
    {
        // Avisar al BoardManager de que se ha tocado la pantalla en una posición concreta
        bm.Touched(position);
    }
}
