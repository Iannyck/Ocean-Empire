using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Camera cam;
    public float YPos { get {  return tr.position.y; } }
    public float XPos { get { return tr.position.x; } }

    public float Top { get { return YPos + HalfHeight; } }
    public float Bottom { get { return YPos - HalfHeight; } }

    public float Left { get { return tr.position.x - HalfWidth; } }
    public float Right { get { return tr.position.x + HalfWidth; } }

    public float Height { get { return cam.orthographicSize * 2; } }
    public float Width { get { return Height * cam.aspect; } }

    public float HalfHeight { get { return cam.orthographicSize; } }
    public float HalfWidth { get { return HalfHeight * cam.aspect; } }


    public float Aspect { get { return cam.aspect; } }

    private Transform tr;

    private void Awake()
    {
        tr = transform;
        PersistentCamera.Enabled = false;
    }

    void OnDestroy()
    {
        PersistentCamera.Enabled = true;
    }
}
