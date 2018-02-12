using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BrainBehaviour : MonoBehaviour
{
    [SerializeField] private Brain brain;
    [SerializeField] private bool resetBrainOnEnable = true;

    private object brainData;
    private Rigidbody2D rb;
    private Brain currentBrain;

    private bool hasAwaken = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SetNewBrain(brain);
        hasAwaken = true;
    }

    private void OnValidate()
    {
        if (hasAwaken && brain != currentBrain)
            SetNewBrain(brain);
    }

    void OnEnable()
    {
        if (resetBrainOnEnable)
            Restart();
    }

    private void Update()
    {
        if (currentBrain != null)
            currentBrain.Tick(rb, Time.deltaTime, brainData);
    }

    public void SetNewBrain(Brain brain)
    {
        currentBrain = brain;
        Restart();
    }


    public void Restart()
    {
        if (brain != null)
        {
            brainData = brain.NewInstanceData(rb, this);
        }
    }
}