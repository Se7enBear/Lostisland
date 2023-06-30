using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SaveLoadManager : Single<SaveLoadManager>
{
    private string jasonFolder;
    private List<ISaveable> saveableList=new List<ISaveable>();
    private Dictionary<string,GameSaveData> saveDataDict=new Dictionary<string, GameSaveData>();
    protected override void Awake()
    {
        base.Awake();
        jasonFolder = Application.persistentDataPath + "/SAVE";
    }
    private void OnEnable()
    {
        EventHandler.StarNewGameEvent += OnStarNewGameEvent;

    }
    private void OnDisable()
    {
        EventHandler.StarNewGameEvent -= OnStarNewGameEvent;
    }

    private void OnStarNewGameEvent(int obj)
    {
        var resultPath = jasonFolder + "data.sav";
        if(File.Exists(resultPath))
        {
            File.Delete(resultPath);
        }
    }

    public void Register(ISaveable saveable)
    {
        saveableList.Add(saveable);
    }
    public void Save()
    {
        saveDataDict.Clear();
        foreach(var saveable in saveableList)
        {
            saveDataDict.Add(saveable.GetType().Name, saveable.GenerateSaveData());
        }
        var resultPath = jasonFolder + "data.sav";
        var jsonData=JsonConvert.SerializeObject(saveDataDict,Formatting.Indented);
        if(!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jasonFolder);
        }
        File.WriteAllText(resultPath, jsonData);
    }
    public void Load()
    {
        var resultPath = jasonFolder + "data.sav";
        if(!File.Exists(resultPath))
        {
            return;
        }
        var stringData=File.ReadAllText(resultPath);
        var jsonData=JsonConvert.DeserializeObject<Dictionary<string,GameSaveData>>(stringData);
        foreach(var saveable in saveableList)
        {
            saveable.RestoreGameData(jsonData[saveable.GetType().Name]);
        }
    }
}
