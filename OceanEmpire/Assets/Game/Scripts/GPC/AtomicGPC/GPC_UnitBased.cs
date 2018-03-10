using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    public abstract class GPC_UnitBased : AbstractGPCBehaviour
    {
        protected readonly SceneManager sceneManager;

        protected GameObject unit;

        #region TEMPORAIRE
        protected GameObject unitReference;
        protected Vector2 referencePosition;

        public GPC_UnitBased(SceneManager sceneManager, GameObject unitPrefab, Vector2 referencePosition)
        {
            this.sceneManager = sceneManager;
            unitReference = unitPrefab;
            this.referencePosition = referencePosition;
        }

        public GPC_UnitBased(SceneManager sceneManager, GameObject unit)
        {
            this.sceneManager = sceneManager;
            this.unit = unit;
        }
        #endregion

        public GPC_UnitBased(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public override void Launch()
        {
            SpawnUnitIfNull();
        }

        protected void SpawnUnitIfNull()
        {
            if (unit == null)
            {
                if (unitReference == null)
                {
                    Debug.LogError("Cannot spawn unit because reference is null");
                }
                else
                {
                    var spawner = sceneManager.Read<UnitSpawner>("unit spawner");
                    unit = spawner.Spawn(unitReference, referencePosition);
                    OnUnitSpawned();
                }
            }
        }

        protected virtual void OnUnitSpawned() { }
    }
}