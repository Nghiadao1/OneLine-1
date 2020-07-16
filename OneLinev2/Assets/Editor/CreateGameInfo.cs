using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Option accesible from the editor to create a file that stores the
/// information of the different assets. This is for later use when 
/// generating random skins and taking the prefabs from the AssetBundles. 
/// 
/// It's made to make easier the process to detect the different prefabs. 
/// It's also used to control the different files that are needed for the 
/// game to work correctly. Checks if all the difficulties are in the 
/// correct folder. 
/// </summary>
public class CreateGameInfo
{
    /// <summary>
    /// Creates the folder JSON that will store the information about 
    /// the game. 
    /// </summary>
    [MenuItem("Create/Game Info")]
    public static void CreateInfo()
    {
        // GameInfo class for Serialization
        GameFilesInfo inf = new GameFilesInfo();
        // Directory info for retrieving number of files
        DirectoryInfo dir;

        // Check how many Difficulty files are in Levels folder
        dir = new DirectoryInfo("Assets/StreamingAssets/Levels/Difficulties/");
        FileInfo[] infoDifficulties = dir.GetFiles("*.json");

        // Retrieve the number of pathSkins there are for later calculations
        dir = new DirectoryInfo("Assets/Prefabs/Game/Paths/PathSkin/");
        FileInfo[] infoPathSkins = dir.GetFiles("*.prefab");

        // Number of skins for the touching screen feedback
        dir = new DirectoryInfo("Assets/Prefabs/Game/Touch/");
        FileInfo[] infoTouchSkins = dir.GetFiles("*.prefab");

        // Tile skins defined in the Prefab folder
        dir = new DirectoryInfo("Assets/Prefabs/Game/Tiles/TileSkin/");
        FileInfo[] infoTileSkins = dir.GetFiles("*.prefab");

        // Save that info in the class
        inf._numDifficulties = infoDifficulties.Length;
        inf._numPathSkins = infoPathSkins.Length;
        inf._numTouchSkins = infoTouchSkins.Length;
        inf._numTileSkins = infoTileSkins.Length;

        // Create JSON object
        string gameInfo = JsonUtility.ToJson(inf);

        // Write everything in the file
        FileStream file = File.Create("Assets/StreamingAssets/game_data.json");
        StreamWriter sw = new StreamWriter(file);
        sw.Write(gameInfo);

        // Close everything
        sw.Close();
        sw.Dispose();
        file.Close();
        file.Dispose();
    }
}
