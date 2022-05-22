using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager
{
    public static PlayerData Player;

    public static void Initialize()
    {
        Player = new PlayerData();
        FileStreamer.AddDataSet(Player);
    }

    public static void Save()
    {
        FileStreamer.Save(Player);
    }

    public static void Delete()
    {
        FileStreamer.Delete();
    }
}

public class PlayerData : Data
{
    public int level = 1;
    public int coin = 0;

    public PlayerData()
    {
        _persistentPath = "/player";
    }
    public override void SetData<TData>(TData data)
    {
        PlayerData convertedData = data as PlayerData;
        level = convertedData.level;
        coin = convertedData.coin;
    }

    public override void SetDefault()
    {
        level = 1;
        coin = 0;
    }
}