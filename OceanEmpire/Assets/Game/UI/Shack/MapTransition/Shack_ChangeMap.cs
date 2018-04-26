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
    public Shack_Environment shack_Environment;
    public Transform clouds;
    public Transform[] thrusters;

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
    public float upPause = 1;
    public float cloudOutTimingOffset = -0.5f;
    public Ease cloudSpeedEaseOut;
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

            hud.DOFade(0, uiFadeOutDuration);
            mainCanvas.DOFade(0, uiFadeOutDuration).onComplete = () =>
            {

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

                // Thrusters
                for (int i = 0; i < thrusters.Length; i++)
                {
                    thrusters[i].gameObject.SetActive(true);
                    thrusters[i].DOScale(1, 0.5f).SetEase(Ease.OutBack);
                }

                // Go up
                sq.Join(tr.DOMoveY(heightIncrease, goUpDuration).SetRelative().SetEase(goUpEase));
                sq.Join(clouds.DOMoveY(cloudsHeightIncrease, goUpDuration).SetRelative().SetEase(goUpEase));
                sq.AppendCallback(() =>
                {
                    shack_Environment.CopyManualModeFromMapData(MapManager.Instance.MapData);
                    shack_Environment.manualMode = true;

                    questPanel.HideInstant();
                    QuestManager.Instance.RemoveAllQuests();
                    MapManager.Instance.SetMap_Next(true);
                });

                // Rotate platform
                sq.Append(shackStructure.DORotate(Vector3.forward * platformRotation, platformRotationDuration)
                    .SetEase(platformRotationEase));

                // Travel ! (Move clouds)
                sq.Join(DOTween.To(() => cloudMover.horizontalSpeed, (x) => cloudMover.horizontalSpeed = x, cloudSpeed, platformRotationDuration)
                    .SetEase(cloudSpeedEaseIn));

                // TRANSITION MAP COLOR
                sq.AppendCallback(() =>
                {
                    //NB, on le fait dans un callback pour aléger la frame
                    var transition = 0f;
                    var waterStart = shack_Environment.manualWaterColor;
                    var skyBottom = shack_Environment.manualSkyColorBottom;
                    var skyCenter = shack_Environment.manualSkyColorCenter;
                    var skyTop = shack_Environment.manualSkyColorTop;
                    MapData mapData = MapManager.Instance.MapData;

                    DOTween.To(() => transition, (x) =>
                    {
                        transition = x;
                        shack_Environment.manualWaterColor = Color.Lerp(waterStart, mapData.ShallowColor, x);
                        shack_Environment.manualSkyColorTop = Color.Lerp(skyTop, mapData.SkyColorTop, x);
                        shack_Environment.manualSkyColorCenter = Color.Lerp(skyCenter, mapData.SkyColorCenter, x);
                        shack_Environment.manualSkyColorBottom = Color.Lerp(skyBottom, mapData.SkyColorBottom, x);
                    }, 1, upPause);
                });
                sq.AppendInterval(upPause);
                sq.Append(DOTween.To(() => cloudMover.horizontalSpeed, (x) => cloudMover.horizontalSpeed = x, 0, platformRotateBackDuration)
                    .SetEase(cloudSpeedEaseOut));

                // Rotate platform back to normal
                sq.Insert(sq.Duration() + cloudOutTimingOffset, shackStructure.DORotate(Vector3.zero, platformRotateBackDuration)
                    .SetEase(platformRotateBackEase));

                // Move back down
                sq.Append(tr.DOMoveY(-heightIncrease, goDownDuration).SetRelative().SetEase(goDownEase));
                sq.Join(clouds.DOLocalMoveY(-cloudsHeightIncrease, goDownDuration).SetRelative().SetEase(goDownEase));

                sq.InsertCallback(sq.Duration() - (goDownDuration / 2), () =>
                {
                    // Thrusters close
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        thrusters[i].DOScale(0, 0.5f).SetEase(Ease.InQuint);
                    }
                });

                // On complete
                sq.onComplete = () =>
                {
                    shack_Environment.manualMode = false;
                    shack_Environment.ApplyMapData(MapManager.Instance.MapData);

                    // Thrusters deactivate
                    for (int i = 0; i < thrusters.Length; i++)
                    {
                        thrusters[i].gameObject.SetActive(false);
                    }

                    hud.DOFade(1, uiFadeInDuration);

                    // Detach structure and camera
                    shackStructure.SetParent(shackOriginalParent);
                    cam.transform.SetParent(null);
                    DontDestroyOnLoad(cam.gameObject);

                    mainCanvas.DOFade(1, uiFadeInDuration).onComplete = () =>
                    {
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
