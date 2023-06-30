using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : Single<GameController>
{
    public UnityEvent OnFinsh;
    [Header("��Ϸ����")]
    public GameH2A_SO gameData;
    public GameH2A_SO[] gameDataArray;
    public GameObject lineParent;
    public LineRenderer linePrefab;
    public Ball ballPrefab;

    public Transform[] holderTransforms;
    //private void Start()
    //{
    //    DrawLine();
    //    CreateBall();
    //}
    private void OnEnable()
    {
        EventHandler.CheckGameStateEvent += OnCheckGameStateEvent;
    }
    private void OnDisable()
    {
        EventHandler.CheckGameStateEvent -= OnCheckGameStateEvent;
    }

    private void OnCheckGameStateEvent()
    {
        foreach(var ball in FindObjectsOfType<Ball>())
        {
            if (!ball.isMatch)
                return;
        }
        foreach(var holder in holderTransforms)
        {
            holder.GetComponent<Collider2D>().enabled = false;
        }
        EventHandler.CallGamePassEvent(gameData.gameName);
        OnFinsh?.Invoke();
    }
    public void Reset()
    {
        foreach(var holder in holderTransforms)
        {
            if(holder.childCount>0)
            {
                Destroy(holder.GetChild(0).gameObject);
            }
        }
        CreateBall();
    }
    public void DrawLine()
    {
        foreach(var conections in gameData.lineConections)
        {
            var line = Instantiate(linePrefab, lineParent.transform);
            line.SetPosition(0, holderTransforms[conections.from].position);
            line.SetPosition(1, holderTransforms[conections.to].position);
            holderTransforms[conections.from].GetComponent<Holder>().linkHolders.Add(holderTransforms[conections.to].GetComponent<Holder>());
            holderTransforms[conections.to].GetComponent<Holder>().linkHolders.Add(holderTransforms[conections.from].GetComponent<Holder>());
        }
    }
    public void CreateBall()
    {
        for (int i = 0;i<gameData.startBallOrder.Count;i++)
        {
            if (gameData.startBallOrder[i]==BallName.None)
            {
                holderTransforms[i].GetComponent<Holder>().isEmpty = true;
                continue;
            }
            Ball ball = Instantiate(ballPrefab, holderTransforms[i]);

            holderTransforms[i].GetComponent<Holder>().CheckBall(ball);
            holderTransforms[i].GetComponent<Holder>().isEmpty = false;
            ball.SetupBall(gameData.GetBallDetails(gameData.startBallOrder[i]));
        }
    }
    public void SetGameWeekData(int week)
    {
        gameData = gameDataArray[week];
        DrawLine();
        CreateBall();
    }
}
