using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotControl : MonoBehaviour
{
    public float playerTouchRadius = 0.75f;
    public Slingshot slingshotInstance;

    [ReadOnly]
    public bool isDragging;
    [ReadOnly]
    public Vector2 worldPosition;

    [Header("Debug"), SerializeField] bool overrideItems = false;
    [SerializeField] Harpoon overridePrefab;
    [SerializeField] int overrideCount = 1;


    private Camera cam;
    private float toucheRadiusSQR;
    private Transform tr;
    private HarpoonThrower harpoonThrower;

    private void Start()
    {
        Game.OnGameReady += () => cam = Game.GameCamera.cam;
        toucheRadiusSQR = playerTouchRadius * playerTouchRadius;
        tr = transform;
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

    #region Harpoon Resources
    private Harpoon GetHarpoonPrefab()
    {
        if (overrideItems)
        {
            return overridePrefab;
        }
        else
        {
            if (!FetchHarpoonThrower())
                return null;
            return harpoonThrower.harpoonPrefab;
        }
    }

    private int GetHarpoonCount()
    {
        if (overrideItems)
        {
            return overrideCount;
        }
        else
        {
            if (!FetchHarpoonThrower())
                return -1;

            return harpoonThrower.amountThrown;
        }
    }

    private bool FetchHarpoonThrower()
    {
        if (harpoonThrower == null)
            harpoonThrower = GetComponent<SubmarinParts>().GetHarpoonThrower();
        return harpoonThrower != null;
    }
    #endregion

    #region Dragging
    public void StartDrag(Vector2 screenPosition)
    {
        if (GetHarpoonPrefab() == null)
            return;

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

    void ConvertToWorldPos(Vector2 screenPos)
    {
        if (cam != null)
        {
            worldPosition = cam.ScreenToWorldPoint(screenPos);
        }
    }
    #endregion

    #region Shooting
    void ShootMultipleHarrpons(Vector2 direction)
    {
        const float angleoffset = 5;
        var harpoonCount = GetHarpoonCount();
        float middleAngle = direction.ToAngle();

        bool nombrePair = (harpoonCount % 2 == 0);

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

        for (int i = 0 + impair; i < harpoonCount + impair; ++i)
        {
            ShootHarpoon((middleAngle + currentOffset).ToVector());

            currentOffset = -currentOffset;
            if (i % 2 == 1) currentOffset += angleoffset;
        }
    }

    void ShootHarpoon(Vector2 direction)
    {
        Harpoon harpoon = Game.Spawner.Spawn(GetHarpoonPrefab(), tr.position);
        harpoon.Shoot_Direction(direction);
    }
    #endregion
}
