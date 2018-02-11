using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BrainBehaviour : MonoBehaviour
{
    [SerializeField] private Brain brain;

    private object brainData;
    private Rigidbody2D rb;
    private Brain currentBrain;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SetNewBrain(brain);
    }

    private void OnValidate()
    {
        if (Application.isPlaying && brain != currentBrain)
            SetNewBrain(brain);
    }

    public void SetNewBrain(Brain brain)
    {
        currentBrain = brain;
        ResetBrain();
    }

    public void ResetBrain()
    {
        if (brain != null)
        {
            brainData = brain.NewInstanceData(rb, this);
        }
    }

    private void Update()
    {
        if (currentBrain != null)
            currentBrain.Tick(rb, Time.deltaTime, brainData);
    }
}