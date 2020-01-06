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
    Levels levels;
    int actLevel;
    string currentDifficulty;
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
    public Levels.Level InitActualLevel()
    {
        return levels.GetLevel(actLevel);
    }

    /// <summary>
    /// Returns in which difficulty is the player at this moment. Used to put it on the 
    /// canvas. 
    /// </summary>
    /// <returns> string: Difficulty selected </returns>
    public string GetCurrentDifficulty()
    {
        return currentDifficulty;
    }

    public void CompleteLevel()
    { // TODO: Repensar esto m8
        switch (currentDifficulty)
        {
            case "Beginner":
                completedLevels[0]++;
                break;
            case "Regular":
                completedLevels[1]++;
                break;
            case "Advanced":
                completedLevels[2]++;
                break;
            case "Expert":
                completedLevels[3]++;
                break;
            case "Master":
                completedLevels[4]++;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Access to the list of completed levels. Needed to represent them in the
    /// main Menu and to represent them in the Level Selection. 
    /// </summary>
    /// <returns> int[] number of levels completed per difficulty </returns>
    public int[] GetCompletedLevels()
    {
        return completedLevels;
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
    public void ChangeLevelScene()
    {
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

        levels = new Levels(Application.dataPath + "/Levels/" + currentDifficulty + ".json", currentDifficulty);

        SceneManager.LoadScene("LevelSelection");
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
