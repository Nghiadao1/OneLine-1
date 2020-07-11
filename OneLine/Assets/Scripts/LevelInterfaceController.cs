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

public enum Difficulty
{
    Beginner,
    Regular,
    Advanced,
    Expert,
    Master
}

// CAMBIAR INFERIOR Y SUPERIOR POR BOTTOM Y TOP
public class LevelInterfaceController : MonoBehaviour
{
    public InterfaceType _type;

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
        this.gameObject.SetActive(true);

        Text t = null;
        GameObject temp;

        if ((temp = SearchChild("Difficulty")) != null)
        {
            t = temp.transform.GetComponent<Text>();
            if (t != null)
            {
                t.text = GameManager.GetInstance().getDifficultyText();
            }
        }
        else
        {
            ErrorObjectNotFound("Difficulty");
        }

        if ((temp = SearchChild("Level")) != null)
        {
            t = temp.transform.GetComponent<Text>();
            if (t != null)
            {
                t.text = level.ToString();
            }
        }
        else
        {
            ErrorObjectNotFound("Level");
        }

        if ((temp = SearchChild("Coins")) != null)
        {
            t = temp.transform.GetChild(0).GetComponent<Text>();
            if (t != null)
            {
                _numCoins = numCoins;
                t.text = numCoins.ToString();
            }
        }
        else
        {
            ErrorObjectNotFound("Coins");
        }

        if (t != null)
        {
            t.fontStyle = FontStyle.Bold;
            t.fontSize = 40;
        }
    }

    public void SetLevelInferior(int coinsPrice)
    {
        this.gameObject.SetActive(true);

        GameObject h;

        if ((h = SearchChild("Hint")) != null)
        {
            Text t = h.transform.GetChild(0).GetComponent<Text>();
            t.text = coinsPrice.ToString();
            t.fontStyle = FontStyle.Bold;
            t.fontSize = 30;
        }
        else
        {
            ErrorObjectNotFound("Hint");
        }
    }

    public void ChangeCoins(int coins)
    {
        _numCoins = coins;
        GameObject c;
        if ((c = SearchChild("Coins")) != null)
        {
            Text t = c.transform.GetChild(0).GetComponent<Text>();

            if (t != null)
            {
                t.text = _numCoins.ToString();
            }
        }
        else
        {
            ErrorObjectNotFound("Coins");
        }
    }

    public GameObject SearchChild(string name)
    {
        GameObject child = null;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            child = this.transform.GetChild(i).gameObject;

            if (child.name == name)
            {
                return child;
            }
        }

        return null;
    }
    #endregion

    #region Challenge Settings
    // COMENTAR
    public void SetChallengeSuperior()
    {
        this.gameObject.SetActive(true);

        GameObject temp;
        if (temp = SearchChild("Challenge"))
        {
            Text t = temp.GetComponent<Text>();
            if (t != null)
            {
                t.text = "Challenge";
                t.fontStyle = FontStyle.Bold;
                t.fontSize = 50;
            }
        }
        else
        {
            ErrorObjectNotFound("Challenge");
        }
    }

    public void SetChallengeInferior()
    {
        this.gameObject.SetActive(true);

        GameObject temp;

        if((temp = SearchChild("Counter")) != null)
        {
            challengeCounter = temp.GetComponent<Text>();
        }
        else
        {
            ErrorObjectNotFound("Counter");
        }
    }
    #endregion

    void ErrorObjectNotFound(string objectName)
    {
        Debug.LogError("Missing interface object: " + objectName + ". In interface component: " + _type);
    }

    public InterfaceType getType()
    {
        return _type;
    }
}
