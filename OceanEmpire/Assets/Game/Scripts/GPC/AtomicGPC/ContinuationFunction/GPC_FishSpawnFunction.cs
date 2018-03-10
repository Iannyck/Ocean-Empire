using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    public class GPC_FishSpawnFunction : ContinuationFunction
    {
        private readonly SceneManager sceneManager;
        private ParallelAND parallelFish;
        private GPC_FishSpawnCheck spawnCheck;

        public GPC_FishSpawnFunction( SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;

            parallelFish = new ParallelAND();
            spawnCheck = new GPC_FishSpawnCheck(sceneManager);
        }

        public override void Abort()
        {

        }

        public override GPCState Eval()
        {
            return GPCState.RUNNING;
        }

        public override void Launch()
        {

        }

        public override void Reset()
        {

        }
    }
}
