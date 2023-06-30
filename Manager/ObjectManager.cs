using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour,ISaveable
{
    private Dictionary<ItemName, bool> itemAvailableDict = new Dictionary<ItemName, bool>();
    private Dictionary<string,bool>interactiveStateDict=new Dictionary<string,bool>();
    private void Start()
    {
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }
    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
        EventHandler.UpdateUIEvent += OnUpdateUIEvent;
        EventHandler.StarNewGameEvent += OnStarNewGameEvent;

    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
        EventHandler.UpdateUIEvent -= OnUpdateUIEvent;
        EventHandler.StarNewGameEvent -= OnStarNewGameEvent;
    }

    private void OnStarNewGameEvent(int obj)
    {
        itemAvailableDict.Clear();
        interactiveStateDict.Clear();
    }

    private void OnUpdateUIEvent(ItemDetails itemDetails, int index)
    {
        if(itemDetails != null)
        {
            itemAvailableDict[itemDetails.itemName] = false;
        }
    }

    private void OnBeforeSceneUnloadEvent()
    {
        foreach (var item in FindObjectsOfType<Item>())
        {
            if (!itemAvailableDict.ContainsKey(item.itemName))
            {
                itemAvailableDict.Add(item.itemName, true);
            }
        }
        foreach (var item in FindObjectsOfType<Interactive>())
        {
            if(interactiveStateDict.ContainsKey(item.name))
            {
                interactiveStateDict[item.name] = item.isDone;
            }
            else
                interactiveStateDict.Add(item.name, item.isDone);
        }
    }
    private void OnAfterSceneUnloadEvent()
    {
        foreach (var item in FindObjectsOfType<Item>())
        {
            if (!itemAvailableDict.ContainsKey(item.itemName))
            {
                itemAvailableDict.Add(item.itemName, true);
            }
            else
            {
                item.gameObject.SetActive(itemAvailableDict[item.itemName]);

            }

        }
        foreach (var item in FindObjectsOfType<Interactive>())
        {
            if (interactiveStateDict.ContainsKey(item.name))
            {
                item.isDone= interactiveStateDict[item.name];
            }
            else
                interactiveStateDict.Add(item.name, item.isDone);
        }
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();
        saveData.interactiveStateDict = this.interactiveStateDict;
        saveData.itemAvailableDict = this.itemAvailableDict;
        return saveData;
    }

    public void RestoreGameData(GameSaveData saveData)
    {
        this.interactiveStateDict = saveData.interactiveStateDict;
        this.itemAvailableDict = saveData.itemAvailableDict;
    }
}
