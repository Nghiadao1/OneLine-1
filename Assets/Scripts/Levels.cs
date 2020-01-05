using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels
{
    /// <summary>
    /// Struct that stores the information of 1 level. This can be used to load
    /// different levels from the JSON file "levels" and save them in an array
    /// of Levels to retrieve their information later. 
    /// </summary>
    [System.Serializable]
    public struct Level
    {
        int _id; // Level number or index
        string[] _map; // Map of the level 
        int[,] _path; // Solution
    }

    string _difficulty;
    Level[] _levels;

    public Levels(string filePath, string difficulty)
    {
        _levels = JsonUtility.FromJson<Level[]>(filePath);
        _difficulty = difficulty;
    }

    public Level GetLevel(int level)
    {
        return _levels[level];
    }
}
