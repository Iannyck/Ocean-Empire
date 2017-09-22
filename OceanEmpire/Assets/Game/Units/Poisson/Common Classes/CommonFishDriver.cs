using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonFishDriver : BaseFishDriver
{
    public enum Behaviour { Wander = 0, Flee = 1 }
    [Header("General")]
    public float maxSpeed = 1;
    public float acceleration = 3;
    public Behaviour defaultBehaviour;

    [Header("Flee")]
    public bool fleePlayer;
    public float fleeDistance;
    private float fleeDistanceSQR;

    [Header("Brain Freeze")]
    public bool canBrainFreeze;
    [MinMaxSlider(1, 15)]
    public Vector2 brainFreezeFrequence = new Vector2(4, 8);
    [MinMaxSlider(1, 6)]
    public Vector2 brainFreezeDuration = new Vector2(1.5f, 3);

    [Header("Wander")]
    public bool clampWander = true;
    public float changeDestIfCloserThan = 0.25f;
    private float changeDestIfCloserThanSQR;
    [MinMaxSlider(-90, 90)]
    public Vector2 angleRange = new Vector2(-45, 45);
    [MinMaxSlider(0.25f, 5)]
    public Vector2 distanceRange = new Vector2(1f, 2.5f);

    [HideInInspector]
    public Behaviour currentBehaviour;

    private Vector2 targetPosition;
    private bool brainFreeze = false;
    private float brainFreezeTimer;
    private float brainFreezeRemainingDuration;

    protected override void Awake()
    {
        base.Awake();

        changeDestIfCloserThanSQR = changeDestIfCloserThan * changeDestIfCloserThan;
        fleeDistanceSQR = fleeDistance * fleeDistance;
        targetPosition = Position;
    }

    private void Update()
    {
        //Update l'etat du brain freeze
        UpdateBrainFreeze(Time.deltaTime);

        //Variables
        Vector2 meToTargetPos = targetPosition - Position;
        float distSQR = meToTargetPos.sqrMagnitude;

        //Petit switch de comportement dépendemment de l'état.
        //Pas la peine de faire une machine a etat fini développé. (ça risquerait d'être plus lourd pour rien)
        switch (currentBehaviour)
        {
            case Behaviour.Wander:
                if (distSQR < changeDestIfCloserThanSQR)
                {
                    targetPosition = GetWanderPosition();
                }
                break;
            case Behaviour.Flee:
                break;
        }

        //Vitesse cible
        Vector2 targetSpeed = Vector2.zero;
        if (distSQR > 0.02 && (currentBehaviour == Behaviour.Flee || !brainFreeze))
        {
            targetSpeed = meToTargetPos.normalized * maxSpeed;
        }

        //On tend vers la vitesse cible
        Speed = Speed.MovedTowards(targetSpeed, Time.deltaTime * acceleration);
    }

    void UpdateBrainFreeze(float deltaTime)
    {
        if (canBrainFreeze)
        {
            if (brainFreeze)
            {
                //Decrease la duree restante du freeze
                if (brainFreezeRemainingDuration > 0)
                {
                    brainFreezeRemainingDuration -= deltaTime;
                }
                brainFreeze = brainFreezeRemainingDuration > 0;
            }
            else
            {
                //Decrease timer pour le prochain freeze
                if (brainFreezeTimer >= 0)
                {
                    brainFreezeTimer -= deltaTime;
                    if (brainFreezeTimer < 0)
                    {
                        //Nouveau brain freeze
                        brainFreeze = true;
                        brainFreezeRemainingDuration = Random.Range(brainFreezeDuration.x, brainFreezeDuration.y);
                        brainFreezeTimer = Random.Range(brainFreezeFrequence.x, brainFreezeFrequence.y);
                    }
                }
            }
        }
    }

    Vector2 GetWanderPosition()
    {
        //Génère un vecteur random
        Vector2 v = CCC.Math.Vectors.RandomVector2(angleRange.x, angleRange.y, distanceRange.x, distanceRange.y);
        if (Random.value > 0.5f)
            v = -v;


        Vector2 pos = v + Position;
        if (MapInfo.IsOutOfBounds(pos))
        {
            v = -v;
            pos = v + Position;
            pos = new Vector2(pos.x.Clamped(MapInfo.MAP_LEFT, MapInfo.MAP_RIGHT), pos.y);
        }

        return pos;
    }


}