using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData
{
    public string Name;
    public long Gold;
    public int Level;
}

public class SaveController : MonoBehaviour
{
    private const string encryptionKey = "EncryptSaveData!@#$%^&*()_+";
    private const string saveFileName = "SaveData.json";

    private string path;

    public SaveData Data;

    public void Init()
    {
        path = Path.Combine(Application.persistentDataPath, saveFileName);

        if (SaveFileCheck() == true)
        {
            LoadData();
        }
        else
        {
            CreateNewSaveData();
        }
    }

    public void SaveData()
    {
        Debug.Log("Save Data");

        string json = JsonUtility.ToJson(Data);
        File.WriteAllText(path, EncryptAndDecrypt(json));
    }

    public void LoadData()
    {
        Debug.Log("Load Data");

        string json = File.ReadAllText(path);
        Data = JsonUtility.FromJson<SaveData>(EncryptAndDecrypt(json));
    }

    public bool SaveFileCheck()
        => File.Exists(path);

    private void CreateNewSaveData()
    {
        Debug.Log("Create New Save Data");

        Data = new SaveData();
        Data.Name = "Player";
        Data.Gold = 0;
        Data.Level = 1;

        SaveData();
    }

    private string EncryptAndDecrypt(string data)
    {
        string result = string.Empty;

        for (int i = 0; i < data.Length; i++)
            result += (char)(data[i] ^ encryptionKey[i % encryptionKey.Length]);

        return result;
    }
}
