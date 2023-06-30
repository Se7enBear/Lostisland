using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ContineGame()
    {
        SaveLoadManager.Instance.Load();
    }
    public void GOBackToMenu()
    {
        var currentScene=SceneManager.GetActiveScene().name;
        EventHandler.CallBeforeSceneUnloadEvent();
        TransitionManager.Instance.Transition(currentScene, "Menu");

        SaveLoadManager.Instance.Save();
    }
    public void StartGameWeek(int gameWeek)
    {
        EventHandler.CallStartNewGameEvent(gameWeek);
    }
}
