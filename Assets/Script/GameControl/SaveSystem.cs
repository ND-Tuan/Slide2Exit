using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class PlayerData
{
    public int Coin;
    public List<int> SkinOwned;
    public int SkinEquipped;
    public float MusicVolume;
    public float SFXVolume;
    public List<int> GuideUnlocked;
}

public static class SaveSystem
{
    private static readonly string levelDataPath = Path.Combine(Application.persistentDataPath, "leveldata.dat");
    private static readonly string playerDataPath = Path.Combine(Application.persistentDataPath, "playerdata.dat");

    public static void SaveLevelData(LevelData levelData)
    {
        SaveToFile(levelDataPath, levelData);
    }

    public static LevelData LoadLevelData()
    {
        return LoadFromFile<LevelData>(levelDataPath);
    }

    public static void SavePlayerData(PlayerData playerData)
    {
        SaveToFile(playerDataPath, playerData);
    }

    public static PlayerData LoadPlayerData()
    {
        return LoadFromFile<PlayerData>(playerDataPath);
    }

    //====Hàm tiện ích dùng chung================
    private static void SaveToFile(string path, object data)
    {
        try
        {
            using FileStream stream = new FileStream(path, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
        }
        catch (IOException ex)
        {
            Debug.LogError($"Failed to save data to {path}: {ex.Message}");
        }
    }

    private static T LoadFromFile<T>(string path) where T : class
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Save file not found in {path}");
            return null;
        }

        try
        {
            using FileStream stream = new FileStream(path, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream) as T;
        }
        catch (IOException ex)
        {
            Debug.LogError($"Failed to load data from {path}: {ex.Message}");
            return null;
        }
    }
}
