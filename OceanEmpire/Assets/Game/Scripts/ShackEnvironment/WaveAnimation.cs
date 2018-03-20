using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAnimation : MonoBehaviour
{
    [System.Serializable]
    public struct MovementData
    {
        public float Size;
        public float Speed;
        public float CycleOffset;
    }
    [SerializeField] bool _horizontalMovement;
    [ShowIf("_horizontalMovement", HideShowBaseAttribute.Type.Field)]
    [SerializeField] MovementData _horizontalData;

    [SerializeField] bool _verticalMovement;
    [ShowIf("_verticalMovement", HideShowBaseAttribute.Type.Field)]
    [SerializeField] MovementData _verticalData;

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
        if (_verticalMovement)
        {
            pos.y += GetDelta(ref _verticalData);
        }
        tr.localPosition = pos;

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
