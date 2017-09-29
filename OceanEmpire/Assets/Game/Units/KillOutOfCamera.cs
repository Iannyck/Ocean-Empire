using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOutOfCamera : MonoBehaviour
{
    public bool checkY = true;
    public bool checkX = false;

    public bool overrideDefaultMargin = false;
    public float margin = 3.33f;
    private const float DEFAULT_MARGIN = 3.33f;

    private IKillable killable;
    private GameCamera cam;
    private Transform tr;

    private void Awake()
    {
        tr = transform;
        enabled = false;
        killable = GetComponent<IKillable>();

        if (Game.instance != null || Game.instance.gameStarted)
            GetReference();
        else
            Game.OnGameStart += GetReference;
    }

    private void GetReference()
    {
        cam = Game.GameCamera;
        enabled = true;
    }

    private void Update()
    {
        if (killable.IsDead())
            return;

        bool shallWeKill = false;

        if (checkY)
        {
            if (IsOut(transform.position.y, cam.YPos, cam.HalfHeight))
                shallWeKill = true;
        }

        if(!shallWeKill && checkX)
        {
            if (IsOut(transform.position.x, cam.XPos, cam.HalfWidth))
                shallWeKill = true;
        }

        if (shallWeKill)
        {
            killable.Kill();
        }
    }

    bool IsOut(float a, float b, float dist)
    {
        float delta = (b - a).Abs();
        return delta > dist + Margin;
    }

    private float Margin { get { return overrideDefaultMargin ? margin : DEFAULT_MARGIN; } }
}
