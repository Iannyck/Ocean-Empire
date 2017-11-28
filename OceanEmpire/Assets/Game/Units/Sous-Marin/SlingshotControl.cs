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

    private int harpoonThrownAtOnce = 1;

    private void Start()
    {
        Game.OnGameReady += () => cam = Game.GameCamera.cam;
        toucheRadiusSQR = playerTouchRadius * playerTouchRadius;
        tr = transform;
    }

    public void StartDrag(Vector2 screenPosition)
    {
        if (harpoonPrefab == null)
        {
            HarpoonThrower ht = GetComponent<SubmarinParts>().GetHarpoonThrower();
            harpoonThrownAtOnce = ht.amountThrown;
            if (ht != null)
            {
                harpoonThrownAtOnce = ht.amountThrown;
                harpoonPrefab = ht.harpoonPrefab;
            }             
            else
                return;
        };

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
            ShootMultipleHarrpons((Vector2)tr.position - worldPosition);
        }
    }

    void ShootMultipleHarrpons(Vector2 direction)
    {
        const float angleoffset = 5;
        float middleAngle = direction.ToAngle();

        bool nombrePair = (harpoonThrownAtOnce % 2 == 0);

        float currentOffset = 0;
        int impair = 0;

        if (nombrePair)
        {
            currentOffset = angleoffset / 2;
            impair = 0;
        }
        else
        {
            currentOffset = 0;
            impair = 1;
        }

        for (int i = 0 + impair; i < harpoonThrownAtOnce + impair; ++i)
        {
            ShootHarpoon((middleAngle + currentOffset).ToVector());

            currentOffset = -currentOffset;
            if (i % 2 == 1) currentOffset += angleoffset;
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
