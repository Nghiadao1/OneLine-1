using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InterfaceType
{
    NormalSuperior,
    NormalInferior,
    ChallengeSuperior,
    ChallengeInferior
}

public class LevelInterfaceController : MonoBehaviour
{
    public InterfaceType _type;

    public string[] difficulties;

    private int _numCoins;

    private Text challengeCounter;

    private float challengeTimer = 30.0f;

    // Update is called once per frame
    void Update()
    {
        if(_type == InterfaceType.ChallengeInferior)
        {
            challengeTimer -= Time.deltaTime;

            string minutes = Mathf.Floor(challengeTimer / 60).ToString("00");
            string seconds = Mathf.RoundToInt(challengeTimer % 60).ToString("00");

            challengeCounter.text = minutes + ":" + seconds;

            // Comprobar si es 0 y esas mierdas 
        }
    }

    #region Normal Level Settings

    public void SetLevelSuperior(int difficulty, int level, int numCoins)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Text t = null;
            if (this.transform.GetChild(i).name == "Difficulty")
            { 
                t = this.transform.GetChild(i).GetComponent<Text>();
                if (t != null)
                {
                    t.text = difficulties[difficulty];
                }
            }
            else if(this.transform.GetChild(i).name == "Level")
            {
                t = this.transform.GetChild(i).GetComponent<Text>();
                if (t != null)
                {
                    t.text = level.ToString();
                }
            }
            else if(this.transform.GetChild(i).name == "Coins")
            {
                t = this.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>();
                if (t != null)
                {
                    _numCoins = numCoins;
                    t.text = numCoins.ToString();
                }
            }

            if (t != null)
            {
                t.fontStyle = FontStyle.Bold;
                t.fontSize = 40;
            }
        }
    }

    public void ChangeCoins(int coins)
    {
        _numCoins = coins;

        // Actualizar el texto de coins
    }
    #endregion

    #region Challenge Settings
    // COMENTAR
    public void SetChallengeSuperior()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Text t = this.transform.GetChild(i).GetComponent<Text>();
            if (t != null)
            {
                t.text = "Challenge";
                t.fontStyle = FontStyle.Bold;
                t.fontSize = 50;
            }
        }
    }

    public void SetChallengeInferior()
    {
        challengeCounter = this.transform.GetChild(0).GetComponent<Text>();
    }
    #endregion

    public InterfaceType getType()
    {
        return _type;
    }
}
