using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript_Fred : MonoBehaviour
{
    public NonRevivableUnit simpleKillableUnit;
    public RevivableUnit restartableUnit;

    //public string manualGenCode;
    //public GeneratedSpriteKit readKit;
    //public GeneratedSpriteKit kit;
    //public SpriteKitGenerator boboKitGenerator;

    void Start()
    {
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");
        simpleKillableUnit.OnAllDeaths += (x) => Debug.Log("simple All death");
        simpleKillableUnit.OnNextDeath += (x) => Debug.Log("simple Next death");
        restartableUnit.OnAllDeaths += (x) => Debug.Log("restartable All death");
        restartableUnit.OnNextDeath += (x) => Debug.Log("restartable Next death");
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            simpleKillableUnit.Kill();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            restartableUnit.Kill();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            restartableUnit.gameObject.SetActive(true);
        }
    }
}