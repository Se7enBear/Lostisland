using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameH2A_SO", menuName = "Mini Game Data/GameH2A_SO")]
public class GameH2A_SO : ScriptableObject
{
    public string gameName;
    [Header("������ֺͶ�Ӧ��ͼƬ")]
    public List<BallDetails> ballDataList;
    [Header("��Ϸ�߼�����")]
    public List<Conections> lineConections;
    public List<BallName> startBallOrder;
    public BallDetails GetBallDetails(BallName ballName)
    {
        return ballDataList.Find(b=>b.ballName==ballName);
    }
}
[System.Serializable]

public class BallDetails
{
    public BallName ballName;
    public Sprite wrongSprite;
    public Sprite rightSprite;
}
[System.Serializable]
public class Conections
{
    public int from;
    public int to; 

}
