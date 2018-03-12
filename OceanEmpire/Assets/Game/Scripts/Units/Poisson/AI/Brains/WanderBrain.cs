using CCC.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Brain/Wander Brain")]
public class WanderBrain : Brain
{
    [Header("General"), SerializeField] float maxSpeed = 1;
    [SerializeField] float acceleration = 3;

    [Header("Brain Freeze"), SerializeField] bool canBrainFreeze;
    [MinMaxSlider(1, 15), SerializeField] Vector2 brainFreezeFrequence = new Vector2(4, 8);
    [MinMaxSlider(1, 6), SerializeField] Vector2 brainFreezeDuration = new Vector2(1.5f, 3);

    [Header("Wander"), SerializeField] float changeDestIfCloserThan = 0.25f;
    [MinMaxSlider(-90, 90), SerializeField] Vector2 angleRange = new Vector2(-45, 45);
    [MinMaxSlider(0.25f, 5), SerializeField] Vector2 distanceRange = new Vector2(1f, 2.5f);

    private class Data
    {
        public float changeDestIfCloserThanSQR = 0;
        public Vector2 targetPosition = Vector2.zero;
        public bool brainFreeze = false;
        public float brainFreezeTimer = 0;
        public float brainFreezeRemainingDuration = 0;
    }

    public override object NewInstanceData(Rigidbody2D rb, BrainBehaviour component)
    {
        Data data = new Data
        {
            changeDestIfCloserThanSQR = changeDestIfCloserThan * changeDestIfCloserThan,
            brainFreeze = false,
            brainFreezeTimer = Random.Range(brainFreezeFrequence.x, brainFreezeFrequence.y),
            brainFreezeRemainingDuration = 0,
            targetPosition = component.transform.position
        };

        return data;
    }

    public override void Tick(Rigidbody2D rb, float deltaTime, object perInstanceData)
    {
        base.Tick(rb, deltaTime, perInstanceData);
        Data data = (Data)perInstanceData;

        //Update l'etat du brain freeze
        UpdateBrainFreeze(rb, data, Time.deltaTime);

        //Variables
        Vector2 meToTargetPos = data.targetPosition - rb.position;
        float distSQR = meToTargetPos.sqrMagnitude;

        // Get new wander position if we're close
        if (distSQR < data.changeDestIfCloserThanSQR)
        {
            data.targetPosition = GetWanderPosition(rb, data);
        }

        //Vitesse cible
        Vector2 targetSpeed = Vector2.zero;
        if (distSQR > 0.02 && !data.brainFreeze)
        {
            targetSpeed = meToTargetPos.normalized * maxSpeed;
        }

        //On tend vers la vitesse cible
        rb.velocity = rb.velocity.MovedTowards(targetSpeed, Time.deltaTime * acceleration);
    }

    void UpdateBrainFreeze(Rigidbody2D rb, Data data, float deltaTime)
    {
        if (canBrainFreeze)
        {
            if (data.brainFreeze)
            {
                //Decrease la duree restante du freeze
                if (data.brainFreezeRemainingDuration > 0)
                {
                    data.brainFreezeRemainingDuration -= deltaTime;
                }
                data.brainFreeze = data.brainFreezeRemainingDuration > 0;
                if (!data.brainFreeze)
                    data.targetPosition = rb.position;
            }
            else
            {
                //Decrease timer pour le prochain freeze
                if (data.brainFreezeTimer >= 0)
                {
                    data.brainFreezeTimer -= deltaTime;
                    if (data.brainFreezeTimer < 0)
                    {
                        //Nouveau brain freeze
                        data.brainFreeze = true;
                        data.brainFreezeRemainingDuration = Random.Range(brainFreezeDuration.x, brainFreezeDuration.y);
                        data.brainFreezeTimer = Random.Range(brainFreezeFrequence.x, brainFreezeFrequence.y);
                    }
                }
            }
        }
    }

    Vector2 GetWanderPosition(Rigidbody2D rb, Data data)
    {
        //Génère un vecteur random
        Vector2 v = Vectors.RandomVector2(distanceRange.x, distanceRange.y, angleRange.x, angleRange.y);
        if (Random.value > 0.5f)
            v = -v;

        Vector2 pos = v + rb.position;
        //Debug.Log("ID: " + rb.GetInstanceID() + "    rb: " + rb.position + "  v: " + v + "  =  " + pos);
        if (MapInfo.IsOutOfBounds(pos))
        {
            // Flip direction
            v = -v;
            pos = v + rb.position;

            pos.x = Mathf.Clamp(pos.x, MapInfo.MAP_LEFT, MapInfo.MAP_RIGHT);
        }

        // Clamp vertical
        MapInfo map;
        if (Game.Instance != null && (map = Game.Instance.map) != null)
        {
            pos.y = Mathf.Clamp(pos.y, map.mapBottom + 0.5f, map.mapTop - 0.5f);
        }

        return pos;
    }

    public override void DrawGizmosSelected(Rigidbody2D rb, object perInstanceData)
    {
        base.DrawGizmosSelected(rb, perInstanceData);

        Data data = (Data)perInstanceData;

        if (!data.brainFreeze)
            DrawPathGizmos(rb.position, data.targetPosition);

    }
}