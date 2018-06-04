﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerSpawn : MonoBehaviour
{
    public SubmarineMovement submarinePrefab;

    [Header("Intro"), SerializeField] float topHeightIncrease = 2;
    [SerializeField] float introToTopDuration = 0.5f;
    [SerializeField] float introToWaterDuration = 1.0f;

    [Header("Outro"), SerializeField] float exitDuration = 1.5f;


    private Vector2 BoatLocation;
    private Vector2 TopLocation;
    private Vector2 WaterLocation;

    private Vector3 boatScale = new Vector3(0.2f, 0.2f, 0.2f);
    private Vector3 TopScale = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 waterScale = new Vector3(1f, 1f, 1f);


    public void Init()
    {
        BoatLocation = Game.Instance.MapLayout.PlayerSpawn.position;
        WaterLocation = Game.Instance.MapLayout.PlayerStart_.position;
        TopLocation = BoatLocation + Vector2.up * topHeightIncrease;
    }

    public SubmarineMovement Spawn(Vector2 position)
    {
        if (submarinePrefab == null)
            return null;

        return Game.Instance.UnitInstantiator.Spawn(submarinePrefab, position);
    }

    public SubmarineMovement SpawnPlayer()
    {
        Vector3 defaultPosition = Game.Instance.GameCamera.Tr.position;
        defaultPosition.z = 0;

        SubmarineMovement newPlayer = Spawn(defaultPosition);
        if (newPlayer == null)
            Debug.Log("Erreur Spawn Player");
        return newPlayer;
    }

    public SubmarineMovement AnimatePlayerIntro(SubmarineMovement player, Action onIntroAnimComplete)
    {
        Init();

        player.transform.localScale = boatScale;
        player.transform.position = BoatLocation;
        // Physics
        player.enabled = false;

        // Anim
        Tweener intro1 = player.transform.DOMove(TopLocation, introToTopDuration);
        intro1.SetEase(Ease.InSine);
        intro1.SetEase(Ease.OutSine);

        TopScale = boatScale + (waterScale - boatScale) * (introToTopDuration / (introToTopDuration + introToWaterDuration / 2));
        Tweener intro2 = player.transform.DOScale(TopScale, introToTopDuration);
        intro2.SetEase(Ease.InOutSine);




        intro1.OnComplete(delegate () { Tweene2(player, onIntroAnimComplete); });

        return player;
    }


    private SubmarineMovement Tweene2(SubmarineMovement player, Action onIntroAnimComplete)
    {
        // Anim
        Tweener intro2 = player.transform.DOScale((TopScale + waterScale) / 2, introToWaterDuration / 2);
        intro2.SetEase(Ease.InSine);



        Tweener intro1 = player.transform.DOMove((TopLocation + WaterLocation) / 2, introToWaterDuration / 2);
        intro1.SetEase(Ease.InSine);

        intro1.OnComplete(delegate () { Tween3(player, onIntroAnimComplete); });

        return player;
    }

    private SubmarineMovement Tween3(SubmarineMovement player, Action onIntroAnimComplete)
    {
        // Anim
        Tweener intro2 = player.transform.DOScale(waterScale, introToWaterDuration / 2);
        intro2.SetEase(Ease.OutSine);



        Tweener intro1 = player.transform.DOMove(WaterLocation, introToWaterDuration / 2);
        intro1.SetEase(Ease.OutSine);

        intro1.OnComplete(delegate ()
        {
            player.enabled = true;
            onIntroAnimComplete();
        });


        return player;
    }


    public SubmarineMovement AnimatePlayerExit(SubmarineMovement player, TweenCallback onComplete)
    {
        // Physics
        player.enabled = false;

        Vector3 position = player.transform.position;
        position.y += 10;

        // Anim
        player.transform.DOMove(position,
            exitDuration).onComplete = onComplete;

        return player;
    }
}