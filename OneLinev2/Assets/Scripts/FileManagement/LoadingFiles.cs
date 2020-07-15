using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

/// <summary>
/// Toda esta parte es una fumada de narices eh, pero no pasa nada
/// </summary>
[System.Serializable]
public class GameInfo
{
    public int _numDifficulties;
    public int _numPathSkins;
    public int _numTouchSkins;
    public int _numTileSkins;
}

// Referentes al jugador
[System.Serializable]
public struct PlayerData
{
    public int _coinsPlayer;
    public int[] _completedLevelsInDifficulty;
    public int _challengesCompleted;
    public int _timeForNextChallenge;
    public int _dateForNextChallenge;
    public int _lastClosed;
    public int _levelsPlayed;

    public bool _paid;

    private int _hash;

    public PlayerData(int coins, int[] completed, int challengesComp, int timeNextChallenge, int dateForChallenge, int dateClosed, bool paid)
    {
        _coinsPlayer = coins;
        _completedLevelsInDifficulty = completed;
        _challengesCompleted = challengesComp;
        _timeForNextChallenge = timeNextChallenge;
        _dateForNextChallenge = dateForChallenge;
        _lastClosed = dateClosed;
        _paid = paid;

        _hash = 0;
        _levelsPlayed = coins;
    }

    public int GetHash()
    {
        return _hash;
    }

    public void SetHash(int h)
    {
        _hash = h;
    }
}

public class LoadingFiles
{
    static private int fileNum = 0573;

    public static GameInfo ReadGameInfo()
    {
        GameInfo info = new GameInfo();
        string path = Application.streamingAssetsPath + "/game_data.json";
        string gameData = null;

#if !UNITY_EDITOR && UNITY_ANDROID
        WWW loadingAssets = new WWW(path);
        while (!loadingAssets.isDone) { }

        gameData = loadingAssets.text;
#else
        if (File.Exists(path))
        {
            gameData = File.ReadAllText(path);
        }
        else
        {
            Debug.LogError("game_data.json Not Found, File not Created");
        }
#endif
        if (gameData != null)
        {
            info = JsonUtility.FromJson<GameInfo>(gameData);
        }
        else
        {
            Debug.LogError("File not read correctly");
        }

        return info;
    }

    public static PlayerData ReadPlayerData(int maxDifficulty)
    {
        if (File.Exists(Application.persistentDataPath + "/misc/surrender.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/misc/surrender.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);

            // Leemos la sal
            int levelsPlayed = data._levelsPlayed;
            int hash = data.GetHash();

            data.SetHash(0); // Reiniciamos el hash para volver a generarlo
            int checkLevelsPlayed = fileNum + data._coinsPlayer;
            int checkHash = Encrypt(bf, data);
            file.Close();

            if (hash == checkHash && levelsPlayed == checkLevelsPlayed)
            {
                return data;
            }
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

    public static PlayerData NewPlayerData(int maxDifficulty)
    {
        PlayerData data = new PlayerData(0, new int[maxDifficulty], 0, 0, 0, 0, false);

        for (int i = 0; i < data._completedLevelsInDifficulty.Length; i++)
        {
            data._completedLevelsInDifficulty[i] = 1;
        }

        return data;
    }

    public static void SavePlayerData(PlayerData pd)
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (!Directory.Exists(Application.persistentDataPath + "/misc/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/misc/");
        }

        FileStream file = File.Create(Application.persistentDataPath + "/misc/surrender.dat");

        pd._levelsPlayed = fileNum + pd._coinsPlayer;

        if (pd.GetHash() != 0)
        {
            pd.SetHash(0);
        }

        pd.SetHash(Encrypt(bf, pd));

        bf.Serialize(file, pd);

        file.Close();
    }

    public static int Encrypt(BinaryFormatter b, PlayerData pd)
    {
        MemoryStream ms = new MemoryStream();

        b.Serialize(ms, pd);

        ms.Seek(0, SeekOrigin.Begin);

        byte[] bytes = new byte[ms.Length];
        return ms.Read(bytes, 0, (int)ms.Length.GetHashCode());
    }
}
