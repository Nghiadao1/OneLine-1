using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public BoardManager bm;

    public SpriteRenderer fondo;

    // Canvas info 
    public GameObject[] levelInterface = new GameObject[2];
    public GameObject[] challengeInterface = new GameObject[2];

    // Se consultan en GM
    int level = 0;
    int difficulty = 0;
    int coins = 0;

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
        challenge = GameManager.GetInstance().challenge;

        // Comprobamos que las interfaces están desactivadas para luego activarlas según pida el GM
        if (levelInterface != null && challengeInterface != null)
        {
            for (int i = 0; i < levelInterface.Length; i++)
            {
                levelInterface[i].SetActive(false);
            }

            for (int i = 0; i < challengeInterface.Length; i++)
            {
                challengeInterface[i].SetActive(false);
            }
        }
        // Avisamos por si acaso
        else
        {
            Debug.LogError("Game Interfaces not assigned correctly! Here: " + this.gameObject);
        }

        // Establecer el tamaño del fondo
        Vector3 result = GameManager.GetInstance().GetScaling().ScaleToFitScreen(fondo.sprite.bounds.size, fondo.transform.localScale);
        fondo.transform.localScale = result;

        // Comprobar si la partida es un Challenge o no
        LoadLevels(difficulty);

        // Activar el canvas correspondiente
        SetCanvas();


        // Consultar el nivel que hay que poner al GM

        // Cargar ese nivel 
        bm.Init(lr.GetLevel(level));

        // Inicializar el BM con ese nivel 
    }

    public void SetCanvas()
    {
        if (!challenge)
        {
            for (int i = 0; i < levelInterface.Length; i++)
            {
                levelInterface[i].SetActive(true);
                if (levelInterface[i].GetComponent<LevelInterfaceController>().getType() == InterfaceType.NormalSuperior)
                {
                    levelInterface[i].GetComponent<LevelInterfaceController>().SetLevelSuperior(difficulty, level, coins);
                }
            }
        }
        else
        {
            for (int i = 0; i < challengeInterface.Length; i++)
            {
                challengeInterface[i].SetActive(true);
                if (challengeInterface[i].GetComponent<LevelInterfaceController>().getType() == InterfaceType.ChallengeSuperior)
                {
                    challengeInterface[i].GetComponent<LevelInterfaceController>().SetChallengeSuperior();
                }
                else if(challengeInterface[i].GetComponent<LevelInterfaceController>().getType() == InterfaceType.ChallengeInferior)
                {
                    challengeInterface[i].GetComponent<LevelInterfaceController>().SetChallengeInferior();
                }
            }
        }
    }

    public void LoadLevels(int difficulty)
    {
        lr = new LevelReader(Application.dataPath + "/Levels/" + difficulty + ".json");
    }

    public void ReloadLevel()
    {

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

    public void EndGame()
    {
        GameManager.GetInstance().LevelCompleted();
    }
}
