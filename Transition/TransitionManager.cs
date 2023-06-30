using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : Single<TransitionManager>,ISaveable
{
    public string startScene;
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration;
    private bool isFade;
    private bool canTransition;
    private void Start()
    {
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }
    private void OnEnable()
    {
        EventHandler.GameStateChangeEvent += OnGameStateChangeEvent;
        EventHandler.StarNewGameEvent += OnStarNewGameEvent;
    }
    private void OnDisable()
    {
        EventHandler.GameStateChangeEvent -= OnGameStateChangeEvent;
        EventHandler.StarNewGameEvent -= OnStarNewGameEvent;
    }

    private void OnStarNewGameEvent(int obj)
    {
        StartCoroutine(TpToScene("Menu", startScene));
    }

    private void OnGameStateChangeEvent(GameState gameState)
    {
        canTransition = gameState == GameState.GamePlay;
    }

    public void Transition(string from, string to)
    {
        if (!isFade&&canTransition) 
        StartCoroutine(TpToScene(from, to));
    }
   private IEnumerator TpToScene(string from, string to)
    {
        yield return Fade(1);
        if (from != string.Empty)
        {
            EventHandler.CallBeforeSceneUnloadEvent();
            yield return SceneManager.UnloadSceneAsync(from);
        }

        yield return SceneManager.LoadSceneAsync(to,LoadSceneMode.Additive);
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
        EventHandler.CallAfterSceneLoadEvent();
        yield return Fade(0);
    }
    private IEnumerator Fade(float targetAlpha)
    {
        isFade=true;
        fadeCanvasGroup.blocksRaycasts=true;
        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;
        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha,targetAlpha,speed*Time.deltaTime);
            yield return null;
        }
        fadeCanvasGroup.blocksRaycasts=false;
        isFade=false;
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();
        saveData.currentScene=SceneManager.GetActiveScene().name;
        return saveData;
    }

    public void RestoreGameData(GameSaveData saveData)
    {
       Transition("Menu",saveData.currentScene);
    }
}
 
