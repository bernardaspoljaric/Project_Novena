using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.IO;
using JetBrains.Annotations;

public class DataManager : MonoBehaviour
{
    public AppData appData;

    public string fileName = "example.txt";

    private void Start()
    {
        SaveFileToPersistentDataPath();
    }

    public void SaveFileToPersistentDataPath()
    {
        string json = File.ReadAllText(Application.streamingAssetsPath + "/example.json");
        Debug.Log(json);
        WriteToFile(fileName, json);
    }

    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);
        StreamWriter writer= new StreamWriter(fileStream);
        writer.Write(json); 
    }

    private string GetFilePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);  
    }

}
