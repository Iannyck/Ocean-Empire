using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalierSubscriber : MonoBehaviour
{
    private int currentPalier = -1;

    void OnDrawGizmos()
    {
        if(Application.isPlaying && Game.Instance != null && Game.FishSpawner != null && currentPalier != -1)
        {
            var palierHeight = Game.FishSpawner.GetPalierPosition(currentPalier);
            var center = new Vector3(0, palierHeight, 0);
            var size = new Vector3(5, Game.FishSpawner.palierHeigth -0.25f, 0);
            var color = Color.magenta;
            Gizmos.color = color;
            Gizmos.DrawLine(transform.position, center);
            color.a = 0.35f;
            Gizmos.color = color;
            Gizmos.DrawCube(center, size);
        }
    }

    void OnDisable()
    {
        if (Game.Instance == null)
            return;

        if (Game.FishSpawner != null && currentPalier >= 0)
            UnsubscribePalier();
        currentPalier = -1;
    }

    protected virtual void Update()
    {
        CheckPalier();
    }


    private void CheckPalier()
    {
        var fishSpawner = Game.FishSpawner;

        float fishPosition = transform.position.y;
        int closestPalier = fishSpawner.GetClosestPalier(fishPosition);

        if (currentPalier == -1)
        {
            SetPalier(closestPalier);
            return;
        }
        if (currentPalier == closestPalier)
            return;

        if ((fishSpawner.GetPalierPosition(currentPalier) - fishPosition).Abs() > fishSpawner.palierHeigth)
        {
            SetPalier(closestPalier);
        }
    }

    private void SetPalier(int newPalierIte)
    {
        if (currentPalier == -1)
        {
            currentPalier = newPalierIte;
            SubscribePalier();
            return;
        }

        if (Game.FishSpawner.GetPalier(newPalierIte).isActive == false)
        {
            TryToKill();
            return;
        }

        UnsubscribePalier();
        currentPalier = newPalierIte;
        SubscribePalier();
    }

    private void TryToKill()
    {
        var killable = GetComponent<BaseKillableUnit>();
        if (killable != null)
            killable.Kill();
    }

    private void SubscribePalier()
    {
        FishPalier fiP = Game.FishSpawner.GetPalier(currentPalier);
        fiP.SubscribeFish(this);
        if (!fiP.isActive)
            TryToKill();
        fiP.palierDespawnEvent += TryToKill;
    }

    private void UnsubscribePalier()
    {
        FishPalier fiP = Game.FishSpawner.GetPalier(currentPalier);
        fiP.UnsubscribeFish(this);
        fiP.palierDespawnEvent -= TryToKill;
    }
}
