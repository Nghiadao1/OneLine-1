using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

/// <summary>
/// Configuration of the game through data files in JSON. This class
/// stores all the information related to the game configuration. 
/// </summary>
[System.Serializable]
public class GameConfig
{
    //values of different things needed in the game, set in default values
    public int _maxDifficulty;        // Maximum difficulties, number of difficulties
    public string[] _difficultyTexts; // name of the difficulties in string
    public int _hintCost;             // price to be paid for a hint
    public int _coinsMaxReward;       // coins maximum reward after ad
    public int _challengeWaitTime;    // time player must wait between challenges
    public int _challengeReward;      // coins reward after winning a challenge
    public int _challengeCost;        // price to pay for a challenge without seeing an ad
}

/// <summary>
/// This class is used to access to the control some things of the game files. It 
/// stores the information of how many level files are in the directory which contains
/// the levels.json files. Is used to control if the number set in _maxDifficulty in
/// GameConfig matches the number of level files stored in the directory. 
/// 
/// The rest of the variables (_numPathSkins, _numTouchSkins and _numTileSkins) are 
/// used for random Tile and skins selection later. They are used for saving some 
/// space by not including the prefab folders in StreamingAssets folder. Makes easier
/// the access to AssetBundles.
/// </summary>
[System.Serializable]
public class GameFilesInfo
{
    public int _numDifficulties;      // Number of files in /StreamingAssets/Levels/Difficulties
    public int _numPathSkins;         // number of prefabs in paths skins folder
    public int _numTouchSkins;        // number of touching skins in it's folder
    public int _numTileSkins;         // number of tiles skins in Tile's folder
}

/// <summary>
/// Class that stores the information of the player and will later use it to 
/// config the game and set the initial values of some things. 
/// 
/// This class will be used also for saving the players information
/// </summary>
[System.Serializable]
public struct PlayerData
{
    // Player's information 
    public int _coinsPlayer;                   // Coins that the player has
    public int[] _completedLevelsInDifficulty; // levels completed by the player
    public int _challengesCompleted;           // challenges completed succesfully 
    public int _dateForNextChallenge;          // date for the next challenge
    public int _levelsPlayed;                  // how many levels played in this match
    public bool _paid;                         // if the player paid for no ads
    private int _hash;                         // hash code 

    /// <summary>
    /// Constructor of the struct that stores the information of the player. Receives
    /// the values that will form the Player info and stores it.
    /// </summary>
    /// <param name="coins">Coins the player has</param>
    /// <param name="completed">Levels completed in each difficulty</param>
    /// <param name="challengesComp">Number of challenges completed</param>
    /// <param name="dateForChallenge">Date for the next challenge</param>
    /// <param name="paid">If player paid or not for the game</param>
    public PlayerData(int coins, int[] completed, int challengesComp, int dateForChallenge, bool paid)
    {
        _coinsPlayer = coins;
        _completedLevelsInDifficulty = completed;
        _challengesCompleted = challengesComp;
        _dateForNextChallenge = dateForChallenge;
        _paid = paid;

        _hash = 0;
        _levelsPlayed = coins;
    }

    /// <summary>
    /// Give access to the hash. Consult only. 
    /// </summary>
    /// <returns>int Hash actual value</returns>
    public int GetHash()
    {
        return _hash;
    }

    /// <summary>
    /// Used to set the new hash value after calculation. 
    /// </summary>
    /// <param name="h">int New hash value</param>
    public void SetHash(int h)
    {
        _hash = h;
    }
}

/// <summary>
/// Class used to extract information stored in text files, for example .json 
/// or .data files with binary information. 
/// 
/// It is also used to save information in new files. 
/// </summary>
public class LoadingFiles
{
    // Static value used to code some info
    static private int fileNum = 0573; // Salt

    /// <summary>
    /// Reads the game configuration stored in the game_config file 
    /// and stores it to use it in the game and GameManager. 
    /// </summary>
    /// <returns>GameConfig configuration of the game</returns>
    public static GameConfig ReadGameConfig()
    {
        // Value to return, GameCOnfiguration loaded
        GameConfig conf = new GameConfig();

        // Path and string to save the text in the file and convert it
        string path = Application.streamingAssetsPath + "/game_config.json";
        string gameConf = null;

#if !UNITY_EDITOR && UNITY_ANDROID
        // Android uses .jar files, it's necessary to unpack this as an url
        WWW loadingConfig = new WWW(path);
        while (!loadingConfig.isDone) { }

        // Read all the text
        gameConf = loadingConfig.text;
#else
        // Check if the file exists and then try to read it 
        if (File.Exists(path))
        {
            gameConf = File.ReadAllText(path);
        }
        else
        {
            Debug.LogError("GameConfiguration not exists, create game_config.json file with the information"); // CAMBIAR ESTO
        }
#endif
        // Check if the value in the file is not null
        if(gameConf != null)
        {
            conf = JsonUtility.FromJson<GameConfig>(gameConf);
        }
        else
        {
            Debug.LogError("Archivo no leído");
        }

        // Return the configuration read
        return conf;
    }

    /// <summary>
    /// Reads the information estracted from the directories and files of 
    /// the game. Used to control values.
    /// </summary>
    /// <returns>GameFilesInfo info of the files in the directories</returns>
    public static GameFilesInfo ReadGameInfo()
    {
        // Value that will be returned
        GameFilesInfo info = new GameFilesInfo();

        // Path to the file and string to store the text for later conversion
        string path = Application.streamingAssetsPath + "/game_data.json";
        string gameData = null;

#if !UNITY_EDITOR && UNITY_ANDROID
        // Extract form .jar file as an url
        WWW loadingAssets = new WWW(path);
        while (!loadingAssets.isDone) { }

        // Save that text 
        gameData = loadingAssets.text;
#else
        // Check if the file in the path exists
        if (File.Exists(path))
        {
            gameData = File.ReadAllText(path);
        }
        else
        {
            Debug.LogError("game_data.json Not Found, File not Created");
        }
#endif
        // Check if the value in the text is not null
        if (gameData != null)
        {
            info = JsonUtility.FromJson<GameFilesInfo>(gameData);
        }
        else
        {
            Debug.LogError("File not read correctly");
        }

        // Return that info
        return info;
    }

    /// <summary>
    /// Reads the information of the player stored in a Binary file. If the file dows not 
    /// exists, creates a new Player profile and returns it with the new values. 
    /// </summary>
    /// <param name="maxDifficulty">Maximum difficulty for generating the new player</param>
    /// <returns>PlayerData With new or read data</returns>
    public static PlayerData ReadPlayerData(int maxDifficulty)
    {
        // Check if file exists
        if (File.Exists(Application.persistentDataPath + "/surrender.dat"))
        {
            // Binary formatter for conversion 
            BinaryFormatter bf = new BinaryFormatter();

            // File from which will be read the information of the player
            FileStream file = File.Open(Application.persistentDataPath + "/surrender.dat", FileMode.Open);

            // Convert binary to PlayerData information
            PlayerData data = (PlayerData)bf.Deserialize(file);

            // Read levels info and hash
            int levelsPlayed = data._levelsPlayed;
            int hash = data.GetHash();

            // Restart the hash to generate new and check 
            data.SetHash(0);
            int checkLevelsPlayed = fileNum + data._coinsPlayer;
            int checkHash = Encrypt(bf, data);
            file.Close();

            // If everything is correct and hash is the same, return this PlayerData
            if (hash == checkHash && levelsPlayed == checkLevelsPlayed)
            {
                return data;
            }
            // If not, generate a new one
            else
            {
                return NewPlayerData(maxDifficulty);
            }
        }
        else
        {
            return NewPlayerData(maxDifficulty);
        }
    }

    /// <summary>
    /// Generates a new Player profile with new information and data. Sets all
    /// necessary values to 0 and prepares it for a new use. 
    /// </summary>
    /// <param name="maxDifficulty">Maximum difficulty to create arrays</param>
    /// <returns>PlayerData With a new player created</returns>
    public static PlayerData NewPlayerData(int maxDifficulty)
    {
        // The new player
        PlayerData data = new PlayerData(0, new int[maxDifficulty], 0, 0, false);

        // Initialize the levels so that they can play the first level
        for (int i = 0; i < data._completedLevelsInDifficulty.Length; i++)
        {
            data._completedLevelsInDifficulty[i] = 1;
        }

        return data;
    }

    /// <summary>
    /// Saves the PlayerData received into a binary file. Applies everything
    /// to encrypt the information and then writes all bytes in the file.  
    /// </summary>
    /// <param name="pd">PlayerData to save</param>
    public static void SavePlayerData(PlayerData pd)
    {
        // Initialize BinaryFormatter
        BinaryFormatter bf = new BinaryFormatter();

        // Create the new player save file
        FileStream file = File.Create(Application.persistentDataPath + "/surrender.dat");

        // Save the levels played with the coins (Salt)
        pd._levelsPlayed = fileNum + pd._coinsPlayer;

        // Reset the hash for new codification
        if (pd.GetHash() != 0)
        {
            pd.SetHash(0);
        }

        // Create new hash code and write info in the file
        pd.SetHash(Encrypt(bf, pd));
        bf.Serialize(file, pd);

        file.Close();
    }

    /// <summary>
    /// Creates a new hash to encrypt the information in a file. 
    /// </summary>
    /// <param name="b">BinaryFormatter for serialization</param>
    /// <param name="pd"></param>
    /// <returns></returns>
    public static int Encrypt(BinaryFormatter b, PlayerData pd)
    {
        // Create the memorystream
        MemoryStream ms = new MemoryStream();

        // Serialize information in the memory stream
        b.Serialize(ms, pd);

        // Seek 0 value
        ms.Seek(0, SeekOrigin.Begin);

        // Create and return the hash code
        byte[] bytes = new byte[ms.Length];
        return ms.Read(bytes, 0, (int)ms.Length.GetHashCode());
    }
}
