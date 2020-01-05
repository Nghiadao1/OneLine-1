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
    struct Level
    {
        int _id; // Level number or index
        string[] map; // Map of the level 
        int[,] path; // Solution
    }


}
