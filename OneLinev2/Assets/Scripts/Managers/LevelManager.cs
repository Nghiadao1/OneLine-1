using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Canvas cnv;
    public Camera cam;

    public BoardManager bm;

    public SpriteRenderer fondo;

    // Canvas info 
    public GameObject[] levelInterface = new GameObject[2];
    public GameObject[] challengeInterface = new GameObject[2];

    // Touch feedback
    GameObject _touchFB;

    // Se consultan en GM
    int level = 0;
    int difficulty = 0;
    int coins = 2000;

    /// <summary>
    /// Flag que controla si el nivel que va a jugar es un challenge o no
    /// </summary>
    bool challenge = false;
    bool ended = false; // Para gestionar el input

    LevelReader lr;

    public ClearPanelController cpc;

    private void Start()
    {
        GameManager.GetInstance().setLevelManager(this);
        GameManager.GetInstance().SetCanvas(cnv);
        GameManager.GetInstance().SetCamera(cam);
        GameManager.GetInstance().ReloadPanels();

        coins = GameManager.GetInstance().getPlayerCoins();

        ConfigCanvas();

        level = GameManager.GetInstance().getLevel();
        difficulty = GameManager.GetInstance().getDifficulty();
        challenge = GameManager.GetInstance().getChallenge();

        Vector3 result = GameManager.GetInstance().GetScaling().ScaleToFitScreen(fondo.sprite.bounds.size, fondo.transform.localScale);
        fondo.transform.localScale = result;

        LoadLevels(difficulty);

        SetCanvas();

        int color = Random.Range(1, 8);

        _touchFB = Instantiate(GameManager.GetInstance().getPrefabFromTouchAssetBundle("block_0" + color + "_touch"));

        _touchFB.SetActive(false);

        bm.Init(lr.GetLevel(level), color);
    }

    public void LoadLevels(int difficulty)
    {
        lr = new LevelReader(difficulty);
    }

    public void ConfigCanvas()
    {
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
    }

    public LevelInterfaceController GetInterfacePart(InterfaceType t)
    {
        if (challenge)
        {
            for (int i = 0; i < challengeInterface.Length; i++)
            {
                if (challengeInterface[i].GetComponent<LevelInterfaceController>().getType() == t)
                {
                    return challengeInterface[i].GetComponent<LevelInterfaceController>();
                }
            }
        }
        else
        {
            for (int i = 0; i < levelInterface.Length; i++)
            {
                if (levelInterface[i].GetComponent<LevelInterfaceController>().getType() == t)
                {
                    return levelInterface[i].GetComponent<LevelInterfaceController>();
                }
            }
        }
        return null;
    }

    public void SetCanvas()
    {
        LevelInterfaceController lic;

        if (!challenge)
        {
            if ((lic = GetInterfacePart(InterfaceType.NormalSuperior)) != null)
            {
                lic.SetLevelSuperior(difficulty, level, coins);
            }
            else
            {
                Debug.LogError("Interface parts missing: " + InterfaceType.NormalSuperior);
            }
            if ((lic = GetInterfacePart(InterfaceType.NormalInferior)) != null)
            {
                lic.SetLevelInferior(GameManager.GetInstance().getPrice());
            }
            else
            {
                Debug.LogError("Interface parts missing: " + InterfaceType.NormalInferior);
            }
        }
        else
        {
            if ((lic = GetInterfacePart(InterfaceType.ChallengeSuperior)) != null)
            {
                lic.SetChallengeSuperior();
            }
            else
            {
                Debug.LogError("Interface parts missing: " + InterfaceType.ChallengeSuperior);
            }

            if ((lic = GetInterfacePart(InterfaceType.ChallengeInferior)) != null)
            {
                lic.SetChallengeInferior();
            }
            else
            {
                Debug.LogError("Interface parts missing: " + InterfaceType.ChallengeInferior);
            }
        }
    }

    public void HintRequested()
    {
        LevelInterfaceController lic;
        if (bm.CanBuyHint())
        {
            if (coins >= GameManager.GetInstance().getPrice())
            {
                coins -= GameManager.GetInstance().getPrice();

                GameManager.GetInstance().CoinsUsed();

                if ((lic = GetInterfacePart(InterfaceType.NormalSuperior)) != null)
                {
                    lic.ChangeCoins(coins);
                }
                else
                {
                    Debug.LogError("Interface parts missing: " + InterfaceType.NormalSuperior);
                }

                bm.HintGiven();
            }
        }
    }

    public void AdRequested()
    {
        LevelInterfaceController lic;

        GameManager.GetInstance().AdRewardCoins();

        coins = GameManager.GetInstance().getPlayerCoins();

        if ((lic = GetInterfacePart(InterfaceType.NormalSuperior)) != null)
        {
            lic.ChangeCoins(coins);
        }
        else
        {
            Debug.LogError("Interface parts missing: " + InterfaceType.NormalSuperior);
        }

    }

    public void ReloadLevel()
    {
        bm.ResetLevel();
    }

    public void ScreenReleased()
    {
        if (!ended)
        {
            _touchFB.SetActive(false);

            if (bm.Ended())
            {
                ended = true;
                EndGame();
            }
        }
    }

    public void ScreenTouched(Vector2 position)
    {
        if (!ended)
        {
            if (!_touchFB.active)
            {
                _touchFB.SetActive(true);
            }

            _touchFB.transform.SetPositionAndRotation(position, Quaternion.identity);

            // Avisar al BoardManager de que se ha tocado la pantalla en una posición concreta
            bm.Touched(position);
        }
    }

    public void EndGame()
    {
        ended = true;
        // Activar un panel para decidir si vamos al siguiente nivel o no
        if (!challenge)
        {
            cpc.LevelComplete();

            cpc.SetDifficultyText(GameManager.GetInstance().getDifficultyText());
            cpc.SetLevelNumber(level);

            GameManager.GetInstance().LevelCompleted();
        }
        else
        {
            if (bm.Ended())
            {
                cpc.ChallengeComplete();
                GetInterfacePart(InterfaceType.ChallengeInferior).ChallengeCompleted();
                GameManager.GetInstance().ChallengeWin();
            }
            else
            {
                cpc.ChallengeFailed();
                GameManager.GetInstance().ChallengeFailed();
            }
        }
    }
}
