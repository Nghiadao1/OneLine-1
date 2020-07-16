using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateGameInfo
{
    [MenuItem("Create/Game Info")]
    static void CreateInfo()
    {
        GameFilesInfo inf = new GameFilesInfo();
        DirectoryInfo dir;

        // Conseguimos el número de dificultades
        dir = new DirectoryInfo("Assets/StreamingAssets/Levels/Difficulties/");
        FileInfo[] infoDifficulties = dir.GetFiles("*.json");

        // Conseguimos el número de skins del camino de pistas
        dir = new DirectoryInfo("Assets/Prefabs/Game/Paths/PathSkin/");
        FileInfo[] infoPathSkins = dir.GetFiles("*.prefab");

        // Conseguimos el número de skins del feedback del input
        dir = new DirectoryInfo("Assets/Prefabs/Game/Touch/");
        FileInfo[] infoTouchSkins = dir.GetFiles("*.prefab");

        // Conseguimos la info del número de skins de los tiles
        dir = new DirectoryInfo("Assets/Prefabs/Game/Tiles/TileSkin/");
        FileInfo[] infoTileSkins = dir.GetFiles("*.prefab");

        // Volcamos la información en la clase 
        inf._numDifficulties = infoDifficulties.Length;
        inf._numPathSkins = infoPathSkins.Length;
        inf._numTouchSkins = infoTouchSkins.Length;
        inf._numTileSkins = infoTileSkins.Length;

        // Crear el archivo JSON
        string gameInfo = JsonUtility.ToJson(inf);

        FileStream file = File.Create("Assets/StreamingAssets/game_data.json");

        StreamWriter sw = new StreamWriter(file);

        sw.Write(gameInfo);

        sw.Close();
        sw.Dispose();
        file.Close();
        file.Dispose();
    }
}
