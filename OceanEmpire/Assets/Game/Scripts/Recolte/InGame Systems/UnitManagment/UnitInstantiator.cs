using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInstantiator : MonoBehaviour
{
    [SerializeField] UnitPool unitPool;

    private Transform tr;

    private void Awake()
    {
        tr = transform;
        unitPool.spawnFunc = RawSpawn;
    }

    public Transform GetSpawnContainer() { return tr; }

    public T Spawn<T>(T reference, Vector2 position) where T : MonoBehaviour
    {
        return Spawn(reference.gameObject, position).GetComponent<T>();
    }

    public GameObject Spawn(GameObject reference, Vector2 position)
    {
        if (reference == null)
            return null;

        var poolableUnit = reference.GetComponent<PoolableUnit>();
        if (unitPool != null && poolableUnit != null)
        {
            //Poolable Unit
            return unitPool.PlaceUnit(poolableUnit, position).gameObject;
        }
        else
        {
            //Non-Poolable Unit
            return RawSpawn(reference, position);
        }
    }

    private GameObject RawSpawn(GameObject reference, Vector2 position)
    {
        return Instantiate(reference, position, Quaternion.identity, tr);
    }
}
