using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Persistence;
using System;

public class PersistentCamera : MonoPersistent
{
    [SerializeField] private Camera Camera;

    private static PersistentCamera instance;
    private static bool _Enabled = true;

    private void Awake()
    {
        instance = this;
        ApplyEnable();
    }

    public override void Init(Action onComplete)
    {
        onComplete();
    }

    private void ApplyEnable()
    {
        gameObject.SetActive(_Enabled);
    }

    public static Camera GetCamera()
    {
        if (instance == null)
            Debug.LogError("No PersistentCamera instance.");
        return instance.Camera;
    }

    public static bool Enabled
    {
        get
        {
            return _Enabled;
        }
        set
        {
            _Enabled = value;
            if (instance != null)
                instance.gameObject.SetActive(value);
        }
    }
}
