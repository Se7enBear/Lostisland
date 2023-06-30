using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tp : MonoBehaviour
{
    public string scenceFrom;
    public string scenceTo;
    public void TpToScene()
    {
        TransitionManager.Instance.Transition(scenceFrom, scenceTo);
    }
}
