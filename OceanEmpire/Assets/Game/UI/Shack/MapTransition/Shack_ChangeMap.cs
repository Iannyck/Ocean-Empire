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
    public Shack_CloudMover cloudMover;
    public Transform clouds;

    [Header("Animation")]
    public float uiFadeInDuration = 1;
    public float uiFadeOutDuration = 1;
    public float heightIncrease = 5;
    public float cloudsHeightIncrease;
    public float goUpDuration = 6;
    public Ease goUpEase = Ease.InOutQuad;
    public float platformRotation;
    public float platformRotationDuration;
    public Ease platformRotationEase = Ease.InOutQuad;
    public float cloudInTimingOffset = -0.5f;
    public float cloudSpeed;
    public Ease cloudSpeedEaseIn;
    public float cloudOutTimingOffset = -0.5f;
    public Ease cloudSpeedEaseOut;
    public float upPause = 1;
    public float platformRotateBackDuration = 0.75f;
    public Ease platformRotateBackEase = Ease.OutBack;
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
                sq.Append(tr.DOMoveY(heightIncrease, goUpDuration).SetRelative().SetEase(goUpEase));
                sq.Join(clouds.DOMoveY(cloudsHeightIncrease, goUpDuration).SetRelative().SetEase(goUpEase));
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

                // Travel ! (Move clouds)
                if (cloudMover != null)
                {
                    sq.Join(/*sq.Duration() + cloudInTimingOffset,  */DOTween.To(() => cloudMover.horizontalSpeed, (x) => cloudMover.horizontalSpeed = x, cloudSpeed, upPause / 2)
                        .SetEase(cloudSpeedEaseIn));
                    sq.Append(DOTween.To(() => cloudMover.horizontalSpeed, (x) => cloudMover.horizontalSpeed = x, 0, upPause / 2)
                        .SetEase(cloudSpeedEaseOut));
                }
                else
                {
                    sq.AppendInterval(upPause);
                }

                // Rotate platform back to normal
                sq.Insert(sq.Duration() + cloudOutTimingOffset, shackStructure.DORotate(Vector3.zero, platformRotateBackDuration)
                    .SetEase(platformRotateBackEase));

                // Move back down
                sq.Append(tr.DOMoveY(-heightIncrease, goDownDuration).SetRelative().SetEase(goDownEase));
                sq.Join(clouds.DOLocalMoveY(-cloudsHeightIncrease, goDownDuration).SetRelative().SetEase(goDownEase));

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
