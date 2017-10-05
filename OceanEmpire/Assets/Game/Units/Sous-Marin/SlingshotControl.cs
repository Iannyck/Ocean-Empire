using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotControl : MonoBehaviour
{
    public float playerTouchRadius = 0.75f;
    public Slingshot slingshotInstance;

    public Harpoon harpoonPrefab;   // On va changer ça lors qu'on aura plusieurs weapons

    [ReadOnly]
    public bool isDragging;
    [ReadOnly]
    public Vector2 worldPosition;

    private Camera cam;
    private float toucheRadiusSQR;
    private Transform tr;

    private void Start()
    {
        Game.OnGameReady += () => cam = Game.GameCamera.cam;
        toucheRadiusSQR = playerTouchRadius * playerTouchRadius;
        tr = transform;
    }

    public void StartDrag(Vector2 screenPosition)
    {
        ConvertToWorldPos(screenPosition);
        if ((worldPosition - (Vector2)transform.position).sqrMagnitude <= toucheRadiusSQR)
        {
            isDragging = true;
            slingshotInstance.Show();
            slingshotInstance.followAnchor = tr;
            slingshotInstance.UpdatePosition(worldPosition);
        }
    }

    public void ReleaseDrag(Vector2 screenPosition)
    {
        if (isDragging)
        {
            ConvertToWorldPos(screenPosition);
            isDragging = false;
            slingshotInstance.Hide();

            //Shoot !
            ShootHarpoon((Vector2)tr.position - worldPosition);
        }
    }

    void ShootHarpoon(Vector2 direction)
    {
        Harpoon harpoon = Game.Spawner.Spawn(harpoonPrefab, tr.position);
        harpoon.Shoot_Direction(direction);
    }

    private void Update()
    {
        if (isDragging && cam != null)
        {
            Vector2 screenPosition;
            DragDetection.GetTouchPosition(out screenPosition);
            ConvertToWorldPos(screenPosition);
            slingshotInstance.UpdatePosition(worldPosition);
        }
    }

    void ConvertToWorldPos(Vector2 screenPos)
    {
        if (cam != null)
        {
            worldPosition = cam.ScreenToWorldPoint(screenPos);
        }
    }
}
