using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Types of the level interfaces (in Game scene)
/// </summary>
public enum InterfaceType
{
    NormalTop,                                  // Top interface in a normal level
    NormalBottom,                               // Bottom interface in a normal level
    ChallengeTop,                               // Top interface in a challenge level
    ChallengeBottom                             // Bottom interface in a challenge level
}

/// <summary>
/// Manages the buttons, information and values of the all interfaces in Game scene,
/// differentiating when is a normal or a challenge level
/// </summary>
public class LevelInterfaceController : MonoBehaviour
{
    public InterfaceType _type;                 // Specific type of this interface

    private int _numCoins;                      // Number of player coins

    private Text _challengeCounter;             // Shows the timer to finish the challenge

    private float _challengeTimer = 30.0f;      // Maximum seconds to finish a challenge level

    private bool _playing = false;              // Determinates if the player is in a challenge until the timer finish

    // Update is called once per frame
    /// <summary>
    /// Checks the elapsed time between frames to decreases the timer if the player is in a challenge level
    /// </summary>
    void Update()
    {
        // If the player is in a challenge and this is a bottom challenge interface
        if (_playing && _type == InterfaceType.ChallengeBottom)
        {
            // Gets the elapsed time
            _challengeTimer -= Time.deltaTime;

            // If the timer finish
            if (_challengeTimer <= 0)
            {
                // Stops and finishes the challenge. Also notify the time is up to the game manager 
                _playing = false;
                GameManager.GetInstance().TimeIsUp();
            }
            else
            {
                // If the time is not finished sets the time in minutes and seconds with the 00:00 format
                string minutes = Mathf.Floor(_challengeTimer / 60).ToString("00");
                string seconds = Mathf.RoundToInt(_challengeTimer % 60).ToString("00");

                _challengeCounter.text = minutes + ":" + seconds;
            }
        }
    }

    #region Normal Level Settings

    /// <summary>
    /// Sets all the information and values of the top level interface
    /// </summary>
    /// <param name="level">Number of the current level</param>
    /// <param name="numCoins">Number of player coins</param>
    public void SetLevelSuperior( int level, int numCoins)
    {
        // Activates this interface in the Game scene canvas
        this.gameObject.SetActive(true);

        Text t = null;
        GameObject temp;

        // If this has a child named Difficulty
        if ((temp = SearchChild("Difficulty")) != null)
        {
            // Sets the text to the current difficulty
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
        // If this has a child named Level
        if ((temp = SearchChild("Level")) != null)
        {
            // Sets the text to the current level
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
        // If this has a child named Coins
        if ((temp = SearchChild("Coins")) != null)
        {
            // Sets the text to the current player coins
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
        // Sets the texts style and font
        if (t != null)
        {
            t.fontStyle = FontStyle.Bold;
            t.fontSize = 40;
        }
    }

    /// <summary>
    /// Sets all the information and values of the bottom level interface
    /// </summary>
    /// <param name="coinsPrice">Cost in coins of asking for hints</param>
    public void SetLevelInferior(int coinsPrice)
    {
        // Activates this interface in the Game scene canvas
        this.gameObject.SetActive(true);

        GameObject h;

        // If this has a child named Hint
        if ((h = SearchChild("Hint")) != null)
        {
            // Sets the value, style and size of the ask for a hint text (the prize of a hint)
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

    /// <summary>
    /// Sets the actual value of the player coins
    /// </summary>
    /// <param name="coins">Number of player coins</param>
    public void ChangeCoins(int coins)
    {
        _numCoins = coins;
        GameObject c;
        // If this has a child named Coins
        if ((c = SearchChild("Coins")) != null)
        {
            Text t = c.transform.GetChild(0).GetComponent<Text>();

            // Sets its coins text with the actual player coins value
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

    /// <summary>
    /// Search in the object to find a child with the param "name" name
    /// </summary>
    /// <param name="name">Name of a child</param>
    /// <returns>Child with the name searched</returns>
    public GameObject SearchChild(string name)
    {
        GameObject child = null;

        // Search in all the childs
        for (int i = 0; i < this.transform.childCount; i++)
        {
            child = this.transform.GetChild(i).gameObject;

            // If it finds a child with the name searched returns it
            if (child.name == name)
            {
                return child;
            }
        }

        return null;
    }
    #endregion

    #region Challenge Settings

    /// <summary>
    /// Sets all the information and values of the top challenge interface
    /// </summary>
    public void SetChallengeSuperior()
    {
        // Activates this interface in the Game scene canvas
        this.gameObject.SetActive(true);

        GameObject temp;

        // If this has a child named Challange
        if (temp = SearchChild("Challenge"))
        {
            Text t = temp.GetComponent<Text>();
            if (t != null)
            {
                // Sets the value, style and size of Challenge text (title of the challenge)
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

    /// <summary>
    /// Sets all the information and values of the bottom challenge interface
    /// </summary>
    public void SetChallengeInferior()
    {
        // Activates this interface in the Game scene canvas
        this.gameObject.SetActive(true);

        GameObject temp;
        // If this has a child named Counter sets the value of the counter
        if ((temp = SearchChild("Counter")) != null)
        {
            _challengeCounter = temp.GetComponent<Text>();
        }
        else
        {
            ErrorObjectNotFound("Counter");
        }

        _playing = true;
    }

    /// <summary>
    /// Sets the challenge like ended
    /// </summary>
    public void ChallengeCompleted()
    {
        _playing = false;
    }
    #endregion

    /// <summary>
    /// Shows an error of a missing interface object
    /// </summary>
    /// <param name="objectName">Name of the object</param>
    void ErrorObjectNotFound(string objectName)
    {
        Debug.LogError("Missing interface object: " + objectName + ". In interface component: " + _type);
    }

    /// <summary>
    /// Gets the actual type of this interface
    /// </summary>
    /// <returns>Type of this interface</returns>
    public InterfaceType getType()
    {
        return _type;
    }
}
