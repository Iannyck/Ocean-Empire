using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_CanvasSection : MonoBehaviour
{
    private const float ALPHA_PLATEAU = 0.1f;
    private const float ALPHA_RANGE = 0.45f;

    [SerializeField] Shack_CameraController cameraController;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Shack_CameraController.Section section;

    private RectTransform rectTr;

    void Awake()
    {
        rectTr = GetComponent<RectTransform>();

        // On se disable avant en attendant que PersistentLoader soit loadé
        enabled = false;
        PersistentLoader.LoadIfNotLoaded(() => enabled = true);
    }

    void Update()
    {
        var sectionPosition = cameraController.CameraSectionPosition;
        UpdateAlpha(sectionPosition);
        UpdatePosition(sectionPosition);
    }

    void UpdatePosition(float sectionPosition)
    {
        var invDelta = (float)section - sectionPosition;
        rectTr.anchorMin = new Vector2(invDelta, rectTr.anchorMin.y);
        rectTr.anchorMax = new Vector2(invDelta + 1, rectTr.anchorMax.y);
    }

    void UpdateAlpha(float sectionPosition)
    {
        float center = (float)section;
        float delta = (sectionPosition - center).Abs();

        canvasGroup.alpha = 1 - Mathf.Clamp01((delta - ALPHA_PLATEAU) / (ALPHA_RANGE - ALPHA_PLATEAU));
    }
}
