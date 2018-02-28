using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BrainBehaviour : MonoBehaviour
{
    [SerializeField] private Brain exposedBrain;
    [SerializeField] private bool resetBrainOnEnable = true;

    private object brainData;
    private Rigidbody2D rb;
    private Brain brain;

    private bool hasAwaken = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SetNewBrain(exposedBrain);
        hasAwaken = true;
    }

    private void OnValidate()
    {
        if (hasAwaken && exposedBrain != brain)
            SetNewBrain(exposedBrain);
    }

    private void OnDrawGizmosSelected()
    {
        if (brain != null && hasAwaken)
            brain.DrawGizmosSelected(rb, brainData);
    }
    private void OnDrawGizmos()
    {
        if (brain != null && hasAwaken)
            brain.DrawGizmos(rb, brainData);
    }

    void OnEnable()
    {
        if (resetBrainOnEnable)
            Restart();
    }

    private void Update()
    {
        if (brain != null)
            brain.Tick(rb, Time.deltaTime, brainData);
    }

    public void SetNewBrain(Brain brain)
    {
        this.brain = brain;
        Restart();
    }


    public void Restart()
    {
        if (exposedBrain != null)
        {
            brainData = exposedBrain.NewInstanceData(rb, this);
        }
    }
}