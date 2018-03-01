using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicElectrify : Electrify
{
    [Header("On/Off Toggle")]
    public float electricInterval = 2.5f;
    public float electricDuration = 2.5f;

    private Coroutine toggleRoutine;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (electrifiedOnStart)
            toggleRoutine = StartCoroutine(InvertedToggleRoutine());
        else
            toggleRoutine = StartCoroutine(ToggleRoutine());
    }

    /// <summary>
    /// Débute non électrifié, puis le deviens
    /// </summary>
    IEnumerator ToggleRoutine()
    {
        while (this != null)
        {
            Electrified = false;
            yield return new WaitForSeconds(electricInterval);
            Electrified = true;
            yield return new WaitForSeconds(electricDuration);
        }
    }

    /// <summary>
    /// Débute électrifié, puis ne le deviens plus
    /// </summary>
    IEnumerator InvertedToggleRoutine()
    {
        while (this != null)
        {
            Electrified = true;
            yield return new WaitForSeconds(electricDuration);
            Electrified = false;
            yield return new WaitForSeconds(electricInterval);
        }
    }


    void OnDisable()
    {
        if (toggleRoutine != null)
            StopCoroutine(toggleRoutine);
    }
}
