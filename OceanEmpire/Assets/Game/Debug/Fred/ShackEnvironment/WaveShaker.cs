using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveShaker : MonoBehaviour
{
    [System.Serializable]
    public struct MovementData
    {
        public float Size;
        public float Speed;
        public float CycleOffset;
    }
    [SerializeField] bool _horizontalMovement;

    //[Forward]
    [ShowIf("_horizontalMovement", HideShowBaseAttribute.Type.Field)]
    [SerializeField] MovementData _horizontalData;

    Vector3 anchorPos;
    Transform tr;
    float timer;

    void Awake()
    {
        tr = transform;
        anchorPos = tr.localPosition;
    }

    void Update()
    {
        Vector3 pos = anchorPos;
        if (_horizontalMovement)
        {
            pos.x += GetDelta(ref _horizontalData);
        }
        tr.position = pos;

        UpdateTimer();
    }

    void UpdateTimer()
    {
        timer += Time.deltaTime;
    }
    float GetDelta(ref MovementData movementData)
    {
        var radOffset = movementData.CycleOffset * (Mathf.PI * 2);
        var time = timer * movementData.Speed;
        return movementData.Size * Mathf.Sin(time + radOffset);
    }
}
