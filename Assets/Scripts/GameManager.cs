using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// Auí hay ue meter lo de los anunsios para los dineros
// Tambien la lectura de niveles y el paso de escenas
public class GameManager : MonoBehaviour
{
    // Welcome to the GameManager script, enjoy the visit and left some comments below. 

    #region Atributes
    int coins;
    LevelReader levels;
    int actLevel;
    string currentDifficulty;
    int numDifficulty;
    int[] completedLevels;
    int numDifficulties = 5;
    bool paid; // This is for the ads
    #endregion

    #region SingletonInstance
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        //TODO: INICIAR CON LOS DATOS GUARDADOS
        completedLevels = new int[numDifficulties];

        int numLevelsCompleted;
        for (int i = 0; i < numDifficulties; i++)
        {
            numLevelsCompleted = 0; //Lectura

            completedLevels[i] = numLevelsCompleted;
        }

        currentDifficulty = "";

        levels = null;

        coins = 0;
    }
    #endregion

    #region GameManagement
    // Information and management of game information, varibles, etc.

    /// <summary>
    /// Loads the information of the player saved in the computer/phone. 
    /// </summary>
    private void LoadGameInfo()
    {

    }

    /// <summary>
    /// Adds coins to the player's wallet. Can be personalized on the Editor.
    /// </summary>
    /// <param name="sum"> Number of coins added </param>
    public void AddCoins(int sum)
    {
        coins += sum;
    }

    /// <summary>
    /// Returns the number of coins that the player currently has. 
    /// </summary>
    /// <returns> Current number of coins in Player's wallet </returns>
    public int GetCoins()
    {
        return coins;
    }

    /// <summary>
    /// Process a Hint. Take 25 coins from the player's wallet. 
    /// </summary>
    public void Hint()
    {
        coins -= 25;
    }

    /// <summary>
    /// Search for a specific level in the levels array and returns it as a struct. 
    /// </summary>
    /// <returns> Struct of the level with a specific index. </returns>
    public Levels InitActualLevel()
    {
        return levels.GetLevel(actLevel);
    }

    /// <summary>
    /// Returns in which difficulty is the player at this moment. Used to put it on the 
    /// canvas. 
    /// </summary>
    /// <returns> string: Difficulty selected </returns>
    public string GetCurrentDifficultyText()
    {
        return currentDifficulty;
    }

    /// <summary>
    /// Returns the number of difficulty to use it for calculations, etc. 
    /// </summary>
    /// <returns> Current number difficulty </returns>
    public int GetCurrentNumDifficulty()
    {
        return numDifficulty;
    }

    /// <summary>
    /// Adds one to the completed levels when the player gets to complete one of
    /// them. 
    /// </summary>
    public void CompleteLevel()
    {
        completedLevels[numDifficulty]++;
    }

    /// <summary>
    /// Returns the number of levels in the current difficulty.
    /// </summary>
    /// <returns> int: number of levels/ -1 if there are no levels loaded</returns>
    public int GetTotalLevels()
    {
        if (levels != null)
        {
            return levels.GetNumLevels();
        }
        else
        {
            Debug.LogError("Levels not loaded");
            return -1;
        }
    }

    /// <summary>
    /// Access to the list of completed levels. Needed to represent them in the
    /// main Menu and to represent them in the Level Selection. 
    /// </summary>
    /// <returns> int number of levels completed in current difficulty </returns>
    public int[] GetCompletedLevels()
    {
        return completedLevels;
    }

    /// <summary>
    /// Access to the list of completed levels. Needed to represent them in the
    /// main Menu and to represent them in the Level Selection. 
    /// </summary>
    /// <returns> int number of levels completed in current difficulty </returns>
    public int GetCurrentCompletedLevels()
    {
        return completedLevels[numDifficulty];
    }
    #endregion

    #region SceneChangeManagement
    /// <summary>
    /// Used to return to the Main Menu Scene. Restablishes the variables and sets
    /// the values to the original. 
    /// </summary>
    public void ReturnToMainMenu()
    {
        currentDifficulty = ""; // No current scene
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Changes to the Level scene. Then, the LevelManager will handle the creation and
    /// setup of the whole scene (Board creation and everything else).
    /// </summary>
    public void ChangeLevelScene(int level)
    {
        actLevel = level;
        SceneManager.LoadScene("Level");
    }

    /// <summary>
    /// Changes to the LevelSelectionScreen. Receives the difficulty in a string to 
    /// put it in the canvas. 
    /// </summary>
    /// <param name="difficulty"> String difficulty text name. </param>
    public void ChangeLevelSelectionScreen(string difficulty)
    {
        
        if (currentDifficulty == "")
        {
            currentDifficulty = difficulty;
        }

        Debug.Log(Application.dataPath + "/Levels/" + currentDifficulty + ".json");

        levels = new LevelReader(Application.dataPath + "/Levels/" + currentDifficulty + ".json", currentDifficulty);

        Debug.Log(levels);

        SceneManager.LoadScene("LevelSelection");
    }

    public void SetDifficultyNumber(int diff)
    {
        numDifficulty = diff;
    }
    #endregion

    #region ApplicationLifeManagement
    public void ExitGame()
    {
        Application.Quit();
    }

    void SaveState()
    {
        // Pásalo todo a JASON amego
    }

    private void OnApplicationQuit()
    {
        SaveState();
    }
    #endregion
}
