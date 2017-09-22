using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript_Fred : MonoBehaviour
{
    public FishDescription fd;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (fd.LoadSprite() != null)
                print(fd.LoadSprite());
        }
    }
}
