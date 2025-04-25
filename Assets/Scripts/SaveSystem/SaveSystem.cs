using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/";
    public static readonly string FILE_NAME = "save.json";

    public static void SaveToFile(SaveData saveData)
    {
        Debug.Log("Saving game");

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(SAVE_FOLDER + FILE_NAME, json);
    }

    public static SaveData LoadFromFile()
    {
        Debug.Log("Loading game");

        SaveData saveData = new SaveData();

        if (DoesSaveFileExist())
        {
            string json = File.ReadAllText(SAVE_FOLDER + FILE_NAME);
            saveData = JsonUtility.FromJson<SaveData>(json);
        }
        else //if no save, create new one and load it
        {
            saveData = CreateNewSaveFile();
        }

        return saveData;
    }

    public static bool DoesSaveFileExist()
    {
        if (File.Exists(SAVE_FOLDER + FILE_NAME)) return true;
        else return false;
    }

    public static SaveData CreateNewSaveFile()
    {
        SaveData saveData = new SaveData();
        SaveToFile(saveData);
        return saveData ;
    }
}
