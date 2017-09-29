using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    private Transform tr;

    private void Awake()
    {
        tr = transform;
    }

    public T Spawn<T>(T obj, Vector2 position) where T : MonoBehaviour
    {
        return Instantiate(obj, position, Quaternion.identity, tr).GetComponent<T>();
    }
}
