using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript_Fred : MonoBehaviour
{
    public FishingFrenzyWidget widget;
    public int bigAssNumber = int.MaxValue;

    void Start()
    {
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            bigAssNumber--;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            FishingFrenzy.Instance.Activate();
        }
        if (Input.GetKey(KeyCode.U))
        {
            widget.Set_Active(10, bigAssNumber % 60);
        }
        if (Input.GetKey(KeyCode.I))
        {
            widget.Set_Available();
        }
        if (Input.GetKey(KeyCode.O))
        {
            widget.Set_Error();
        }
        if (Input.GetKey(KeyCode.P))
        {
            widget.Set_InCooldown(3, 12, bigAssNumber % 60);
        }

    }
}