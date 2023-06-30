using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable 
{
   void SaveableRegister()
    {
        SaveLoadManager.Instance.Register(this);

    }
    GameSaveData GenerateSaveData();
    void RestoreGameData(GameSaveData saveData);

}
