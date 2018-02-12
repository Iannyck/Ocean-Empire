using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPool : MonoBehaviour
{
    [SerializeField] Transform spawnContainer;
    Dictionary<PoolableUnit, Queue<PoolableUnit>> DeactivatedUnits = new Dictionary<PoolableUnit, Queue<PoolableUnit>>();

    public PoolableUnit PlaceUnit(PoolableUnit prefab, Vector2 position)
    {
        // Get queue
        if (!DeactivatedUnits.ContainsKey(prefab))
            DeactivatedUnits.Add(prefab, new Queue<PoolableUnit>());
        var queue = DeactivatedUnits[prefab];

        // Get unit
        PoolableUnit unit = null;
        if (queue.Count <= 0)
        {
            unit = CreateNewCopy(prefab, position);
        }
        else
        {
            unit = queue.Dequeue();
            if (unit == null)
            {
                unit = CreateNewCopy(prefab, position);
                Debug.LogError("An inactive poolable unit was deleted. This should not happen.");
            }
            else
            {
                var tr = unit.transform;
                tr.position = position;
                tr.rotation = Quaternion.identity;
                unit.Revive();
            }
        }

        var capturable = unit.GetComponent<Capturable>();
        if (capturable != null)
            Game.FishingReport.KeepTrack(capturable);

        return unit;
    }

    private PoolableUnit CreateNewCopy(PoolableUnit prefab, Vector2 position)
    {
        var copy = Game.Spawner.Spawn(prefab, position);
        copy.originalCopy = prefab;
        copy.OnAllDeaths += AddBackToPool;
        return copy;
    }

    private void AddBackToPool(BaseKillableUnit killableUnit)
    {
        var poolable = killableUnit as PoolableUnit;

        if (poolable == null)
        {
            Debug.LogError("Une unit non-poolable est arrivé dans la pool");
            return;
        }

        DeactivatedUnits[poolable.originalCopy].Enqueue(poolable);
    }
}
