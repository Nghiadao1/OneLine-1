using System.IO;
using UnityEngine;

/// <summary>
/// Class that stores the information of 1 level. This can be used to load
/// different levels from the JSON file "levels" and save them in an array
/// of Levels to retrieve their information later. 
/// </summary>
[System.Serializable]
public class Levels
{
    public int index;        // Level number or index
    public string[] layout;  // Map of the level 
    public Vector2[] path;   // Solution
}

/// <summary>
/// Class that stores the complete list of all the levels in the current difficulty.
/// </summary>
[System.Serializable]
public class LevelList
{
    public Levels[] Levels;
}

/// <summary>
/// The LevelReader class is in charge for loading the levels from their files 
/// and saving their information for later use. 
/// 
/// Searchs for the files in the StreaminsAssets folder and checks if they exist
/// or not and then reads their information to serialize in JSON format. Then
/// assign this JSON to a variable that stores that information. 
/// 
/// It also returns the information of the difficulty loaded to create the levels and
/// manage the information.
/// </summary>
public class LevelReader
{
    // All the levels of a specific difficulty
    LevelList list;

    /// <summary>
    /// This function is the Constructor of the class. Reads a Loads the information
    /// of a difficulty from one folder into the levels information variables to use them. 
    /// 
    /// The information is retrieved differently deppending on the platform the game is playing
    /// because the folder estructure is different. 
    /// </summary>
    /// <param name="diff"></param>
    public LevelReader(int diff)
    {
        // Path to find the file and data to read from the file
        string filePath = Application.streamingAssetsPath + "/Levels/Difficulties/" + diff + ".json";
        string data = null;

#if !UNITY_EDITOR && UNITY_ANDROID
        // Get the .jar file and decompress it to load it in text value
        WWW readLevel = new WWW(filePath);
        while (!readLevel.isDone) {  }

        // Read the information
        data = readLevel.text;
#else
        // Check if the file exists and notify if not
        if (File.Exists(filePath))
        {
            // Load the info
            data = File.ReadAllText(filePath);
        }
        else
        {
            Debug.LogError("Can not find data");
        }
#endif
        // Serialize that information and convert it to JSON type file to load it in the variables
        list = JsonUtility.FromJson<LevelList>(data);
    }

    /// <summary>
    /// Gives information about how many levels this difficulty has. 
    /// </summary>
    /// <returns>The number of levels this difficulty has</returns>
    public int GetNumLevels()
    {
        return list.Levels.Length;
    }

    /// <summary>
    /// Returns a specific level from the level list
    /// </summary>
    /// <param name="level">Number of level from 1 - maxLevel</param>
    /// <returns>The level asked for</returns>
    public Levels GetLevel(int level)
    {
        return list.Levels[level - 1];
    }
}
