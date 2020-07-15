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
    public Vector2[] path; // Solution
}

[System.Serializable]
public class LevelList
{
    public Levels[] Levels;
}

public class LevelReader
{
    LevelList list;

    public LevelReader(int diff)
    {
        string filePath = Application.streamingAssetsPath + "/Levels/Difficulties/" + diff + ".json";
        string data = null;

#if !UNITY_EDITOR && UNITY_ANDROID
        WWW readLevel = new WWW(filePath);
        while (!readLevel.isDone) {  }

        data = readLevel.text;
#else
        if (File.Exists(filePath))
        {
            data = File.ReadAllText(filePath);
        }
        else
        {
            Debug.LogError("Can not find data");
        }
#endif

        list = JsonUtility.FromJson<LevelList>(data);
    }

    public int GetNumLevels()
    {
        return list.Levels.Length;
    }

    public Levels GetLevel(int level)
    {
        return list.Levels[level - 1];
    }
}
