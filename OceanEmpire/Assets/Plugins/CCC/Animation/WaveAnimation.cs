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
    [Header("Translation")]
    [SerializeField] bool _horizontalMovement;
    [ShowIf("_horizontalMovement", HideShowBaseAttribute.Type.Field)]
    [SerializeField] MovementData _horizontalData;

    [SerializeField] bool _verticalMovement;
    [ShowIf("_verticalMovement", HideShowBaseAttribute.Type.Field)]
    [SerializeField] MovementData _verticalData;


    [Header("Rotation")]
    [SerializeField] bool _xRotation;
    [ShowIf("_xRotation", HideShowBaseAttribute.Type.Field)]
    [SerializeField] MovementData _xRotData;

    [SerializeField] bool _yRotation;
    [ShowIf("_yRotation", HideShowBaseAttribute.Type.Field)]
    [SerializeField] MovementData _yRotData;

    [SerializeField] bool _zRotation;
    [ShowIf("_zRotation", HideShowBaseAttribute.Type.Field)]
    [SerializeField] MovementData _zRotData;

    Vector3 anchorPos;
    Vector3 anchorRot;
    Transform tr;

    void Awake()
    {
        tr = transform;
        UpdateAnchorPositions();
    }

    /// <summary>
    /// The current animation time, should 
    /// </summary>
    public float CurrentTime { get; set; }

    public void UpdateAnchorPositions()
    {
        anchorPos = tr.localPosition;
        anchorRot = tr.localRotation.eulerAngles;
    }

    void Update()
    {
        if (_horizontalMovement || _verticalMovement)
        {
            var pos = anchorPos;
            pos.x += _horizontalMovement ? GetDelta(ref _horizontalData) : 0;
            pos.y += _verticalMovement ? GetDelta(ref _verticalData) : 0;
            tr.localPosition = pos;
        }

        if (_xRotation || _yRotation || _zRotation)
        {
            var rot = anchorRot;
            rot.x += _xRotation ? GetDelta(ref _xRotData) : 0;
            rot.y += _yRotation ? GetDelta(ref _yRotData) : 0;
            rot.z += _zRotation ? GetDelta(ref _zRotData) : 0;

            tr.localRotation = Quaternion.Euler(rot);
        }

        UpdateTimer();
    }

    void UpdateTimer()
    {
        CurrentTime += Time.deltaTime;
    }

    float GetDelta(ref MovementData movementData)
    {
        var radOffset = movementData.CycleOffset * (Mathf.PI * 2);
        var time = CurrentTime * movementData.Speed;
        return movementData.Size * Mathf.Sin(time + radOffset);
    }
}
