using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Questing;
using System;
using DG.Tweening;

public class Shack_ChangeMap : MonoBehaviour
{
    public QuestPanel questPanel;

    public ScriptableActionQueue shackAnimQueue;
    public CanvasGroup hud;
    public CanvasGroup mainCanvas;

    [Header("Animation References")]
    public Transform shackStructure;
    public Shack_CameraController cameraController;

    [Header("Animation")]
    public float uiFadeInDuration = 1;
    public float uiFadeOutDuration = 1;
    public float heightIncrease = 5;
    public float goUpDuration = 6;
    public Ease goUpEase = Ease.InOutQuad;
    public float platformRotation;
    public float platformRotationDuration;
    public Ease platformRotationEase = Ease.InOutQuad;
    public float platformRotateBackDuration = 0.75f;
    public Ease platformRotateBackEase = Ease.OutBack; 
    public float upPause = 1;
    public float goDownDuration = 6;
    public Ease goDownEase = Ease.InOutQuad;

    public bool IsTransitioning { get; private set; }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            TransitionToNextMap();
    }

    public void TransitionToNextMap()
    {
        if (IsTransitioning)
            return;
        IsTransitioning = true;

        Action onCompletion = null;
        shackAnimQueue.ActionQueue.AddAction(() =>
        {
            // START
            hud.blocksRaycasts = false;
            mainCanvas.blocksRaycasts = false;

            Debug.Log("Start");

            hud.DOFade(0, uiFadeOutDuration);
            mainCanvas.DOFade(0, uiFadeOutDuration).onComplete = () =>
            {
                Debug.Log("Go up!");

                // Disable camera control
                cameraController.enabled = false;

                // Get container
                var tr = transform;

                // Attach structure and camera
                Transform shackOriginalParent = shackStructure.parent;
                shackStructure.SetParent(tr);
                var cam = PersistentCamera.GetCamera();
                cam.transform.SetParent(tr);

                // Change sky color
                //rien pour l'insant;

                Sequence sq = DOTween.Sequence();

                // Go up
                sq.Append(tr.DOMoveY(heightIncrease, goUpDuration).SetEase(goUpEase));
                sq.AppendCallback(() =>
                {
                    Debug.Log("We're up in the sky!");
                    questPanel.HideInstant();
                    QuestManager.Instance.RemoveAllQuests();
                    MapManager.Instance.SetMap_Next(true);
                });

                // Rotate platform
                sq.Append(shackStructure.DORotate(Vector3.forward * platformRotation, platformRotationDuration)
                    .SetEase(platformRotationEase));

                // Travel
                sq.AppendInterval(upPause);

                // Rotate platform back to normal
                sq.Append(shackStructure.DORotate(Vector3.zero, platformRotateBackDuration)
                    .SetEase(platformRotateBackEase));

                // Move back down
                sq.Append(tr.DOMoveY(0, goDownDuration).SetEase(goDownEase));

                // On complete
                sq.onComplete = () =>
                {
                    Debug.Log("Arrived");
                    hud.DOFade(1, uiFadeInDuration);

                    // Detach structure and camera
                    shackStructure.SetParent(shackOriginalParent);
                    cam.transform.SetParent(null);
                    DontDestroyOnLoad(cam.gameObject);

                    mainCanvas.DOFade(1, uiFadeInDuration).onComplete = () =>
                    {
                        Debug.Log("Completion");

                        // COMPLETION
                        hud.blocksRaycasts = true;
                        mainCanvas.blocksRaycasts = true;
                        cameraController.enabled = true;
                        questPanel.Show();
                        onCompletion();
                        IsTransitioning = false;
                    };
                };
            };
        }, 0, out onCompletion);
    }
}
