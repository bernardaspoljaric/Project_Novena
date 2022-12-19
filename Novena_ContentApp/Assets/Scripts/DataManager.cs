using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class DataManager : MonoBehaviour
{
    public TranslatedContents translatedContents;
    private string jsonFileName = "example.json";
    private string folderName = "files";

    public AppData appData;


    private void Start()
    {
        SaveJsonToPersistentDataPath();

        ReadJsonFromPersistentDataPath();

        // Error
        //SaveFolderToPersistentDataPath();
        //ReadFolderFromPersistentDataPath();

    }

    // method for copying json file from SteamingAssets to PersistentDataPath
    public void SaveJsonToPersistentDataPath()
    {
        string json = File.ReadAllText(Application.streamingAssetsPath + "/" + jsonFileName);
        WriteToFile(jsonFileName, json);
    }

    // method for getting json file from PersistentDataPath 
    public void ReadJsonFromPersistentDataPath()
    {
        translatedContents = new TranslatedContents();
        string json = ReadFromFile(jsonFileName);
        JsonUtility.FromJsonOverwrite(json, translatedContents);
        File.WriteAllText(Application.dataPath + "/Resources/" + jsonFileName, json);
        appData.textJson = Resources.Load("example") as TextAsset;
        appData.StartAppData();
    }

    // method for writing a json file
    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.WriteLine(json);
        };

        Debug.Log("Saved");
    }

    // method for reading a json file
    private string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return json;
        }
        else
        {
            Debug.Log("File not found");
            return "";
        }
    }

    public void SaveFolderToPersistentDataPath()
    {
        byte[] folder = File.ReadAllBytes(Application.streamingAssetsPath + "/" + folderName);
        WriteFolderToPersistentDataPath(jsonFileName, folder);
    }

    public void ReadFolderFromPersistentDataPath()
    {
        byte[] folder = ReadFolderFromFile(folderName);
        
        File.WriteAllBytes(Application.dataPath + "/Resources/" + folderName, folder);
        
    }

    private void WriteFolderToPersistentDataPath(string folderName, byte[] folder)
    {
        string path = GetFilePath(folderName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.WriteLine(folderName);
        };

        Debug.Log("Saved");
    }


    private byte[] ReadFolderFromFile(string folderName)
    {
        string path = GetFilePath(folderName);
        if (File.Exists(path))
        {
            byte[] f = File.ReadAllBytes(path);
            return f;
        }
        else
        {
            Debug.Log("File not found");
            return null;
        }
    }

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath+ "/" + fileName;
    }

   
}

 