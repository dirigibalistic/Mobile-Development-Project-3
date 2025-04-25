using UnityEngine;

public class SaveManager : MonoBehaviour
{
    #region Singleton Pattern

    private static SaveManager _instance;
    public static SaveManager Instance
    {
        get
        {
            //when accessed, check if instance exists, create if necessary
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<SaveManager>();
                if(_instance == null)
                {
                    GameObject newGO = new GameObject();
                    _instance = newGO.AddComponent<SaveManager>();
                    newGO.name = "DataManager";
                    DontDestroyOnLoad(newGO);
                }
            }
            return _instance;
        }
    }

    #endregion

    public SaveData ActiveSaveData { get; private set; } = new SaveData();
    public void Save()
    {
        SaveSystem.SaveToFile(ActiveSaveData);
    }
    public void Load()
    {
        ActiveSaveData = SaveSystem.LoadFromFile();
    }
    public void ResetSave()
    {
        ActiveSaveData = SaveSystem.CreateNewSaveFile();
    }
}
