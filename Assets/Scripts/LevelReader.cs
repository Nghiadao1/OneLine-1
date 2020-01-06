using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Struct that stores the information of 1 level. This can be used to load
/// different levels from the JSON file "levels" and save them in an array
/// of Levels to retrieve their information later. 
/// </summary>
[System.Serializable]
public class Levels
{
    public int index; // Level number or index
    public string[] layout; // Map of the level 
    public int[][] path; // Solution
}

[System.Serializable]
public class LevelList
{
    public Levels[] Levels;
}

public class LevelReader
{
    #region Atributes
    LevelList list;
    string difficulty;
    #endregion

    public LevelReader(string filePath, string diff)
    {
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);

            list = JsonUtility.FromJson<LevelList>(data);
            difficulty = diff;
        }
        else
        {
            Debug.LogError("Cannot find data");
        }
    }
    public int GetNumLevels()
    {
        return 0;
    }

    public Levels GetLevel(int level)
    {
        return new Levels();
    }
}
