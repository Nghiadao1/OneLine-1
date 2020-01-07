using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Saving information
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


// Auí hay ue meter lo de los anunsios para los dineros
// Tambien la lectura de niveles y el paso de escenas
public class GameManager : MonoBehaviour
{

    // Welcome to the GameManager script, enjoy the visit and left some comments below. 
    [System.Serializable]
    private struct PlayerData
    {
        public int coins;
        public bool[][] completedLevels;
        public bool paid;

        public PlayerData(int c, bool[][] lev, bool p)
        {
            coins = c;
            completedLevels = lev;
            paid = p;
        }
    }

    #region Atributes
    // Variables generales del juego 
    int coins;
    bool[][] completedLevels;
    bool paid; // This is for the ads


    // Variables con el estado actual del juego
    LevelReader levels;
    int actLevel;
    string currentDifficulty;
    int numCurrDiff = 0;
    #endregion

    #region SingletonInstance
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

            PlayerData dat = LoadState();

            instance.coins = dat.coins;
            instance.completedLevels = dat.completedLevels;
            instance.paid = dat.paid;

            instance.currentDifficulty = "";
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void SaveState()
    {
        BinaryFormatter bf = new BinaryFormatter();
        // The name of the file is random selected from a book
        FileStream file = File.Create(Application.persistentDataPath + "/pehmea.dat");

        PlayerData pd = new PlayerData(instance.coins, instance.completedLevels, instance.paid);

        bf.Serialize(file, pd);

        file.Close();
    }

    private PlayerData LoadState()
    {
        Debug.Log(Application.persistentDataPath + "/pehmea.dat");

        if (File.Exists(Application.persistentDataPath + "/pehmea.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/pehmea.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            return data;
        }
        else
        {
            PlayerData newPLayer = new PlayerData();

            newPLayer.coins = 0;
            newPLayer.paid = false;

            newPLayer.completedLevels = new bool [5][];

            for (int i = 0; i < 5; i++)
            {
                string diff = "";

                if(i == 0)
                {
                    diff = "Beginner";
                }
                else if (i == 1)
                {
                    diff = "Regular";
                } 
                else if (i == 2)
                {
                    diff = "Advanced";
                }
                else if (i == 3)
                {
                    diff = "Expert";
                }
                else if (i == 4)
                {
                    diff = "Master";
                }

                instance.levels = new LevelReader(Application.dataPath + "/Levels/" + diff + ".json", diff);

                newPLayer.completedLevels[i] = new bool[instance.levels.GetNumLevels()];

                for (int j = 0; j < instance.levels.GetNumLevels(); j++)
                {
                    newPLayer.completedLevels[i][j] = false;
                }
            }

            return newPLayer;
        }
    }

    /// <summary>
    /// This method will initialize all values of the GameManager with the information
    /// stored in the JSON archive that contains it. (Maybe we should hash it later?)
    /// </summary>
    private void InitGame()
    {

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
        instance.coins += sum;
    }

    /// <summary>
    /// Returns the number of coins that the player currently has. 
    /// </summary>
    /// <returns> Current number of coins in Player's wallet </returns>
    public int GetCoins()
    {
        return instance.coins;
    }

    /// <summary>
    /// Process a Hint. Take 25 coins from the player's wallet. 
    /// </summary>
    public void Hint()
    {
        if (instance.coins >= 25)
        {
            instance.coins -= 25;
        }
    }

    /// <summary>
    /// Search for a specific level in the levels array and returns it as a struct. 
    /// </summary>
    /// <returns> Struct of the level with a specific index. </returns>
    public Levels InitActualLevel()
    {
        return instance.levels.GetLevel(actLevel);
    }

    public int GetActualLevel()
    {
        return instance.actLevel;
    }

    /// <summary>
    /// Returns in which difficulty is the player at this moment. Used to put it on the 
    /// canvas. 
    /// </summary>
    /// <returns> string: Difficulty selected </returns>
    public string GetCurrentDifficultyText()
    {
        return instance.currentDifficulty;
    }

    /// <summary>
    /// Returns the number of difficulty to use it for calculations, etc. 
    /// </summary>
    /// <returns> Current number difficulty </returns>
    public int GetCurrentnumCurrDiff()
    {
        return instance.numCurrDiff;
    }

    /// <summary>
    /// Adds one to the completed levels when the player gets to complete one of
    /// them. 
    /// </summary>
    public void CompleteLevel()
    {
        if (SceneManager.GetActiveScene().GetRootGameObjects()[0].GetComponent<LevelManager>().CheckCompleted())
        {
            instance.completedLevels[numCurrDiff][actLevel] = true;
        }
    }

    /// <summary>
    /// Returns the number of levels in the current difficulty.
    /// </summary>
    /// <returns> int: number of levels/ -1 if there are no levels loaded</returns>
    public int GetTotalLevels()
    {
        if (instance.levels != null)
        {
            return instance.levels.GetNumLevels();
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
        int[] completedTotal = new int[5];

        for (int i = 0; i < instance.completedLevels.Length; i++)
        {
            instance.numCurrDiff = i;
            completedTotal[i] = GetCurrentCompletedLevels();
        }

        instance.numCurrDiff = 0;

        return completedTotal;
    }

    /// <summary>
    /// Access to the list of completed levels. Needed to represent them in the
    /// main Menu and to represent them in the Level Selection. 
    /// </summary>
    /// <returns> int number of levels completed in current difficulty </returns>
    public int GetCurrentCompletedLevels()
    {
        bool completed = true;
        int i = 0;

        while (completed && i < instance.completedLevels[instance.numCurrDiff].Length)
        {
            completed = instance.completedLevels[instance.numCurrDiff][i];
            if (completed)
            {
                i++;
            }
        }

        return i;
    }
    #endregion

    #region SceneChangeManagement
    /// <summary>
    /// Used to return to the Main Menu Scene. Restablishes the variables and sets
    /// the values to the original. 
    /// </summary>
    public void ReturnToMainMenu()
    {
        instance.currentDifficulty = ""; // No current difficulty
        instance.numCurrDiff = 0;
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Changes to the Level scene. Then, the LevelManager will handle the creation and
    /// setup of the whole scene (Board creation and everything else).
    /// </summary>
    public void ChangeLevelScene(int level)
    {
        instance.actLevel = level;
        SceneManager.LoadScene("Level");
    }

    public void NextLevel()
    {
        ChangeLevelScene(instance.actLevel + 1);
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene("Level");
    }

    public bool IsInLevel()
    {
        return SceneManager.GetActiveScene().name == "Level";
    }

    /// <summary>
    /// Changes to the LevelSelectionScreen. Receives the difficulty in a string to 
    /// put it in the canvas. 
    /// </summary>
    /// <param name="difficulty"> String difficulty text name. </param>
    public void ChangeLevelSelectionScreen(string difficulty)
    {
        
        if (instance.currentDifficulty == "")
        {
            instance.currentDifficulty = difficulty;
        }
        
        instance.levels = new LevelReader(Application.dataPath + "/Levels/" + instance.currentDifficulty + ".json", instance.currentDifficulty);

        SceneManager.LoadScene("LevelSelection");
    }

    public void SetDifficultyNumber(int diff)
    {
        instance.numCurrDiff = diff;
    }
    #endregion

    #region ApplicationLifeManagement
    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        SaveState();
    }
    #endregion
}
