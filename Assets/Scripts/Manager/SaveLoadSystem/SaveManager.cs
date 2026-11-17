using System.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{
    public Arc arc;
    public int arcProgress;
    public string currentSong;
    public Dictionary<string, int> ghostNameToStoryIndex;
    public Dictionary<string, bool> ghostNameToCompleted;
    public Dictionary<string, bool> hasOpenedTutorial;
}

public class SaveManager
{
    private static string fileName = "savegame.json";
    private static string FullPath => Path.Combine(Application.persistentDataPath, fileName);

    public static bool SaveGame(SaveData saveData)
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
            return false;
        }

        SaveSucceeded?.Invoke();
        return true;
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
        if (FileVerification())
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private static bool FileVerification()
    {
        try
        {
            string json = File.ReadAllText(FullPath);
            SaveData data = JsonConvert.DeserializeObject<SaveData>(json);
            if (data.ghostNameToStoryIndex.Keys.SequenceEqual(GameManager.Instance.ghostManager.ghostNameToStoryIndex.Keys) == false)
            {
                Debug.Log("Bad save: Story info not matching");
                return false;
            }
            if (data.ghostNameToCompleted.Keys.SequenceEqual(GameManager.Instance.ghostManager.GetGhostNameToCompleted().Keys) == false)
            {
                Debug.Log("Bad save: Completed ghosts not matching");
                return false;
            }
            if (((int) data.arc) < 0 || ((int) data.arc) >= 4)
            {
                Debug.Log("Bad save: Arc is bad");
                return false;
            }
            if (AudioManager.Instance.IsSongInDict(data.currentSong) == false)
            {
                Debug.Log("Bad save: Song not found");
                return false;
            }
            if (data.hasOpenedTutorial.Keys.SequenceEqual(GameManager.Instance.hasOpenedTutorial.Keys) == false)
            {
                Debug.Log("Bad save: Tutorial info not matching");
                return false;
            }
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public static event Action SaveSucceeded;
}