using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{
    public State state;
    public Arc arc;
    public Dictionary<string, int> ghostNameToStoryIndex;
    public Dictionary<string, bool> hasOpenedTutorial;
}

public class SaveManager
{
    private static string fileName = "savegame.json";
    private static string FullPath => Path.Combine(Application.persistentDataPath, fileName);

    public static void SaveGame(SaveData saveData)
    {
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        try
        {
            File.WriteAllText(FullPath, json);
            Debug.Log($"Saved to {FullPath}");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save: " + e);
        }
    }

    public static SaveData LoadGame()
    {
        if (!HasSaveFile())
        {
            Debug.Log("No save file found.");
            return null; 
        }

        try
        {
            string json = File.ReadAllText(FullPath);
            Debug.Log(FullPath);
            SaveData data = JsonConvert.DeserializeObject<SaveData>(json);
            Debug.Log("Loaded save.");
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load: " + e);
            return null;
        }
    }

    public static void DeleteSave()
    {
        if (File.Exists(FullPath)) File.Delete(FullPath);
    }

    public static bool HasSaveFile()
    {
        if (!File.Exists(FullPath))
        {
            return false;
        }
        return true;

    }
}