using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] Camera _cam;
    [SerializeField] CameraMovement _movement;

    public CameraMovement CameraMovement { get { return _movement; } }
    public Camera CameraComponent { get { return _cam; } }

    public static float CameraHeight { get { return 10; } }
    public static float CameraWidth { get { return CameraHeight * Screen.width / Screen.height; } }
    public float YPos { get { return Tr.position.y; } }
    public float XPos { get { return Tr.position.x; } }

    public float Top { get { return YPos + HalfHeight; } }
    public float Bottom { get { return YPos - HalfHeight; } }

    public float Left { get { return Tr.position.x - HalfWidth; } }
    public float Right { get { return Tr.position.x + HalfWidth; } }

    public float Height { get { return CameraComponent.orthographicSize * 2; } }
    public float Width { get { return Height * CameraComponent.aspect; } }

    public float HalfHeight { get { return CameraComponent.orthographicSize; } }
    public float HalfWidth { get { return HalfHeight * CameraComponent.aspect; } }


    public float Aspect { get { return CameraComponent.aspect; } }

    public Transform Tr { get; private set; }

    private void Awake()
    {
        Tr = transform;
        PersistentCamera.Enabled = false;
    }

    void OnDestroy()
    {
        PersistentCamera.Enabled = true;
    }
}
