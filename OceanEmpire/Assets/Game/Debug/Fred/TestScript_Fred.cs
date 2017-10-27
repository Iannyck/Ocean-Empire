using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript_Fred : MonoBehaviour
{
    public FishDescription fd;
    public float value;
    public List<int> list = new List<int>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            list.SortedAdd(Random.Range(0,20), (a, b)=> -a.CompareTo(b));
        }
    }
}
