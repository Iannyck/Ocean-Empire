using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotControl : MonoBehaviour
{
    public float playerTouchRadius = 0.75f;
    public Slingshot slingshotInstance;
    public float maxDragLength = 2.5f;

    [Header("Visuals")]
    public Transform canonRotator;
    public SpriteRenderer stillHarpoon;
    [Header("Handle")]
    public SpriteRenderer handleRenderer;
    public Transform handleTransform;
    public Vector3 handleMinDraggingScale = Vector3.one;
    public Vector3 handleMaxDraggingScale = Vector3.one;

    [ReadOnly]
    public bool isDragging;
    [ReadOnly]
    public Vector2 worldPosition;

    [Header("Debug"), SerializeField] bool overrideItems = false;
    [SerializeField] Harpoon overridePrefab;
    [SerializeField] int overrideCount = 1;
    [SerializeField] float overrideCooldown = 2;


    private float cooldownTimer = 0;
    private float toucheRadiusSQR;
    private bool wasDragging = false;
    private Transform tr;
    private HarpoonThrower harpoonThrower;
    private Vector3 handleRestScale;
    private Vector2 handleRestSize;

    private void Start()
    {
        toucheRadiusSQR = playerTouchRadius * playerTouchRadius;
        tr = transform;
        if (handleTransform != null)
            handleRestScale = handleTransform.localScale;
        if (handleRenderer != null)
            handleRestSize = handleRenderer.size;
    }

    private void Update()
    {
        UpdateCooldown(Time.deltaTime);
        UpdateVisuals();
        wasDragging = isDragging;
    }

    private void UpdateVisuals()
    {
        if (isDragging)
        {
            Vector2 screenPosition;
            DragDetection.GetTouchPosition(out screenPosition);
            ConvertToWorldPos(screenPosition);

            var dir = (Vector2)tr.position - worldPosition;
            var maxLength = GetDragMaxLength();
            var length = Mathf.Min(dir.magnitude, maxLength);
            var normalizedLength = length / maxLength;
            var rotation = Quaternion.AngleAxis(dir.ToAngle(), Vector3.forward);

            if (canonRotator != null)
            {
                canonRotator.rotation = rotation;
            }
            if (handleRenderer != null && handleTransform != null)
            {
                handleTransform.localScale = Vector3.Lerp(handleMinDraggingScale, handleMaxDraggingScale, normalizedLength);
                float targetAdjustedLength = length / handleTransform.lossyScale.x;

                var xSize = Mathf.Max(handleRestSize.x, targetAdjustedLength);
                handleRenderer.size = new Vector2(xSize, handleRestSize.y);
            }

            slingshotInstance.UpdatePosition(worldPosition);
        }
    }

    #region Cooldown Handling
    private void UpdateCooldown(float deltaTime)
    {
        if (IsInCooldown())
        {
            cooldownTimer -= deltaTime;
            if (!IsInCooldown())
            {
                if (stillHarpoon != null)
                    stillHarpoon.enabled = true;
            }
        }
    }
    private void PutInCooldown()
    {
        cooldownTimer = GetShootCooldown();
        if (IsInCooldown())
        {
            if (stillHarpoon != null)
                stillHarpoon.enabled = false;
        }
    }
    private bool IsInCooldown()
    {
        return cooldownTimer > 0;
    }
    #endregion


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
    private float GetShootCooldown()
    {
        if (overrideItems)
        {
            return overrideCooldown;
        }
        else
        {
            if (!FetchHarpoonThrower())
                return -1;
            return -1;
            //return harpoonThrower.coo;
        }
    }
    private bool FetchHarpoonThrower()
    {
        if (harpoonThrower == null)
            harpoonThrower = GetComponent<SubmarinParts>().GetHarpoonThrower();
        return harpoonThrower != null;
    }
    private float GetDragMaxLength() { return maxDragLength; }
    #endregion

    #region Dragging
    public void StartDrag(Vector2 screenPosition)
    {
        if (GetHarpoonPrefab() == null || IsInCooldown())
            return;

        ConvertToWorldPos(screenPosition);

        if ((worldPosition - (Vector2)transform.position).sqrMagnitude <= toucheRadiusSQR)
        {
            isDragging = true;

            if (handleTransform != null)
            {
                handleTransform.localScale = handleMinDraggingScale;
            }

            slingshotInstance.Show();
            slingshotInstance.maxLength = maxDragLength;
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

            if (handleRenderer != null)
                handleRenderer.size = handleRestSize;
            if (handleTransform != null)
                handleTransform.localScale = handleRestScale;

            //Shoot !
            ShootMultipleHarpoons((Vector2)tr.position - worldPosition);
        }
    }

    void ConvertToWorldPos(Vector2 screenPos)
    {
        worldPosition = GetCamera().ScreenToWorldPoint(screenPos);
    }

    Camera GetCamera()
    {
        if (Game.Instance != null)
        {
            return Game.GameCamera.cam;
        }
        else
        {
            var camGameObject = GameObject.FindGameObjectWithTag("MainCamera");
            if (camGameObject == null)
                Debug.LogError("No camera found");
            return camGameObject.GetComponent<Camera>();
        }
    }
    #endregion

    #region Shooting
    void ShootMultipleHarpoons(Vector2 direction)
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
            ShootHarpoon(Quaternion.AngleAxis(middleAngle + currentOffset, Vector3.forward));

            currentOffset = -currentOffset;
            if (i % 2 == 1) currentOffset += angleoffset;
        }
    }
    void ShootHarpoon(Quaternion direction)
    {
        Harpoon harpoon = GetHarpoonPrefab().DuplicateGO(tr.position, direction);
        harpoon.shootOnStart = true;

        PutInCooldown();
    }
    #endregion
}
