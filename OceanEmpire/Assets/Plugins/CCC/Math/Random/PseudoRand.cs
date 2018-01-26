using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PseudoRand
{
    [SerializeField]
    private float currentChances;
    [SerializeField, Range(0, 1)]
    private float successRate;
    [SerializeField, Range(0, 1)]
    private float hardness;

    public float CurrentChances
    {
        get { return currentChances; }
    }
    public float SuccessRate
    {
        get { return successRate; }
        set { successRate = value.Clamped(0, 1); }
    }

    public float Hardness
    {
        get { return hardness; }
        set { hardness = value.Clamped(0, 1); }
    }

    public PseudoRand(float successRate, float hardness)
    {
        SuccessRate = successRate;
        this.hardness = hardness;
    }

    public void ResetChances()
    {
        currentChances = successRate;
    }

    public bool PickResult()
    {
        bool success = Random.Range(0, 1f) <= currentChances;

        if (success)
        {
            currentChances -= (1 - SuccessRate) * hardness;
        }
        else
        {
            currentChances += SuccessRate * hardness;
        }

        return success;
    }
}