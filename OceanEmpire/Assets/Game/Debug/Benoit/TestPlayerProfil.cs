using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerProfil : MonoBehaviour {

	
    public void AddLevel()
    {
        PlayerProfile.IncrementLevel(1);
    }

    public void PrintLevel()
    {
        print(PlayerProfile.Level);
    }

    public void ResetPlayerLevel()
    {
        DataSaverBank.Instance.GetDataSaver(DataSaverBank.Type.PlayerProfile).ClearSave();
    }
}