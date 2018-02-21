using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript_Fred : MonoBehaviour
{

    //public string manualGenCode;
    //public GeneratedSpriteKit readKit;
    //public GeneratedSpriteKit kit;
    //public SpriteKitGenerator boboKitGenerator;

    public Locker gameRunning = new Locker();

    void Start()
    {
        gameRunning.onLockStateChange += GameRunning_onLockStateChange;

        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");

        print(CoroutineLauncher.Instance);
    }

    private void GameRunning_onLockStateChange(bool state)
    {
        if (state)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    kit = boboKitGenerator.GenerateNewSpriteKit();
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    readKit = boboKitGenerator.GenerateSpriteKit(manualGenCode);
        //}
    }
}