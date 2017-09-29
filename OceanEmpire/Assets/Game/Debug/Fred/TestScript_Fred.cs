using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript_Fred : MonoBehaviour
{
    public FishDescription fd;
    public float value;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            print(fd.icon.GetSprite());
        }
    }
}
