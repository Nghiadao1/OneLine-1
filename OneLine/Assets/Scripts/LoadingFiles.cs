using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public static class LoadingFiles
{


    public static PlayerData ReadPlayerData(int maxDifficulty)
    {
        if (File.Exists(Application.persistentDataPath + "/surrender.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/surrender.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            return data;
        }
        else
        {
            PlayerData data = new PlayerData(0, new int[maxDifficulty], 0, 0, 0, 0, false);

            for (int i = 0; i < data._completedLevelsInDifficulty.Length; i++)
            {
                data._completedLevelsInDifficulty[i] = 1;
            }

            return data;
        }
    }

    public static void SavePlayerData(PlayerData pd)
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "surrender.dat");

        bf.Serialize(file, pd);

        file.Close();
    }
}
