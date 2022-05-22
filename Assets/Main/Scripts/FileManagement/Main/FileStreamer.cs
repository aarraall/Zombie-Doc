using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
public static class FileStreamer
{
    private const string FORMAT = ".dat";
    public static void AddDataSet<TData>(TData data) where TData : Data
    {
        if (File.Exists(Application.persistentDataPath + data.PersistentPath + FORMAT))
        {
            LoadOperation(data);
        }
        else
        {
            data.SetDefault();
            SaveOperation(data);
        }
    }

    public static void Save<TData>(TData data) where TData : Data
    {
        SaveOperation(data);
    }

    public static void Load<TData>(TData data) where TData : Data
    {
        LoadOperation(data);
    }

    public static void Delete(Data data)
    {
        DeleteOperation(data.PersistentPath);
    }
    public static void Delete(string path)
    {
        DeleteOperation(path);
    }
    public static void Delete()
    {
        DeleteOperation("/");
    }
    private static void DeleteOperation(string path)
    {
        if (Directory.Exists(Application.persistentDataPath + path))
            Directory.Delete(Application.persistentDataPath + path);
    }

    private static void LoadOperation<TData>(TData data) where TData : Data
    {
        string fullPath = Application.persistentDataPath + data.PersistentPath + FORMAT;
        string json = File.ReadAllText(fullPath);
        TData newData = JsonConvert.DeserializeObject(json, typeof(TData), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }) as TData;
        data.SetData(newData);
    }

    private static void SaveOperation<TData>(TData data) where TData : Data
    {
        string path = Application.persistentDataPath + data.PersistentPath + FORMAT;
        string json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        File.WriteAllText(path, json);
    }
}
