using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveable
{
    private Dictionary<string,bool>miniGameSateDict=new Dictionary<string,bool>();
    private GameController currentGame;
    private int gameWeek;
    private void OnEnable()
    {
        EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
        EventHandler.GamePassEvent += OnGamePassEvent;
        EventHandler.StarNewGameEvent += OnStarNewGameEvent;
    }
    private void OnDisable()
    {
        EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
        EventHandler.GamePassEvent -= OnGamePassEvent;
        EventHandler.StarNewGameEvent -= OnStarNewGameEvent;
    }

    private void OnStarNewGameEvent(int gameWeek)
    {
        this.gameWeek = gameWeek;
        miniGameSateDict.Clear();
    }

    private void OnGamePassEvent(string gameName)
    {
        miniGameSateDict[gameName] = true;
    }

    private void OnAfterSceneUnloadEvent()
    {
        foreach(var miniGame in FindObjectsOfType<MiniGame>())
        {
            if(miniGameSateDict.TryGetValue(miniGame.gameName,out bool isPass))
            {
                miniGame.isPass = isPass;
                miniGame.UpdateMiniGameState();
            }
        }
        currentGame=FindObjectOfType<GameController>();
        currentGame?.SetGameWeekData(gameWeek);
    }

    void Start()
    {
        EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }

    public GameSaveData GenerateSaveData()
    {
       GameSaveData saveData = new GameSaveData();  
        saveData.gameWeek=this.gameWeek;
        saveData.miniGameSateDict=this.miniGameSateDict;
        return saveData;
    }

    public void RestoreGameData(GameSaveData saveData)
    {
        this.gameWeek=saveData.gameWeek;
        this.miniGameSateDict = saveData.miniGameSateDict;
    }
}
