using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalierSubscriber : MonoBehaviour
{
    private Palier currentPalier;
    private Transform tr;
    private bool AutoRevive { get { return GetComponent<PoolableUnit>() == null; } }

    void Awake()
    {
        tr = transform;
    }

    void OnDisable()
    {
        // Si nous autoRevive == false OU que le component lui-même est disabled
        if (Game.Instance != null && Game.PalierManager != null && (!AutoRevive || !enabled))
        {
            Unsubscribe();
        }
    }

    protected void Update()
    {
        CheckPalier();
    }

    private void CheckPalier()
    {
        var palierManager = Game.PalierManager;
        var closestPalierIndex = palierManager.PalierPlans.GetClosestPalier(tr.position.y);

        if (currentPalier == null || closestPalierIndex != currentPalier.Index)
        {
            SetPalier(palierManager.GetPalier(closestPalierIndex));
        }
    }

    private void SetPalier(Palier newPalier)
    {
        Unsubscribe();
        Subscribe(newPalier);
    }

    private void Unsubscribe()
    {
        if (currentPalier != null)
        {
            currentPalier.Unsubscribe(this);
            currentPalier = null;
        }
    }

    private void Subscribe(Palier newPalier)
    {
        currentPalier = newPalier;

        if (newPalier != null)
        {
            newPalier.Subscribe(this);
            if (!newPalier.IsActive)
                Suicide();
        }
        else
        {
            Suicide();
        }
    }

    public void OnPalierActivate()
    {
        if (AutoRevive)
            Revive();
    }

    public void OnPalierDeactivate()
    {
        Suicide();
    }

    private void Suicide()
    {
        var killable = GetComponent<BaseKillableUnit>();
        if (killable != null)
        {
            if (currentPalier != null)
                currentPalier.FishToRespawn++;

            killable.Kill();
        }
    }
    private void Revive()
    {
        var revivable = GetComponent<RevivableUnit>();
        if (revivable != null)
        {
            if (currentPalier != null)
                currentPalier.FishToRespawn--;

            revivable.Revive();
        }
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying && Game.Instance != null && Game.PalierManager != null && currentPalier != null && enabled)
        {
            var center = new Vector3(0, Game.PalierManager.PalierPlans.GetPalierCenter(currentPalier.Index), 0);
            var color = Color.magenta;
            Gizmos.color = color;
            Gizmos.DrawLine(transform.position, center);
        }
    }
}
